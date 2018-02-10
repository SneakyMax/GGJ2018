using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Depth
{
    public class SpawnPoints : MonoBehaviour
    {
        public static SpawnPoints Instance { get; private set; }
        private IList<Transform> points;

        public void Awake()
        {
            Instance = this;
            points = transform.Cast<Transform>().ToList();
        }

        /// <summary>Gets the best spawn point for a player.</summary>>
        public Transform GetSpawnPoint(int player)
        {
            var subPositions = SubManager.Instance.Subs
                .Where(x => x.IsDestroyed == false && x.Player != player)
                .Select(x => x.transform.position).ToList();

            var bestSpawn = points[Random.Range(0, points.Count)];
            float bestDistance = 0;
            if (subPositions.Count > 0)
            {
                foreach (var spawn in points)
                {
                    var minDist = subPositions.Select(x => Vector3.Distance(x, spawn.position)).Min();
                    if (bestSpawn == null || minDist > bestDistance)
                    {
                        bestSpawn = spawn;
                        bestDistance = minDist;
                    }
                }
            }
            return bestSpawn;
        }
    }
}