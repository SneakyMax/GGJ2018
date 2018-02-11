using System.Linq;
using UnityEngine;

namespace Depth
{
    [RequireComponent(typeof(ParticleSystem))]
    public class OneShotParticles : MonoBehaviour
    {
        private ParticleSystem[] particles;

        public void Start()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
        }

        public void Update()
        {
            if (particles.All(x => x.IsAlive() == false))
            {
                Destroy(gameObject);
            }
        }
    }
}
