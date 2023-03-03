using System;

namespace GuitarMan.Models
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