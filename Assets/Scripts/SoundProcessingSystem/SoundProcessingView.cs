using UnityEngine;

namespace GuitarMan.SoundProcessingSystem
{
    [AddComponentMenu(nameof(SoundProcessingSystem) + "/" + nameof(SoundProcessingView))]
    public class SoundProcessingView : MonoBehaviour
    {
        [field: SerializeField] public SoundView SoundViewPrefab { get; private set; }
        [field: SerializeField] public Transform LoadedFilesContainer { get; private set; }
    }
}