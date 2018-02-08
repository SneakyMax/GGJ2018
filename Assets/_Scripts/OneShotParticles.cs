using UnityEngine;

namespace Depth
{
    [RequireComponent(typeof(ParticleSystem))]
    public class OneShotParticles : MonoBehaviour
    {
        private ParticleSystem particles;

        public void Start()
        {
            particles = GetComponent<ParticleSystem>();
        }

        public void Update()
        {
            if (particles.IsAlive() == false)
            {
                Destroy(gameObject);
            }
        }
    }
}
