using GuitarMan.EnemyBehaviour;
using GuitarMan.Utils;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(StartupLifeTimeScope))]
    public class StartupLifeTimeScope : MonoBehaviour
    {
        [SerializeField] private EnemySystemView _enemySystemView;

        [SerializeField] private Transform _wallet;

        private EnemyController _enemyController;

        private void Awake()
        {
            _enemyController = new EnemyController(_enemySystemView, _wallet, new WithoutLastRandomizer());
        }

        private void Start()
        {
            _enemyController.Initialize();
            _enemyController.StartEnemySpawn();
        }
    }
}