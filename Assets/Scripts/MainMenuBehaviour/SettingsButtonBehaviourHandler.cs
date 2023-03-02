using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    [CreateAssetMenu(fileName = nameof(SettingsButtonBehaviourHandler),
        menuName = nameof(MainMenuBehaviour) + "/" + nameof(SettingsButtonBehaviourHandler))]
    public class SettingsButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        protected override void HandleButtonClick()
        {
            Debug.Log($"{nameof(SettingsButtonBehaviourHandler)} {nameof(HandleButtonClick)} — ShowSettings");
        }
    }
}