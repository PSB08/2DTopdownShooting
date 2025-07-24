using System;
using UnityEngine;

namespace Code.Scripts.Core
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private RectTransform map;
        [SerializeField] private Vector3 smallScale = Vector3.one;
        [SerializeField] private Vector3 bigScale = new Vector3(2f, 2f, 2f);
        [SerializeField] private Vector3 smallPosition = new Vector3(-150, 150, 0);
        [SerializeField] private Vector3 bigPosition = new Vector3(-300, 300, 0);
        private bool isBig;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
                ToggleMap();
        }

        private void ToggleMap()
        {
            isBig = !isBig;
            map.localScale = isBig ? bigScale : smallScale;
            map.anchoredPosition = isBig ? bigPosition : smallPosition;
        }
        
        
    }
}