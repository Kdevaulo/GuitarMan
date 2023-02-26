using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    public class SettingsButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        public SettingsButtonBehaviourHandler(MenuButtonView buttonView) : base(buttonView)
        {
        }

        protected override void HandleButtonClick()
        {
            Debug.Log($"{nameof(SettingsButtonBehaviourHandler)} {nameof(HandleButtonClick)} — ShowSettings");
        }
    }
}