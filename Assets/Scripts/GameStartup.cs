using GuitarMan.AwardSystem;
using GuitarMan.EnemyBehaviour;
using GuitarMan.Utils;
using GuitarMan.WalletBehaviour;

using UnityEngine;

namespace GuitarMan
{
    [AddComponentMenu(nameof(GuitarMan) + "/" + nameof(GameStartup))]
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private EnemySystemView _enemySystemView;

        [SerializeField] private WalletView _walletView;

        [SerializeField] private Transform _wallet;

        private EnemyController _enemyController;

        private AwardController _awardController;

        private WalletService _walletService;

        private LevelEventsModel _levelEventsModel;

        private void Awake()
        {
            _levelEventsModel = new LevelEventsModel();

            _enemyController =
                new EnemyController(_enemySystemView, _wallet, new WithoutLastRandomizer(), _levelEventsModel);

            _walletService = new WalletService(_walletView);

            _awardController = new AwardController(_levelEventsModel, _walletService);
        }

        private void Start()
        {
            _enemyController.Initialize();
            _enemyController.StartEnemySpawn();
        }
    }
}