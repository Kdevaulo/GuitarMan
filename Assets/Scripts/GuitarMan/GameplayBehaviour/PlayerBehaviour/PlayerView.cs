using UnityEngine;

namespace GuitarMan.GameplayBehaviour.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerBehaviour) + "/" + nameof(PlayerView)),
     RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class PlayerView : MonoBehaviour
    {
    }
}