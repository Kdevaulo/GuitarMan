using GuitarMan.Models;
using GuitarMan.SoundMenuBehaviour.AudioPlayerSystem;
using GuitarMan.SoundMenuBehaviour.ChooseGameplaySystem;
using GuitarMan.SoundMenuBehaviour.FileLoadSystem;
using GuitarMan.SoundMenuBehaviour.SoundProcessingSystem;
using GuitarMan.SoundMenuBehaviour.SoundsContainerSystem;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(SoundMenuStartup))]
    public class SoundMenuStartup : MonoBehaviour
    {
        [SerializeField] private DisposableService _disposableService;

        [SerializeField] private FileLoadView _fileLoadView;

        [SerializeField] private SoundContainerView _soundContainerView;

        [SerializeField] private GamePlayModeView _gamePlayModeView;

        [SerializeField] private AudioSource _audioSource;

        private FileLoadController _fileLoadController;

        private SoundProcessingController _soundProcessingController;

        private SoundContainerEventsModel _soundContainerEventsModel;

        private GamePlayModeController _gamePlayModeController;

        private SoundsContainerController _soundsContainerController;

        private AudioPlayer _menuAudioPlayer;

        private void Awake()
        {
            _soundContainerEventsModel = new SoundContainerEventsModel();

            _menuAudioPlayer = new AudioPlayer(_audioSource);

            _fileLoadController = new FileLoadController(_fileLoadView, _soundContainerEventsModel);
            _soundProcessingController = new SoundProcessingController(_soundContainerEventsModel);
            _gamePlayModeController = new GamePlayModeController(_gamePlayModeView);
            _soundsContainerController =
                new SoundsContainerController(_soundContainerView, _soundContainerEventsModel, _menuAudioPlayer);

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