using UnityEngine;

namespace Scripts
{
    public class BombSpawner : MonoBehaviour
    {

        public GameObject bombPrefab;
        private AudioSource bombSound;
   
        public void SpawnBomb(Vector3 position, Vector3 size)
        {
            var newBomb = Instantiate(bombPrefab, position, Quaternion.identity);
            newBomb.transform.localScale = size;
            newBomb.GetComponent<BombBehaviour>().Explode(size.x);
            if (bombSound != null)
            {
                float scaleFactor = size.x;

                //bombSound.volume = Mathf.Clamp01(Mathf.Pow(size.x / 2f, 2f));
                bombSound.pitch = Mathf.Clamp(scaleFactor * 0.5f, 0.5f, 2f); // deeper or sharper based on size
                bombSound.Play();
            }

        }

        private void Awake()
        {
            bombSound = GetComponent<AudioSource>();
        }
    }
}
