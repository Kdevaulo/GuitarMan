using System;

namespace GuitarMan
{
    public class LevelEventsModel
    {
        public event Action EnemyCameToTarget = delegate { };

        public void InvokeEnemyCameToTarget()
        {
            EnemyCameToTarget.Invoke();
        }
    }
}