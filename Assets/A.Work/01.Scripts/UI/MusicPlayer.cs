using Ami.BroAudio;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private SoundID sound;

        private void Start()
        {
            BroAudio.Play(sound);
        }

        public void StopMusic()
        {
            BroAudio.Stop(sound);
        }
        
        
    }
}