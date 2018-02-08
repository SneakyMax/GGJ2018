using UnityEngine;

namespace Depth
{
    public class ParticleGroup : MonoBehaviour
    {
        public bool EmittingAtStart;

        private ParticleSystem[] systems;

        public void Awake()
        {
            systems = GetComponentsInChildren<ParticleSystem>();
        }

        public void Start()
        {
            SetEmitting(EmittingAtStart);
            emitting = EmittingAtStart;
        }

        public void OnEnable()
        {
            SetEmitting(emitting);
        }

        private bool emitting;

        public bool Emitting
        {
            get { return emitting; }
            set
            {
                if (value == emitting) return;
                SetEmitting(value);
                emitting = value;
            }
        }

        private void SetEmitting(bool isEmitting)
        {
            foreach (var system in systems)
            {
                if (isEmitting)
                {
                    system.Play();
                }
                else
                {
                    system.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
    }
}
