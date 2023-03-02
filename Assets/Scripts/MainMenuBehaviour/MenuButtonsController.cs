using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

namespace GuitarMan.MainMenuBehaviour
{
    public class MenuButtonsController : IDisposable
    {
        private readonly SceneManagerService _sceneManagerService;

        private readonly ButtonBehaviourDependencyStorage _buttonDependencyStorage;

        private readonly MainMenuView _mainMenuView;

        private readonly List<AbstractButtonBehaviourHandler> _buttonBehaviourHandlers =
            new List<AbstractButtonBehaviourHandler>();

        public MenuButtonsController(SceneManagerService sceneManagerService,
            ButtonBehaviourDependencyStorage buttonDependencyStorage, MainMenuView mainMenuView)
        {
            _sceneManagerService = sceneManagerService;
            _buttonDependencyStorage = buttonDependencyStorage;
            _mainMenuView = mainMenuView;
        }

        void IDisposable.Dispose()
        {
            foreach (var handler in _buttonBehaviourHandlers)
            {
                handler.UnsubscribeView();
            }
        }

        public void Initialize()
        {
            foreach (var view in _mainMenuView.ButtonViews)
            {
                var behaviourHandler = _buttonDependencyStorage.GetHandler(view.ButtonType);

                Assert.IsNotNull(behaviourHandler, $"{nameof(MenuButtonsController)} {nameof(Initialize)}");

                behaviourHandler.Initialize(_sceneManagerService, view);
                behaviourHandler.SubscribeView();

                _buttonBehaviourHandlers.Add(behaviourHandler);
            }
        }
    }
}