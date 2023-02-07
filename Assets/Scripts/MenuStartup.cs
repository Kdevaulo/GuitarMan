using GuitarMan.FileLoadSystem;
using GuitarMan.SoundProcessingSystem;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(MenuStartup))]
    public class MenuStartup : MonoBehaviour
    {
        [SerializeField] private FileLoadView _fileLoadView;

        [SerializeField] private SoundProcessingView _soundProcessingView;

        private FileLoadController _fileLoadController;

        private SoundProcessingController _soundProcessingController;

        private SoundLoadEventsModel _soundLoadEventsModel;

        private void Awake()
        {
            _soundLoadEventsModel = new SoundLoadEventsModel();

            _fileLoadController = new FileLoadController(_fileLoadView, _soundLoadEventsModel);
            _soundProcessingController = new SoundProcessingController(_soundProcessingView, _soundLoadEventsModel);
        }

        private void Start()
        {
            _fileLoadController.Initialize();
        }
    }
}