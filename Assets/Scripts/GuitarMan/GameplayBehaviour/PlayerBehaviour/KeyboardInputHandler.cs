using UnityEngine;

namespace GuitarMan.GameplayBehaviour.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerBehaviour) + "/" + nameof(KeyboardInputHandler))]
    public class KeyboardInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerNavigation _playerNavigation;

        [SerializeField] private PlayerAnimation _playerAnimation;

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            var direction = new Vector3(horizontal, 0, vertical);

            _playerNavigation.Move(direction);
            _playerNavigation.Rotate(direction);

            _playerAnimation.Move(direction);
        }
    }
}