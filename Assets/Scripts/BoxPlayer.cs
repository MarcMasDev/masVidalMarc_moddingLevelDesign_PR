using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts
{
    public class BoxPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        [Button]
        public void AddForce(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private void Awake()
        {
            Finder.Instance.AddPlayerReference(transform);
        }
    }
}