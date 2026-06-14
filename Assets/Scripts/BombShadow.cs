using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class BombShadow : MonoBehaviour
    {
        [SerializeField] private LayerMask _rayMask;
        [SerializeField] private LayerMask blockMask;

        public float ammoReloadPerSecond = 1;
        public float BombSizeIncreaseRate = 0.1f;
        public float cooldownTime = 1f;
        public float maxSize = 1f;
        public GameObject bombShadow;
        private int _ammoCount = 1;
        private float _cooldownTimer = 0f;
        private Vector3 ShadowLocation;
        public bool autoGrow = false;
        private AudioSource FallSound;
        private bool rightButton = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            FallSound = bombShadow.GetComponent<AudioSource>();
        }
        

        private void Start()
        {
            StartCoroutine(AmmoReload());
        }

        // Update is called once per frame
        private void Update()
        {
            
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                bombShadow.SetActive(false);
                rightButton = true;
            }

            _cooldownTimer -= Time.deltaTime;
            if (Mouse.current.leftButton.isPressed)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    rightButton = false;

                    if (_ammoCount <= 0)
                    {
                        Debug.Log("No Ammo!");
                        return;
                    }

                    if (_cooldownTimer > 0f)
                    {
                        Debug.Log("Wait to cooldown!");
                        return;
                    }

                    if (SpawnShadow())
                    {
                        _cooldownTimer = cooldownTime;
                        _ammoCount--;
                        if(FallSound != null)
                            FallSound.Play();
                    }
                }

                if (gameObject.activeSelf && !rightButton)
                {
                    if (bombShadow.transform.localScale.x > maxSize) return;

                    bombShadow.transform.localScale += Vector3.one * BombSizeIncreaseRate * Time.deltaTime;

                    if (FallSound != null && FallSound.isPlaying)
                    {
                        Debug.Log("Adjusting Fall Sound Pitch and Volume");
                        FallSound.pitch = Mathf.Clamp(bombShadow.transform.localScale.x * 1f, 0.8f, 2f);
                        FallSound.volume = Mathf.Clamp(BombSizeIncreaseRate * 10f, 0.2f, 1f);
                    }
                }
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame && bombShadow.activeSelf && !rightButton)
            {
                GetComponent<BombSpawner>().SpawnBomb(ShadowLocation, bombShadow.transform.localScale);
                bombShadow.SetActive(false);
            }
        }

        private bool SpawnShadow()
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            Ray bombRay = Camera.main.ScreenPointToRay(mousePos);
            if (!Physics.Raycast(bombRay, out RaycastHit hitInfo, float.PositiveInfinity, _rayMask.value))
                return false;

            if (((1 << hitInfo.collider.gameObject.layer) & blockMask) != 0) return false;


            ShadowLocation = hitInfo.point;
            bombShadow.SetActive(true);
            bombShadow.transform.position = ShadowLocation;
            bombShadow.transform.localScale = Vector3.one;

            return true;
        }

        private IEnumerator AmmoReload()
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / ammoReloadPerSecond);
                _ammoCount++;
            }
        }

        

    }
}