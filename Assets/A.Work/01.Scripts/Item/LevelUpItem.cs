using Code.Scripts.Entities;
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

        private void Awake()
        {
            skillIcon.sprite = _levelUpItemSO.SkillIcon;
            skillName.text = _levelUpItemSO.Name;
            skillDescription.text = _levelUpItemSO.Description;
        }

        public virtual void ApplyItem(Entity targetEntity)
        {
        }
        
    }
}