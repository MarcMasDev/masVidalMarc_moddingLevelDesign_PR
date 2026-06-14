using UnityEngine;

namespace Scripts
{
    public class BombBehaviour: MonoBehaviour
    {
        public GameObject _explosionVFX;
        public float explosionRadius = 5f;
        public float force = 700f;
        
        
        public void Explode(float size)
        {
            PushBodies(size);
            PlayVFX();
            Destroy(gameObject);
        }

        private void PushBodies(float size)
        {
            var nearbyObjects = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var hitObject in nearbyObjects)
            {
                Rigidbody rb = hitObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    force *= size;
                    Vector3 direction = hitObject.transform.position - transform.position;
                    var forceVector = direction.normalized * force;
                    forceVector.y = Mathf.Clamp(forceVector.y, 0, 10); 
                    rb.AddForce(forceVector, ForceMode.Impulse);
                }
            }
        }

        private void PlayVFX()
        {
            var go = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
            Destroy(go, 0.7f);
        }
    }
}
