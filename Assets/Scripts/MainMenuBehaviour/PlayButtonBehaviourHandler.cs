using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    public sealed class PlayButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        public PlayButtonBehaviourHandler(MenuButtonView buttonView) : base(buttonView)
        {
        }

        protected override void HandleButtonClick()
        {
            Debug.Log($"{ButtonType} - Change Scene");
        }
    }
}