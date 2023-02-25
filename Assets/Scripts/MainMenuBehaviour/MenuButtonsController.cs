using System;
using System.Collections.Generic;

namespace GuitarMan.MainMenuBehaviour
{
    public class MenuButtonsController : IDisposable
    {
        private readonly MainMenuView _mainMenuView;

        private readonly List<PlayButtonBehaviourHandler> _buttonBehaviourHandlers =
            new List<PlayButtonBehaviourHandler>();

        public MenuButtonsController(MainMenuView mainMenuView)
        {
            _mainMenuView = mainMenuView;

            foreach (var view in mainMenuView.ButtonViews)
            {
                var behaviourHandler = new PlayButtonBehaviourHandler(view);
                behaviourHandler.SubscribeView();

                _buttonBehaviourHandlers.Add(behaviourHandler);
            }
        }

        void IDisposable.Dispose()
        {
            foreach (var handler in _buttonBehaviourHandlers)
            {
                handler.UnsubscribeView();
            }
        }
    }
}