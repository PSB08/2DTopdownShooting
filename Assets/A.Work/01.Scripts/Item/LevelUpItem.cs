using Code.Scripts.Entities;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Item
{
    public abstract class LevelUpItem : MonoBehaviour
    {
        [SerializeField] protected LevelUpItemSO _levelUpItemSO;
        
        [SerializeField] protected Image skillIcon;
        [SerializeField] protected TextMeshProUGUI skillName;
        [SerializeField] protected TextMeshProUGUI skillDescription;

        [SerializeField] protected float changeValue;

        private void Awake()
        {
            skillIcon.sprite = _levelUpItemSO.SkillIcon;
            skillName.text = _levelUpItemSO.Name;
            skillDescription.text = _levelUpItemSO.Description;
        }

        public virtual void ApplyItem(Entity targetEntity)
        {
        }

        protected virtual void SizeUp(Entity targetEntity)
        {
            Vector3 trans = targetEntity.transform.localScale;
            
            transform.DOScale(trans * 1.2f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    transform.DOScale(trans, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
                });
        }
        
    }
}