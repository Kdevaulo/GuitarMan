using GuitarMan.FileLoadSystem;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(MenuStartup))]
    public class MenuStartup : MonoBehaviour
    {
        [SerializeField] private FileLoadView _fileLoadView;

        private FileLoadController _fileLoadController;

        private void Awake()
        {
            _fileLoadController = new FileLoadController(_fileLoadView);
        }

        private void Start()
        {
            _fileLoadController.Initialize();
        }
    }
}