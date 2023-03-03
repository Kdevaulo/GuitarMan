using System;

namespace GuitarMan.GameplayBehaviour.EnemyBehaviour
{
    public class SubscriptionContainer
    {
        public Action CameToShelterSubscription;

        public Action CameToTargetSubscription;

        public Action PlayerEnteredSubscription;

        public Action PlayerExitedSubscription;
    }
}