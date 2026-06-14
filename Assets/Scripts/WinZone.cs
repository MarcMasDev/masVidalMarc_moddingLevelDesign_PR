using System;
using UnityEngine;

namespace Scripts
{
    public class WinZone : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("OpenDoor");
        [SerializeField] private Animator _door;
        
        public event Action Win;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _door.SetTrigger(Open);
                Win?.Invoke();
            }
        }
    }
}