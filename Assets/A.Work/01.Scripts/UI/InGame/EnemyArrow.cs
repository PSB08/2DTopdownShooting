using System.Collections.Generic;
using Code.Scripts.Enemies;
using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class EnemyArrow : MonoBehaviour
    {
        [SerializeField] private RectTransform arrowPrefab;
        [SerializeField] private RectTransform arrowParent; // Canvas 상단에 올릴 부모
        [SerializeField] private Transform player;
        [SerializeField] private Camera mainCam;
        [SerializeField] private float arrowDistance = 300f;

        private Dictionary<Enemy, RectTransform> _activeArrows = new();

        public void AddEnemy(Enemy enemy)
        {
            if (_activeArrows.ContainsKey(enemy)) return;

            RectTransform arrow = Instantiate(arrowPrefab, arrowParent);
            arrow.gameObject.SetActive(true);
            _activeArrows.Add(enemy, arrow);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            if (_activeArrows.TryGetValue(enemy, out var arrow))
            {
                Destroy(arrow.gameObject);
                _activeArrows.Remove(enemy);
            }
        }

        private void LateUpdate()
        {
            foreach (var pair in _activeArrows)
            {
                Enemy enemy = pair.Key;
                RectTransform arrow = pair.Value;

                if (enemy == null)
                {
                    arrow.gameObject.SetActive(false);
                    continue;
                }

                Vector3 screenPos = mainCam.WorldToViewportPoint(enemy.transform.position);
                bool isOffScreen = screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1 ||
                               screenPos.z < 0;

                if (isOffScreen)
                {
                    Vector2 dir = (enemy.transform.position - player.position).normalized;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    arrow.rotation = Quaternion.Euler(0, 0, angle - 90);

                    Vector2 screenCenter = new(Screen.width / 2, Screen.height / 2);
                    Vector2 screenPos2D = screenCenter + dir * arrowDistance;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        arrowParent, screenPos2D, mainCam, out Vector2 localPos);
                    arrow.anchoredPosition = localPos;

                    arrow.gameObject.SetActive(true);
                }
                else
                {
                    arrow.gameObject.SetActive(false);
                }
            }
        }
    
    
    }
}