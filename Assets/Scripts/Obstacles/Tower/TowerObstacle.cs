using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class TowerObstacle : MonoBehaviour
    {
        [SerializeField] private float _attackRange;
        [SerializeField] private float _burstDelay;
        [SerializeField] private float _numberOfBulletsPerBurst;
        [SerializeField] private float _bulletDelay;
        [SerializeField] private float _towerRotationSpeed;
        [SerializeField] private TowerProjectile _projectile;
        [SerializeField] private Transform _spawnPoint;

        private float _attackRangeSqr;
        private bool _wasInRangeLastFrame;

        private Transform _target;
        private bool _shooting;

        public event Action Shooted;

        private void Start()
        {
            _target = Finder.Instance.Player;
            _attackRangeSqr = _attackRange * _attackRange;

            if (_target == null)
            {
                Debug.LogError($"[Tower {gameObject.name}] Player not found in Finder!", this);
                return;
            }

            Debug.Log($"[Tower {gameObject.name}] Started. Attack range: {_attackRange}, Range²: {_attackRangeSqr}");

            var direction = _target.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void Update()
        {
            if (_target == null)
            {
                Debug.LogWarning($"[Tower {gameObject.name}] No target in Update!", this);
                return;
            }

            var direction = (_target.position - transform.position);
            direction.y = 0;
            direction = direction.normalized;

            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _towerRotationSpeed * Time.deltaTime
            );

            bool isInRange = IsInRange();

            // Only log on state changes
            if (isInRange && !_wasInRangeLastFrame)
            {
                Debug.Log($"[Tower {gameObject.name}] Player ENTERED range - starting shooting");
            }
            else if (!isInRange && _wasInRangeLastFrame)
            {
                Debug.Log($"[Tower {gameObject.name}] Player EXITED range - stopping shooting");
            }

            if (isInRange && !_shooting)
            {
                StopAllCoroutines();
                _shooting = false;
                StartCoroutine(nameof(Shoot));
            }

            _wasInRangeLastFrame = isInRange;
        }


        private bool IsInRange()
        {
            if (_target == null)
                return false;

            float distanceSqr = Vector3.SqrMagnitude(_target.position - transform.position);
            bool inRange = distanceSqr < _attackRangeSqr;

            // Uncomment for detailed range checking every frame (very spammy)
            // Debug.Log($"[Tower {gameObject.name}] Distance²: {distanceSqr:F2}, Range²: {_attackRangeSqr:F2}, InRange: {inRange}");

            return inRange;
        }

        private IEnumerator Shoot()
        {
            Debug.Log($"[Tower {gameObject.name}] Shoot coroutine started.");

            _shooting = true;
            while (true)
            {
                Debug.Log($"[Tower {gameObject.name}] Starting burst of {_numberOfBulletsPerBurst} bullets.");

                for (int i = 0; i < _numberOfBulletsPerBurst; i++)
                {
                    if (i != 0)
                        yield return new WaitForSeconds(_bulletDelay);
                    
                    SpawnBullet();
                }
                
                if(!IsInRange())
                    yield break;
                
                Debug.Log($"[Tower {gameObject.name}] Burst complete. Waiting {_burstDelay}s before next burst.");
                yield return new WaitForSeconds(_burstDelay);
            }
        }

        private void SpawnBullet()
        {
            if (_projectile == null || _spawnPoint == null)
            {
                Debug.LogError($"[Tower {gameObject.name}] Cannot spawn bullet - missing projectile or spawn point!", this);
                return;
            }

            float distanceToPlayer = Vector3.Distance(_target.position, transform.position);
            Debug.Log($"[Tower {gameObject.name}] Spawning bullet at {_spawnPoint.position}. Player distance: {distanceToPlayer:F2}m");
            Instantiate(_projectile, _spawnPoint.position, _spawnPoint.rotation);
            Shooted?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}