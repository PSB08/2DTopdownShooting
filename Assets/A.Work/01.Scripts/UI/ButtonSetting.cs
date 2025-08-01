using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class ButtonSetting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float targetScale = 1.2f;
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private Ease easeType = Ease.OutBack;

        [SerializeField] private Color hoverColor = Color.white;
        private Color _originalColor;

        [SerializeField] private bool canColorChange = false;

        private Vector3 _originalScale;
        private Image _image;

        private void Awake()
        {
            _originalScale = transform.localScale;

            _image = GetComponent<Image>();
            if (_image != null)
            {
                _originalColor = _image.color;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(_originalScale * targetScale, duration).SetEase(easeType).SetUpdate(true);

            if (canColorChange && _image != null)
            {
                _image.DOColor(hoverColor, duration).SetUpdate(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(_originalScale, duration).SetEase(easeType).SetUpdate(true);

            if (canColorChange && _image != null)
            {
                _image.DOColor(_originalColor, duration).SetUpdate(true);
            }
        }
        
    }
}