using UnityEngine;

namespace GuitarMan.SoundMenuBehaviour.SoundsContainerSystem
{
    [AddComponentMenu(nameof(SoundsContainerSystem) + "/" + nameof(SoundContainerView))]
    public class SoundContainerView : MonoBehaviour
    {
        [field: SerializeField] public SoundView SoundViewPrefab { get; private set; }
        [field: SerializeField] public Transform LoadedFilesContainer { get; private set; }
    }
}