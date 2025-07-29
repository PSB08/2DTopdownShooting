using UnityEngine;

namespace Code.Scripts.Item
{
    [CreateAssetMenu(fileName = "LevelUpItem", menuName = "SO/Item", order = 0)]
    public class LevelUpItemSO : ScriptableObject
    {
        public Sprite SkillIcon;
        public string Name;
        public string Description;
        
        [System.NonSerialized]
        public int selectCount;
    }
}