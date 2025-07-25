using System;
using System.Collections;
using UnityEngine;
using Ami.Extension;
using static Ami.BroAudio.Utility;

namespace Ami.BroAudio.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public partial class AudioPlayer : MonoBehaviour, IAudioPlayer, IPlayable, IRecyclable<AudioPlayer>
    {
        public delegate void PlaybackHandover(int id, InstanceWrapper<AudioPlayer> wrapper, PlaybackPreference pref, EffectType effectType, float trackVolume, float pitch);

        public PlaybackHandover OnPlaybackHandover;
        [Obsolete]
        public event Action<SoundID> OnEndPlaying
        {
            add => _onEnd += value;
            remove => _onEnd -= value;
        }

        private PlaybackPreference _pref;
        private StopMode _stopMode = default;
        private Coroutine _playbackControlCoroutine = null;

        private event Action<SoundID> _onEnd = null;
        private event Action<IAudioPlayer> _onUpdate = null;
        private event Action<IAudioPlayer> _onStart = null;

        public int PlaybackStartingTime { get; private set; }

        public void SetPlaybackData(int id, PlaybackPreference pref)
        {
            ID = id;
            _pref = pref;
        }

        public void Play()
        {
            if(IsStopping || ID <= 0 || _pref.Entity == null)
            {
                return;
            }

            if(PlaybackStartingTime == 0)
            {
                PlaybackStartingTime = TimeExtension.UnscaledCurrentFrameBeganTime;
            }

            try
            {
                if (_stopMode == StopMode.Stop)
                {
                    _clip = _pref.PickNewClip();
                }

                this.StartCoroutineAndReassign(PlayControl(), ref _playbackControlCoroutine);
            }
            catch (Exception ex)
            {
                ClearEvents();
                EndPlaying();
                Debug.LogException(ex);
            }
        }

        private IEnumerator PlayControl()
        {
            if (!SoundManager.Instance.TryGetAudioTypePref(ID.ToAudioType(), out var audioTypePref))
            {
                Debug.LogError(LogTitle + $"The ID:{ID} is invalid");
                yield break;
            }

            if (!Mathf.Approximately(audioTypePref.Volume, DefaultTrackVolume) && !_audioTypeVolume.IsFading)
            {
                _audioTypeVolume.Complete(audioTypePref.Volume, false);
            }
            _clipVolume.Complete(0f, false);
            int sampleRate = _clip.GetAudioClip().frequency;
            bool hasScheduled = false;
            if (_stopMode == StopMode.Stop) // we only do this process when it's fresh
            {
                AudioSource.clip = _clip.GetAudioClip();
                AudioSource.priority = _pref.Entity.Priority;

                SetPlayPosition(sampleRate);
                SetInitialPitch(_pref.Entity, audioTypePref);
                SetSpatial(_pref);

                if (IsDominator)
                {
                    TrackType = AudioTrackType.Dominator;
                }
                else
                {
                    SetEffect(audioTypePref.EffectType, SetEffectMode.Add);
                }

                SetScheduleTime(out hasScheduled);
                if(hasScheduled)
                {
                    yield return WaitForScheduledStartTime();
                }

                if (_decorators.TryGetDecorator<MusicPlayer>(out var musicPlayer))
                {
                    AudioSource.reverbZoneMix = 0f;
                    AudioSource.priority = AudioConstant.HighestPriority;
                    musicPlayer.DoTransition(ref _pref);
                    while (musicPlayer.IsWaitingForTransition)
                    {
                        yield return null;
                    }
                }
#if !UNITY_WEBGL
                AudioTrack = MixerPool.GetTrack(TrackType); 
#endif
            }
            
            do
            {
                if(!hasScheduled)
                {
                    StartPlaying(sampleRate);
                }
                float targetClipVolume = _clip.Volume * _pref.Entity.GetMasterVolume();
                float elapsedTime = 0f;

                #region FadeIn
                if (HasFading(_clip.FadeIn, _pref.FadeIn, out float fadeIn))
                {
                    _clipVolume.SetTarget(targetClipVolume);
                    while (_clipVolume.Update(ref elapsedTime, fadeIn, _pref.FadeInEase))
                    {
                        yield return null;
                        if (!OnUpdate())
                        {
                            yield break;
                        }
                    }
                }
                else
                {
                    _clipVolume.Complete(targetClipVolume);
                }
                #endregion

                if (_pref.IsLoop(LoopType.SeamlessLoop))
                {
                    _pref.ScheduledStartTime = 0d;
                    _pref.ApplySeamlessFade();
                }

                #region FadeOut
                int endSample = AudioSource.clip.samples - GetSample(sampleRate, _clip.EndPosition);
                if (HasFading(_clip.FadeOut, _pref.FadeOut, out float fadeOut))
                {
                    while (endSample - AudioSource.timeSamples > fadeOut * sampleRate)
                    {
                        yield return null;
                        if (!OnUpdate())
                        {
                            yield break;
                        }
                    }
                    
                    TriggerPlaybackHandover();
                    _clipVolume.SetTarget(0f);
                    elapsedTime = 0f;
                    IsFadingOut = true;
                    while (_clipVolume.Update(ref elapsedTime, fadeOut, _pref.FadeOutEase))
                    {
                        yield return null;
                        if (!OnUpdate())
                        {
                            yield break;
                        }
                    }
                    IsFadingOut = false;
                }
                else
                {
                    bool hasPlayed = false;
                    while(!HasEndPlaying(ref hasPlayed, endSample, sampleRate))
                    {
                        yield return null;
                        if (!OnUpdate())
                        {
                            yield break;
                        }
                    }
                    TriggerPlaybackHandover();
                }
                #endregion

                if (_pref.IsLoop(LoopType.Loop))
                {
                    _pref.ResetFading();
                }
                hasScheduled = false;
            } while (_pref.IsLoop(LoopType.Loop) && CanLoopIfIsChainedMode());

            EndPlaying();
        }

        private void StartPlaying(int sampleRate)
        {
            switch (_stopMode)
            {
                case StopMode.Stop:
                    PlayFromPos(sampleRate);
                    break;
                case StopMode.Pause:
                    AudioSource.UnPause();
                    break;
                case StopMode.Mute:
                    if (!AudioSource.isPlaying)
                    {
                        PlayFromPos(sampleRate);
                    }
                    break;
            }
            _stopMode = default;
            _onStart?.Invoke(this);
            _onUpdate?.Invoke(this);
            _onStart = null;
        }

        private void PlayFromPos(int sampleRate)
        {
            SetPlayPosition(sampleRate);
            AudioSource.Play();
        }

        private void SetPlayPosition(int sampleRate)
        {
            AudioSource.Stop();
            AudioSource.timeSamples = GetSample(sampleRate, _clip.StartPosition);
        }

        // more accurate than AudioSource.isPlaying
        private bool HasEndPlaying(ref bool hasPlayed, int endSample, int sampleRate)
        {
            int currentSample = AudioSource.timeSamples;
            int startSample = GetSample(sampleRate, _clip.StartPosition);
            if (!hasPlayed)
            {
                hasPlayed = currentSample > startSample;
            }

            return hasPlayed && (currentSample <= startSample || currentSample >= endSample);
        }

        private void TriggerPlaybackHandover(bool isEnd = false)
        {
            if ((isEnd && !_pref.CanHandoverToEnd()) || (!isEnd && !_pref.CanHandoverToLoop()))
            {
                return;
            }

            var newPref = _pref;
            if (newPref.IsChainedMode())
            {
                newPref.ChainedModeStage = isEnd ? PlaybackStage.End : PlaybackStage.Loop;
            }

            ClearScheduleEndEvents(); // it should be rescheduled in the new player
            OnPlaybackHandover?.Invoke(ID, _instanceWrapper, newPref, CurrentActiveEffects, _trackVolume.Target, StaticPitch);
            OnPlaybackHandover = null;
            _instanceWrapper = null; // the instance has been transferred to the new player
        }

        #region Stop Overloads
        void IAudioStoppable.Pause()
            => this.Pause(UseEntitySetting);
        void IAudioStoppable.Pause(float fadeOut)
            => Stop(fadeOut, StopMode.Pause, null);
        void IAudioStoppable.UnPause()
            => this.UnPause(UseEntitySetting);
        void IAudioStoppable.UnPause(float fadeIn)
        {
            _pref.FadeIn = fadeIn;
            Play();
        }
        void IAudioStoppable.Stop()
            => this.Stop(UseEntitySetting);
        void IAudioStoppable.Stop(float fadeOut)
            => this.Stop(fadeOut, null);
        void IAudioStoppable.Stop(Action onFinished)
            => this.Stop(UseEntitySetting, onFinished);
        void IAudioStoppable.Stop(float fadeOut, Action onFinished)
            => Stop(fadeOut, StopMode.Stop, onFinished);
        #endregion
        public void Stop(float overrideFade, StopMode stopMode, Action onFinished)
        {
            if (IsStopping && !Mathf.Approximately(overrideFade, Immediate))
            {
                return;
            }

            bool isPlaying = AudioSource.isPlaying;
            if(stopMode == StopMode.Pause && !isPlaying)
            {
                return;
            }

            if (ID <= 0 || !isPlaying)
            {
                onFinished?.Invoke();
                EndPlaying();
                return;
            }

            this.StartCoroutineAndReassign(StopControl(overrideFade, stopMode, onFinished), ref _playbackControlCoroutine);
        }

        private IEnumerator StopControl(float overrideFade, StopMode stopMode, Action onFinished)
        {
            _stopMode = stopMode;
            IsStopping = true;
            
            TriggerPlaybackHandover(isEnd: true);
            #region FadeOut
            if (HasFading(_clip.FadeOut,overrideFade,out float fadeTime))
            {
                if (IsFadingOut)
                {
                    // if is fading out. then don't stop. just wait for it
                    AudioClip clip = AudioSource.clip;
                    float endSample = clip.samples - (_clip.EndPosition * clip.frequency);
                    while(AudioSource.timeSamples < endSample)
                    {
                        yield return null;
                        if(!OnUpdate())
                        {
                            yield break;
                        }
                    }
                }
                else
                {
                    float elapsedTime = 0f;
                    _clipVolume.SetTarget(0f);
                    while(_clipVolume.Update(ref elapsedTime, fadeTime, SoundManager.FadeOutEase))
                    {
                        yield return null;
                        if (!OnUpdate())
                        {
                            yield break;
                        }
                    }
                }
            }
            #endregion
            switch (stopMode)
            {
                case StopMode.Stop: 
                    EndPlaying();
                    break;
                case StopMode.Pause:
                    AudioSource.Pause();
                    break;
                case StopMode.Mute:
                    this.SetVolume(0f);
                    break;
            }
            IsStopping = false;
            onFinished?.Invoke();
        }

        private static bool HasFading(float clipFade, float overrideFade, out float fadeTime)
        {
            fadeTime = clipFade;
            if (overrideFade >= 0f)
            {
                fadeTime = overrideFade;
            }
            return fadeTime > Immediate;
        }

        private bool OnUpdate()
        {
            _onUpdate?.Invoke(this);
            return IsActive;
        }

        private void EndPlaying()
        {
            PlaybackStartingTime = 0;
            _stopMode = default;
            _pref = default;
            IsFadingOut = false;
            IsStopping = false;
            ResetVolume();
            ResetPitch();
            
            AudioSource.Stop();
            AudioSource.clip = null;
            _clip = null;
            ResetSpatial();
            ResetEffect();

            // Don't add StopCoroutine(_playbackCoroutine) here, as this method is typically called within it, and further processing after this method cannot be guaranteed.
            _trackVolume.StopCoroutine();
            _audioTypeVolume.StopCoroutine();

            _onEnd?.Invoke(ID);
            _onEnd = null;

            Recycle();
        }
        
        private bool CanLoopIfIsChainedMode()
        {
            return !_pref.IsChainedMode() || (_pref.IsChainedMode() && _pref.ChainedModeStage == PlaybackStage.Loop);
        }

        public IAudioPlayer OnEnd(Action<SoundID> onEnd)
        {
            _onEnd -= onEnd;
            _onEnd += onEnd;
            return this;
        }

        public IAudioPlayer OnUpdate(Action<IAudioPlayer> onUpdate)
        {
            _onUpdate -= onUpdate;
            _onUpdate += onUpdate;
            return this;
        }

        public IAudioPlayer OnStart(Action<IAudioPlayer> onStart)
        {
            _onStart -= onStart;
            _onStart += onStart;
            return this;
        }
    }
}
