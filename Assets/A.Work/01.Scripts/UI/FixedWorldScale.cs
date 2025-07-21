using UnityEngine;

namespace Code.Scripts.UI
{
    public class FixedWorldScale : MonoBehaviour
    {
        [SerializeField] private Vector3 targetWorldScale = new Vector3(1f, 1f, 1f);

        private void LateUpdate()
        {
            if (transform.parent == null) return;

            Vector3 parentScale = transform.parent.lossyScale;
            transform.localScale = new Vector3(
                targetWorldScale.x / parentScale.x,
                targetWorldScale.y / parentScale.y,
                targetWorldScale.z / parentScale.z
            );
        }
    }
}