using System;
using Unity.Mathematics;
using UnityEngine;

namespace Scripts
{
    public class RotateTo : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed;
        
        private void Update()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime);
        }
    }
}