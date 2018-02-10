using UnityEngine;

namespace Depth.UI
{
    public class TorpedoIndicator : MonoBehaviour
    {
        public CooldownRadial Radial { get; private set; }

        public void Awake()
        {
            Radial = GetComponentInChildren<CooldownRadial>();
        }
    }
}