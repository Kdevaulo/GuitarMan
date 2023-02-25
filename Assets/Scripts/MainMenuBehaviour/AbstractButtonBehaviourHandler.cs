using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
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
            Debug.Log("Pointer enter");
        }

        private void HandlePointerExit()
        {
            Debug.Log("Pointer exit");
        }
    }
}