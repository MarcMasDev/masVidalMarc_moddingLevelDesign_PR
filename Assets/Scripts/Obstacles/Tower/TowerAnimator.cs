using System;
using UnityEngine;

namespace Scripts
{
    public class TowerAnimator : MonoBehaviour
    {
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        [SerializeField] private Animator _animator;
        private TowerObstacle _towerObstacle;

        private void Awake()
        {
            _towerObstacle = GetComponent<TowerObstacle>();
            _towerObstacle.Shooted += OnShooted;
        }

        private void OnDestroy()
        {
            _towerObstacle.Shooted -= OnShooted;
        }

        private void OnShooted()
        {
            _animator.SetTrigger(Shoot);
        }
    }
}