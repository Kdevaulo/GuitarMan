using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

namespace GuitarMan.MainMenuBehaviour
{
    public class MenuButtonsController : IDisposable
    {
        private readonly ButtonBehaviourDependencyStorage _buttonDependencyStorage;

        private readonly MainMenuView _mainMenuView;

        private readonly List<AbstractButtonBehaviourHandler> _buttonBehaviourHandlers =
            new List<AbstractButtonBehaviourHandler>();

        public MenuButtonsController(ButtonBehaviourDependencyStorage buttonDependencyStorage,
            MainMenuView mainMenuView)
        {
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
                var rawHandler = _buttonDependencyStorage.GetHandler(view.ButtonType);

                var behaviourHandler =
                    Activator.CreateInstance(rawHandler.GetClass(), args: view) as AbstractButtonBehaviourHandler;

                Assert.IsNotNull(behaviourHandler, $"{nameof(MenuButtonsController)} {nameof(Initialize)}");

                behaviourHandler.SubscribeView();

                _buttonBehaviourHandlers.Add(behaviourHandler);
            }
        }
    }
}