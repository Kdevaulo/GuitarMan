using UnityEngine;

namespace GuitarMan.GameplayBehaviour.PlayerBehaviour
{
    [AddComponentMenu(nameof(PlayerBehaviour) + "/" + nameof(PlayerNavigation))]
    public class PlayerNavigation : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public void Move(Vector3 direction)
        {
            var movementOffset = direction * (PlayerSystemConstants.MovementSpeed * Time.deltaTime);

            _rigidbody.MovePosition(_rigidbody.position + movementOffset);
        }

        public void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                return;
            }

            _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction),
                PlayerSystemConstants.RotationSpeed * Time.deltaTime));
        }
    }
}