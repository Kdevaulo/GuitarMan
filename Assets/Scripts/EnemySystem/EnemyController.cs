using System.Collections.Generic;
using System.Threading.Tasks;

using Cysharp.Threading.Tasks;

using UnityEngine;

using Object = UnityEngine.Object;

namespace GuitarMan.EnemySystem
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

            enemyView.transform.LookAt(_wallet);
            enemyView.StartMovingToTarget(_wallet.position, EnemySystemConstants.MovingToTargetDuration);
        }

        private void HandleCameToTarget(EnemyView view)
        {
            var targetPosition = _positionsByIds[view.GetInstanceID()];

            view.SetMoneyCaughtSprite();
            view.StartMovingToShelter(targetPosition, EnemySystemConstants.MovingToShelterDuration);
        }

        private void HandleCameToShelter(EnemyView view)
        {
            view.gameObject.SetActive(false);

            view.CameToShelter -= HandleCameToShelter;
            view.CameToTarget -= HandleCameToTarget;

            Object.Destroy(view.gameObject);
        }
    }
}