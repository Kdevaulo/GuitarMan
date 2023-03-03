using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(SceneManagerService))]
    public class SceneManagerService : MonoBehaviour
    {
        [SerializeField] private int _startupSceneBuildIndex;

        private Scene _sceneForUnload;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private void Start()
        {
            SceneManager.LoadSceneAsync(_startupSceneBuildIndex, LoadSceneMode.Additive).WithCancellation(_cts.Token);
        }

        public void SwitchScene(int currentSceneIndex, int targetSceneBuildIndex, LoadSceneMode loadSceneMode)
        {
            _sceneForUnload = SceneManager.GetSceneByBuildIndex(currentSceneIndex);

            var loadingOperation = SceneManager.LoadSceneAsync(targetSceneBuildIndex, loadSceneMode)
                .WithCancellation(_cts.Token);

            HandleSceneLoadingAsync(loadingOperation).Forget();
        }

        private async UniTask HandleSceneLoadingAsync(UniTask loadingOperation)
        {
            await loadingOperation;

            SceneManager.UnloadSceneAsync(_sceneForUnload).WithCancellation(_cts.Token).Forget();
        }
    }
}