using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace GuitarMan.EnemyBehaviour
{
    public class EnemyController
    {
        private readonly EnemySystemView _enemySystemView;

        private readonly Transform _wallet;

        private readonly IRandomizer _randomizer;

        private Vector3[] _startPositions;

        private EnemyView _enemyPrefab;

        private Transform _enemyParent;

        private Dictionary<int, Vector3> _positionsByIds = new Dictionary<int, Vector3>();

        public EnemyController(EnemySystemView enemySystemView, Transform wallet, IRandomizer randomizer)
        {
            _enemySystemView = enemySystemView;
            _wallet = wallet;

            _randomizer = randomizer;
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

            enemyView.CameToShelter += HandleCameToShelter;
            enemyView.CameToTarget += HandleCameToTarget;
            enemyView.PlayerEntered += HandlePlayerEntered;
            enemyView.PlayerExited += HandlePlayerExited;

            enemyView.transform.LookAt(_wallet);
            enemyView.StartMovingToTarget(_wallet.position, EnemySystemConstants.MovingToTargetSpeed);
        }

        private void HandleCameToTarget(EnemyView view)
        {
            view.SetMoneyCaughtSprite();
            view.DisableInteraction();
            view.StartMovingToShelter(GetShelterPosition(view), EnemySystemConstants.MovingToShelterSpeed);
        }

        private void HandleCameToShelter(EnemyView view)
        {
            view.gameObject.SetActive(false);

            view.TryKillTween();

            view.CameToShelter -= HandleCameToShelter;
            view.CameToTarget -= HandleCameToTarget;
            view.PlayerEntered -= HandlePlayerEntered;
            view.PlayerExited -= HandlePlayerExited;

            Object.Destroy(view.gameObject);
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
    }
}