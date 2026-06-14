using UnityEngine;

namespace Scripts
{
    public class TowerProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _maxRotationSpeed;
        [SerializeField] private float _bulletLifeTime;

        [Header("Force to target")]
        [SerializeField] private float _upwardForce = 5f;
        [SerializeField] private float _knockbackForce = 10f;

        private Transform _target;

        private void Start()
        {
            _target = Finder.Instance.Player.transform;
            Destroy(gameObject, _bulletLifeTime);
        }

        private void Update()
        {
            // Calculate direction to target
            Vector3 direction = (_target.position - transform.position).normalized;

            // Calculate the rotation needed to look at target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Gradually rotate towards target at max rotation speed (degrees per second)
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _maxRotationSpeed * Time.deltaTime
            );

            // Move forward in current facing direction
            transform.position += transform.forward * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                Destroy(gameObject);
                return;
            }

            var contactPoint = other.ClosestPoint(transform.position);

            // Calculate knockback direction from contact point to player center
            var knockbackDir = (other.transform.position - contactPoint).normalized;
            knockbackDir.y = 0; // Remove vertical component for pure horizontal push

            // Combine horizontal knockback with upward force
            var finalForce = (knockbackDir * _knockbackForce) + (Vector3.up * _upwardForce);
            
            var boxPlayer = other.GetComponent<BoxPlayer>();
            boxPlayer.AddForce(finalForce);
            
            Destroy(gameObject);
        }
    }
}