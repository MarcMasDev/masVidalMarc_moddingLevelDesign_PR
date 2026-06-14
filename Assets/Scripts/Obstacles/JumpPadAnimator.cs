using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = PrimeTween.Sequence;

namespace Scripts
{
    public class JumpPadAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _jumpPadPivot;
        [SerializeField] private float _speed;
        [SerializeField] private float _delay;

        [SerializeField] private Vector3 _startRotation; 
        [SerializeField] private Vector3 _endRotation; 

        private Sequence _tween;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_tween.isAlive)
            {
                _tween = Sequence.Create()
                    .Insert(_delay, Tween.LocalRotation(_jumpPadPivot, _endRotation, _speed, ease: Ease.OutBounce))
                    .Insert(_delay + _speed + 0.3f, Tween.LocalRotation(_jumpPadPivot, _startRotation, _speed));
            }
        }
    }
}
