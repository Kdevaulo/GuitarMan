using UnityEngine;

namespace GuitarMan.GameplayBehaviour.PlayerBehaviour
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void Move(Vector3 direction)
        {
            if (direction == Vector3.zero)
            {
                PlayIdle();
            }
            else
            {
                PlayMove();
            }
        }

        private void PlayMove()
        {
            SetMovementTrigger(true);
        }

        private void PlayIdle()
        {
            SetMovementTrigger(false);
        }

        private void SetMovementTrigger(bool value)
        {
            _animator.SetBool(PlayerSystemConstants.MoveAnimationTrigger, value);
        }
    }
}