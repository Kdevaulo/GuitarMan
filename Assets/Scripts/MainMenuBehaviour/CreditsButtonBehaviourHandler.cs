using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    public class CreditsButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        public CreditsButtonBehaviourHandler(MenuButtonView buttonView) : base(buttonView)
        {
        }

        protected override void HandleButtonClick()
        {
            Debug.Log($"{nameof(CreditsButtonBehaviourHandler)} {nameof(HandleButtonClick)} — ShowCredits");
        }
    }
}