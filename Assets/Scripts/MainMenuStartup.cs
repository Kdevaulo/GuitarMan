using GuitarMan.MainMenuBehaviour;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(MainMenuStartup))]
    public class MainMenuStartup : MonoBehaviour
    {
        [SerializeField] private DisposableService _disposableService;

        [SerializeField] private MainMenuView _mainMenuView;

        private MenuButtonsController _menuButtonsController;

        private void Awake()
        {
            _menuButtonsController = new MenuButtonsController(_mainMenuView);

            _disposableService.Initialize(_menuButtonsController);
        }

        private void OnDestroy()
        {
            _disposableService.Dispose();
        }
    }
}