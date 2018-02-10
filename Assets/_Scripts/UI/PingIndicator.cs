using UnityEngine;

namespace Depth.UI
{
    public class PingIndicator : MonoBehaviour
    {
        public CooldownRadial Radial { get; private set; }

        public void Awake()
        {
            Radial = GetComponentInChildren<CooldownRadial>();
        }
    }
}