using System.Collections.Generic;
using Code.Scripts.Entities;
using Code.Scripts.Players;
using PSB_Lib.Dependencies;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Item
{
    [Provide]
    public class PlayerLevelUpUI : MonoBehaviour, IEntityComponent, IDependencyProvider
    {
        [SerializeField] private List<Transform> spawnTrans;
        [SerializeField] private List<GameObject> skillSelectUI;

        public UnityEvent OnValueChanged;
        
        private Player _player;
        
        private bool _selectionMade = false;
        private List<GameObject> _activeSkillUIs = new List<GameObject>();
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }

        public void HandleLevelUpUIShow()
        {
            _player.PlayerInput.IsCanAttack = false;
            Time.timeScale = 0;
            _selectionMade = false;
            _activeSkillUIs.Clear();

            List<int> skillIndices = new List<int>();
            List<int> spawnIndices = new List<int>();

            for (int i = 0; i < skillSelectUI.Count; i++) skillIndices.Add(i);
            for (int i = 0; i < spawnTrans.Count; i++) spawnIndices.Add(i);

            Shuffle(skillIndices);
            Shuffle(spawnIndices);

            for (int i = 0; i < 3; i++)
            {
                int skillIdx = skillIndices[i];
                int spawnIdx = spawnIndices[i];

                GameObject instance = Instantiate(skillSelectUI[skillIdx], transform.position, Quaternion.identity);
                instance.transform.SetParent(spawnTrans[spawnIdx], false);

                _activeSkillUIs.Add(instance);

                var button = instance.GetComponentInChildren<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => OnSkillSelected(instance));
                }
            }
        }

        private void OnSkillSelected(GameObject selectedUI)
        {
            if (_selectionMade)
                return; 

            _selectionMade = true;
            
            // 여기서 적용 로직 호출 가능
            LevelUpItem item = selectedUI.gameObject.GetComponent<LevelUpItem>();
            item.ApplyItem(_player);
            
            OnValueChanged?.Invoke();
            
            foreach (var ui in _activeSkillUIs)
            {
                Destroy(ui);
            }
            _activeSkillUIs.Clear();

            Time.timeScale = 1;
            _player.PlayerInput.IsCanAttack = true;
        }
        
        private void Shuffle(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
        
    }
}