using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using GuitarMan.PlayerBehaviour;

using UnityEngine;

namespace GuitarMan.EnemyBehaviour
{
    [AddComponentMenu(nameof(EnemyBehaviour) + "/" + nameof(EnemyView)),
     RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EnemyView : MonoBehaviour, IDisposable
    {
        public event Action<EnemyView> PlayerFaced = delegate { };
        public event Action<EnemyView> CameToTarget = delegate { };
        public event Action<EnemyView> CameToShelter = delegate { };

        [SerializeField] private SpriteRenderer _enemyRenderer;

        [SerializeField] private Sprite _moneyCaughtSprite;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private void OnTriggerEnter(Collider collider)
        {
            collider.TryGetComponent<PlayerView>(out var view);

            PlayerFaced.Invoke(this);

            Debug.Log("PlayerFaced");
        }

        void IDisposable.Dispose()
        {
            DisposeToken();
            _cts = null;

            CameToTarget = null;
            CameToShelter = null;
        }

        public void SetMoneyCaughtSprite()
        {
            _enemyRenderer.sprite = _moneyCaughtSprite;
        }

        public void StartMovingToTarget(Vector3 targetPosition, float duration)
        {
            MoveToTargetAsync(targetPosition, duration, CameToTarget).Forget();
        }

        public void StartMovingToShelter(Vector3 targetPosition, float duration)
        {
            MoveToTargetAsync(targetPosition, duration, CameToShelter).Forget();
        }

        public void TryRefreshToken()
        {
            DisposeToken();
            _cts = new CancellationTokenSource();
        }

        private async UniTask MoveToTargetAsync(Vector3 targetPosition, float duration, Action<EnemyView> action)
        {
            await transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).WithCancellation(_cts.Token);

            action.Invoke(this);
        }

        private void DisposeToken()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }
    }
}