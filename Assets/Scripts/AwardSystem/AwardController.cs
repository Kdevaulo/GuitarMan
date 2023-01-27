using System;

using GuitarMan.EnemyBehaviour;
using GuitarMan.WalletBehaviour;

namespace GuitarMan.AwardSystem
{
    public class AwardController : IDisposable
    {
        private readonly LevelEventsModel _levelEventsModel;

        private readonly WalletService _walletService;

        private readonly EnemyController _enemyController;

        public AwardController(LevelEventsModel levelEventsModel, WalletService walletService)
        {
            _levelEventsModel = levelEventsModel;
            _walletService = walletService;
            _levelEventsModel.EnemyCameToTarget += HandleEnemyCameToTarget;
        }

        void IDisposable.Dispose()
        {
            _levelEventsModel.EnemyCameToTarget -= HandleEnemyCameToTarget;
        }

        private void HandleEnemyCameToTarget()
        {
            // todo: how much money to remove?
            _walletService.RemoveMoney(10);
        }
    }
}