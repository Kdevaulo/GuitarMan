using System.Linq;

using UnityEngine;

namespace GuitarMan.GameplayBehaviour.EnemyBehaviour
{
    [AddComponentMenu(nameof(EnemyBehaviour) + "/" + nameof(EnemySystemView))]
    public class EnemySystemView : MonoBehaviour
    {
        [field: SerializeField] public EnemyView EnemyPrefab { get; private set; }

        [SerializeField] private Transform[] _startPoints;

        public Vector3[] GetPositions()
        {
            return _startPoints.Select(x => x.position).ToArray();
        }
    }
}