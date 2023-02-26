using GuitarMan.MainMenuBehaviour;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(MainMenuStartup))]
    public class MainMenuStartup : MonoBehaviour
    {
        [SerializeField] private DisposableService _disposableService;

        [SerializeField] private MainMenuView _mainMenuView;

        [SerializeField] private ButtonBehaviourDependencyStorage _buttonDependencyStorage;

        private MenuButtonsController _menuButtonsController;

        private void Awake()
        {
            _menuButtonsController = new MenuButtonsController(_buttonDependencyStorage, _mainMenuView);

            _disposableService.Initialize(_menuButtonsController);
        }

        private void Start()
        {
            _menuButtonsController.Initialize();
        }

        private void OnDestroy()
        {
            _disposableService.Dispose();
        }
    }
}