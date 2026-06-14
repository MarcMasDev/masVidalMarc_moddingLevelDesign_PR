using Unity.Cinemachine;
using UnityEngine;

namespace Scripts
{
    public class CinemachineSpeedBasedDistance : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private Rigidbody _targetRigidbody;
    
        [Header("Distance Settings")]
        [SerializeField] private float _baseDistance = 7.5f;
        [SerializeField] private float _maxDistance = 15f;
        [SerializeField] private float _minSpeed = 0f;
        [SerializeField] private float _maxSpeed = 20f;
    
        [Header("Smoothing")]
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private AnimationCurve _speedToDistanceCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
        private CinemachineFollow _follow;
        private float _currentDistance;
        private float _distanceVelocity;
        private Vector3 _originalOffsetDirection;
        private float _originalDistance;
    
        void Start()
        {
            _follow = _camera.GetComponent<CinemachineFollow>();
        
            Vector3 originalOffset = _follow.FollowOffset;
            _originalDistance = originalOffset.magnitude;
            _originalOffsetDirection = originalOffset.normalized;
        
            _currentDistance = _baseDistance;
        }
    
        void LateUpdate()
        {
            var speed = _targetRigidbody.linearVelocity.magnitude;
            var normalizedSpeed = Mathf.Clamp01((speed - _minSpeed) / (_maxSpeed - _minSpeed));
            var curveValue = _speedToDistanceCurve.Evaluate(normalizedSpeed);
        
            var targetDistance = Mathf.Lerp(_baseDistance, _maxDistance, curveValue);
        
            _currentDistance = Mathf.SmoothDamp(
                _currentDistance, 
                targetDistance, 
                ref _distanceVelocity, 
                _smoothTime
            );
        
            _follow.FollowOffset = _originalOffsetDirection * _currentDistance;
        }
    }
}