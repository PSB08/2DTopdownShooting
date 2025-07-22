using System;
using UnityEngine;

namespace Code.Scripts.Core
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private RectTransform map;

        private bool _isBig = false;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M) && !_isBig)
            {
                _isBig = true;
                map.localScale = new Vector3(2f, 2f, 2f);
                map.anchoredPosition = new Vector3(-300, 300, 0);
            }
            else if (Input.GetKeyDown(KeyCode.M) && _isBig)
            {
                _isBig = false;
                map.localScale = new Vector3(1, 1, 1);
                map.anchoredPosition = new Vector3(-150, 150, 0);
            }
        }
        
        
    }
}