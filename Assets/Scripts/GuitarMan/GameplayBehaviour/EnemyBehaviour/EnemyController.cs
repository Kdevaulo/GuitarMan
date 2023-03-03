using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using GuitarMan.Models;

using UnityEngine;

using Object = UnityEngine.Object;

namespace GuitarMan.GameplayBehaviour.EnemyBehaviour
{
    public class EnemyController : IDisposable
    {
        private readonly EnemySystemView _enemySystemView;

        private readonly Transform _wallet;

        private readonly IRandomizer _randomizer;

        private readonly LevelEventsModel _levelEventsModel;

        private readonly Dictionary<int, Vector3> _positionsByIds = new Dictionary<int, Vector3>();

        private readonly Dictionary<EnemyView, SubscriptionContainer> _subscriptionsByViews =
            new Dictionary<EnemyView, SubscriptionContainer>();

        private Vector3[] _startPositions;

        private EnemyView _enemyPrefab;

        private Transform _enemyParent;

        public EnemyController(EnemySystemView enemySystemView, Transform wallet, IRandomizer randomizer,
            LevelEventsModel levelEventsModel)
        {
            _enemySystemView = enemySystemView;
            _wallet = wallet;
            _randomizer = randomizer;
            _levelEventsModel = levelEventsModel;
        }

        void IDisposable.Dispose()
        {
            foreach (var pair in _subscriptionsByViews)
            {
                UnsubscribeView(pair.Key, pair.Value);
            }

            _subscriptionsByViews.Clear();
        }

        public void Initialize()
        {
            _randomizer.Initialize(0, _enemySystemView.GetPositions().Length);
            _startPositions = _enemySystemView.GetPositions();
            _enemyPrefab = _enemySystemView.EnemyPrefab;
            _enemyParent = _enemySystemView.transform;
        }

        public void StartEnemySpawn()
        {
            SpawnEnemiesAsync().Forget();
        }

        private async UniTask SpawnEnemiesAsync()
        {
            while (true)
            {
                SpawnEnemy();
                await UniTask.Delay(6000);
            }
        }

        private void SpawnEnemy()
        {
            var startPosition = _startPositions[_randomizer.GetIndex()];

            var enemyView = Object.Instantiate(_enemyPrefab, startPosition, Quaternion.identity, _enemyParent);

            _positionsByIds.Add(enemyView.GetInstanceID(), startPosition);

            SubscribeView(enemyView);

            enemyView.transform.LookAt(_wallet);
            enemyView.StartMovingToTarget(_wallet.position, EnemySystemConstants.MovingToTargetSpeed);
        }

        private void HandleCameToTarget(EnemyView view)
        {
            view.SetMoneyCaughtSprite();
            view.DisableInteraction();
            view.StartMovingToShelter(GetShelterPosition(view), EnemySystemConstants.MovingToShelterSpeed);

            _levelEventsModel.InvokeEnemyCameToTarget();
        }

        private void HandleCameToShelter(EnemyView view)
        {
            DestroyEnemy(view);
        }

        private void HandlePlayerEntered(EnemyView view)
        {
            view.StartMovingToShelter(GetShelterPosition(view), EnemySystemConstants.MovingToShelterSpeed);
        }

        private void HandlePlayerExited(EnemyView view)
        {
            view.StartMovingToTarget(_wallet.position, EnemySystemConstants.MovingToTargetSpeed);
        }

        private Vector3 GetShelterPosition(EnemyView view)
        {
            return _positionsByIds[view.GetInstanceID()];
        }

        private void SubscribeView(EnemyView view)
        {
            var subscriptionContainer = new SubscriptionContainer
            {
                CameToShelterSubscription = () => HandleCameToShelter(view),
                CameToTargetSubscription = () => HandleCameToTarget(view),
                PlayerEnteredSubscription = () => HandlePlayerEntered(view),
                PlayerExitedSubscription = () => HandlePlayerExited(view)
            };

            _subscriptionsByViews.Add(view, subscriptionContainer);

            view.CameToShelter += subscriptionContainer.CameToShelterSubscription;
            view.CameToTarget += subscriptionContainer.CameToTargetSubscription;
            view.PlayerEntered += subscriptionContainer.PlayerEnteredSubscription;
            view.PlayerExited += subscriptionContainer.PlayerExitedSubscription;
        }

        private void DestroyEnemy(EnemyView view)
        {
            view.gameObject.SetActive(false);

            view.TryKillTween();

            UnsubscribeView(view, _subscriptionsByViews[view]);

            _subscriptionsByViews.Remove(view);
            Object.Destroy(view.gameObject);
        }

        private void UnsubscribeView(EnemyView view, SubscriptionContainer subscriptionContainer)
        {
            view.CameToShelter -= subscriptionContainer.CameToShelterSubscription;
            view.CameToTarget -= subscriptionContainer.CameToTargetSubscription;
            view.PlayerEntered -= subscriptionContainer.PlayerEnteredSubscription;
            view.PlayerExited -= subscriptionContainer.PlayerExitedSubscription;
        }
    }
}