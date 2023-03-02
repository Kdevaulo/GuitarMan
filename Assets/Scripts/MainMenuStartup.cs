using GuitarMan.MainMenuBehaviour;

using UnityEngine;
using UnityEngine.Assertions;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(MainMenuStartup))]
    public class MainMenuStartup : MonoBehaviour
    {
        [SerializeField] private DisposableService _disposableService;

        [SerializeField] private MainMenuView _mainMenuView;

        [SerializeField] private ButtonBehaviourDependencyStorage _buttonDependencyStorage;

        private MenuButtonsController _menuButtonsController;

        private SceneManagerService _sceneManagerService;

        private void Awake()
        {
            _sceneManagerService = FindObjectOfType<SceneManagerService>();

            Assert.IsNotNull(_sceneManagerService,
                $"{nameof(MainMenuStartup)} {nameof(Awake)} _sceneManagerService == null");

            _menuButtonsController =
                new MenuButtonsController(_sceneManagerService, _buttonDependencyStorage, _mainMenuView);

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