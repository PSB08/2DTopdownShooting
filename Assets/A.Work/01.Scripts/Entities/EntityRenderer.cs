using Code.Scripts.Enemies.BT;
using UnityEngine;

namespace Code.Scripts.Entities
{
    public class EntityRenderer : MonoBehaviour, IBtEntityComponent
    {
        [field: SerializeField]public float FacingDirection { get; private set; } = 1f;
        
        [SerializeField] private Animator animator;
        
        private IComponentOwner _owner;
        private AnimParamSO _currClip;

        public void ChangeClip(AnimParamSO nextClip)
        {
            if (_currClip != null)
            {
                SetParam(_currClip, false);
            }
            
            _currClip = nextClip;
            SetParam(_currClip, true);
        }

        public void Initialize(IComponentOwner owner)
        {
            _owner = owner;
        }
        
        #region Animator Parameter Section

        public void SetParam(AnimParamSO param, bool value) => animator.SetBool(param.paramHash, value);
        public void SetParam(AnimParamSO param, float value) => animator.SetFloat(param.paramHash, value);
        public void SetParam(AnimParamSO param, int value) => animator.SetInteger(param.paramHash, value);
        public void SetParam(AnimParamSO param) => animator.SetTrigger(param.paramHash);

        #endregion

        #region Character Flip Controller Section

        public void FlipController(float xMove)
        {
            if (Mathf.Abs(FacingDirection + xMove) < 0.5f)
            {
                Flip();
            }
        }
    
        public void Flip()
        {
            FacingDirection *= -1;
            float targetYAngle = FacingDirection > 0 ? 180f : 0;
            _owner.Transform.rotation = Quaternion.Euler(0f, targetYAngle, 0f);
        }

        #endregion
        
        
    }
}