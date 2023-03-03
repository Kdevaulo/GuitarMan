using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuitarMan.MainMenuBehaviour
{
    [CreateAssetMenu(fileName = nameof(PlayButtonBehaviourHandler),
        menuName = nameof(MainMenuBehaviour) + "/" + nameof(PlayButtonBehaviourHandler))]
    public sealed class PlayButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        private const int CurrentSceneBuildIndex = 1;

        private const int TargetSceneBuildIndex = 2;

        protected override void HandleButtonClick()
        {
            SceneManagerService.SwitchScene(CurrentSceneBuildIndex, TargetSceneBuildIndex, LoadSceneMode.Additive);
        }
    }
}