using UnityEngine;

namespace GuitarMan.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerBehaviour) + "/" + nameof(PlayerView)),
     RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class PlayerView : MonoBehaviour
    {
    }
}