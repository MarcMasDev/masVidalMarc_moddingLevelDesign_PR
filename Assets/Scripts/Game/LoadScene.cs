using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private int _sceneIndex;
        [SerializeField] private bool _onStart;

        private void Start()
        {
            Load();
        }

        private void Load()
        {
            SceneLoader.LoadScene(_sceneIndex).Forget();
        }
    }
}