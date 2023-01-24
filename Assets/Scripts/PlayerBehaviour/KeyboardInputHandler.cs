using UnityEngine;

namespace GuitarMan.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerBehaviour) + "/" + nameof(KeyboardInputHandler))]
    public class KeyboardInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerNavigation playerNavigation;

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            var direction = new Vector3(horizontal, 0, vertical);

            playerNavigation.Move(direction);
            playerNavigation.Rotate(direction);
        }
    }
}