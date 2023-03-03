using System;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using GuitarMan.GameplayBehaviour.PlayerBehaviour;

using UnityEngine;

namespace GuitarMan.GameplayBehaviour.EnemyBehaviour
{
    [AddComponentMenu(nameof(EnemyBehaviour) + "/" + nameof(EnemyView)),
     RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EnemyView : MonoBehaviour
    {
        public event Action PlayerEntered = delegate { };
        public event Action PlayerExited = delegate { };
        public event Action CameToTarget = delegate { };
        public event Action CameToShelter = delegate { };

        [SerializeField] private SpriteRenderer _enemyRenderer;

        [SerializeField] private Sprite _moneyCaughtSprite;

        private bool _canInteract = true;

        private Tween _currentTween;

        private void OnTriggerEnter(Collider target)
        {
            TryInvokeCollisionEvent(target, PlayerEntered);
        }

        private void OnTriggerExit(Collider target)
        {
            TryInvokeCollisionEvent(target, PlayerExited);
        }

        private void OnDestroy()
        {
            TryKillTween();
            _currentTween = null;

            PlayerEntered = null;
            PlayerExited = null;
            CameToTarget = null;
            CameToShelter = null;
        }

        public void TryKillTween()
        {
            if (_currentTween.IsActive())
            {
                _currentTween.Kill();
            }
        }

        public void SetMoneyCaughtSprite()
        {
            _enemyRenderer.sprite = _moneyCaughtSprite;
        }

        public void DisableInteraction()
        {
            _canInteract = false;
        }

        public void StartMovingToTarget(Vector3 targetPosition, float duration)
        {
            MoveToTargetAsync(targetPosition, duration, CameToTarget).Forget();
        }

        public void StartMovingToShelter(Vector3 targetPosition, float duration)
        {
            MoveToTargetAsync(targetPosition, duration, CameToShelter).Forget();
        }

        private async UniTask MoveToTargetAsync(Vector3 targetPosition, float duration, Action action)
        {
            TryKillTween();

            _currentTween = transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).SetSpeedBased();

            await _currentTween;

            if (!_currentTween.IsActive())
            {
                action.Invoke();
            }
        }

        private void TryInvokeCollisionEvent(Collider target, Action collisionEvent)
        {
            if (_canInteract && target.TryGetComponent<PlayerView>(out _))
            {
                collisionEvent.Invoke();
            }
        }
    }
}