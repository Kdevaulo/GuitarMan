using System;

namespace GuitarMan.MainMenuBehaviour
{
    [Serializable]
    public abstract class AbstractButtonBehaviourHandler
    {
        private readonly MenuButtonView _buttonView;

        protected readonly ButtonType ButtonType;

        protected AbstractButtonBehaviourHandler(MenuButtonView buttonView)
        {
            _buttonView = buttonView;
            ButtonType = buttonView.ButtonType;
        }

        public void SubscribeView()
        {
            _buttonView.ButtonClicked += HandleButtonClick;
            _buttonView.ButtonPointerEnter += HandlePointerEnter;
            _buttonView.ButtonPointerExit += HandlePointerExit;
        }

        public void UnsubscribeView()
        {
            _buttonView.ButtonClicked -= HandleButtonClick;
            _buttonView.ButtonPointerEnter -= HandlePointerEnter;
            _buttonView.ButtonPointerExit -= HandlePointerExit;
        }

        protected abstract void HandleButtonClick();

        private void HandlePointerEnter()
        {
            _buttonView.SetAnimationState(true);
        }

        private void HandlePointerExit()
        {
            _buttonView.SetAnimationState(false);
        }
    }
}