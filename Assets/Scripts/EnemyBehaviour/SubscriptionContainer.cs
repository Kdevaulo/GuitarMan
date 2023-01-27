using System;

namespace GuitarMan.EnemyBehaviour
{
    public class SubscriptionContainer
    {
        public Action CameToShelterSubscription;

        public Action CameToTargetSubscription;

        public Action PlayerEnteredSubscription;

        public Action PlayerExitedSubscription;
    }
}