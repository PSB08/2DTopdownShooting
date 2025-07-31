using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class NexusHitScript : MonoBehaviour
    {
        [SerializeField] private RectTransform hitMessage;
        
        [Header("DoTween Settings")]
        [SerializeField] private float appearOffsetY = -100f;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private float stayDuration = 1.5f;
        [SerializeField] private Ease easeIn = Ease.OutBack;
        [SerializeField] private Ease easeOut = Ease.InBack;
        
        private Vector2 _originalAnchoredPos;

        private void Awake()
        {
            _originalAnchoredPos = hitMessage.anchoredPosition;
            
            hitMessage.anchoredPosition = _originalAnchoredPos + new Vector2(0, appearOffsetY);

            gameObject.SetActive(false);
        }

        public void HitMessage()
        {
            gameObject.SetActive(true);
            
            hitMessage.anchoredPosition = _originalAnchoredPos + new Vector2(0, appearOffsetY);

            Sequence seq = DOTween.Sequence();
            seq.Append(hitMessage.DOAnchorPosY(_originalAnchoredPos.y, moveDuration).SetEase(easeIn));

            seq.AppendInterval(stayDuration);

            seq.Append(hitMessage.DOAnchorPosY(_originalAnchoredPos.y + appearOffsetY, moveDuration).SetEase(easeOut));

            seq.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        
    }
}