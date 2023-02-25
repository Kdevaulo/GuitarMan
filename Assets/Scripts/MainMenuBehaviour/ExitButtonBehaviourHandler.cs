using System;

namespace GuitarMan.MainMenuBehaviour
{
    public class ExitButtonBehaviourHandler : AbstractButtonBehaviourHandler
    {
        public ExitButtonBehaviourHandler(MenuButtonView buttonView) : base(buttonView)
        {
        }

        protected override void HandleButtonClick()
        {
            throw new NotImplementedException();
        }
    }
}