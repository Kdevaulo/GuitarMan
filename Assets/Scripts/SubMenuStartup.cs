using GuitarMan.AudioPlayerSystem;
using GuitarMan.FileLoadSystem;
using GuitarMan.GamePlayChooseBehaviour;
using GuitarMan.SoundProcessingSystem;
using GuitarMan.SoundsContainerBehaviour;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(SubMenuStartup))]
    public class SubMenuStartup : MonoBehaviour
    {
        [SerializeField] private DisposableService _disposableService;

        [SerializeField] private FileLoadView _fileLoadView;

        [SerializeField] private SoundContainerView _soundContainerView;

        [SerializeField] private GamePlayModeView _gamePlayModeView;

        [SerializeField] private AudioSource _audioSource;

        private FileLoadController _fileLoadController;

        private SoundProcessingController _soundProcessingController;

        private SoundLoadEventsModel _soundLoadEventsModel;

        private GamePlayModeController _gamePlayModeController;

        private SoundsContainerController _soundsContainerController;

        private AudioPlayer _menuAudioPlayer;

        private void Awake()
        {
            _soundLoadEventsModel = new SoundLoadEventsModel();

            _menuAudioPlayer = new AudioPlayer(_audioSource);

            _fileLoadController = new FileLoadController(_fileLoadView, _soundLoadEventsModel);
            _soundProcessingController = new SoundProcessingController(_soundLoadEventsModel);
            _gamePlayModeController = new GamePlayModeController(_gamePlayModeView);
            _soundsContainerController =
                new SoundsContainerController(_soundContainerView, _soundLoadEventsModel, _menuAudioPlayer);

            _disposableService.Initialize(_soundsContainerController, _soundProcessingController, _fileLoadController,
                _fileLoadView, _gamePlayModeView);
        }

        private void Start()
        {
            _fileLoadController.Initialize();
        }

        private void OnDestroy()
        {
            _disposableService.Dispose();
        }
    }
}