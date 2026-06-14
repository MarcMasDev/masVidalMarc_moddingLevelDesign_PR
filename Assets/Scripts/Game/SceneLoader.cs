using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        public static async UniTask LoadScene(int sceneIndex)
        {
            await SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        }
    }
}