using UnityEngine;

namespace GuitarMan.SoundsContainerBehaviour
{
    [AddComponentMenu(nameof(SoundsContainerBehaviour) + "/" + nameof(SoundContainerView))]
    public class SoundContainerView : MonoBehaviour
    {
        [field: SerializeField] public SoundView SoundViewPrefab { get; private set; }
        [field: SerializeField] public Transform LoadedFilesContainer { get; private set; }
    }
}