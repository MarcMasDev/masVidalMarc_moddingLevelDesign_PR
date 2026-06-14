using System;
using Cysharp.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;

namespace Scripts
{
    public class GameManager : MMSingleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public async UniTask PlayGame()
        {
            await SceneLoader.LoadScene(2);
        }

        public async UniTask EndGame()
        {
            Time.timeScale = 1;
            await SceneLoader.LoadScene(1);
        }
    }
}
