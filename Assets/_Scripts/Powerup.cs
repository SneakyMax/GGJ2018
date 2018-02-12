using UnityEngine;

namespace Depth.Assets._Scripts
{
    public class Powerup : MonoBehaviour
    {
        public float Time;

        public string Name;

        public void OnTriggerEnter(Collider other)
        {
            var sub = other.gameObject.GetComponentInParent<Sub>();
            if (sub != null)
            {
                sub.GotPowerup(this);
                Destroy(gameObject);
            }
        }
    }
}
