using System;

using UnityEngine;

namespace GuitarMan.MainMenuBehaviour
{
    // note: createAssetMenu attribute need to be at inheritors
    [Serializable]
    public abstract class AbstractButtonBehaviourHandler : ScriptableObject
    {
        private MenuButtonView _buttonView;

        protected SceneManagerService SceneManagerService;

        protected ButtonType ButtonType;

        public void Initialize(SceneManagerService sceneManagerService, MenuButtonView buttonView)
        {
            _buttonView = buttonView;
            SceneManagerService = sceneManagerService;
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