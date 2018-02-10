using UnityEngine;

namespace Depth.UI
{
    public class MineIndicator : MonoBehaviour
    {
        public CooldownRadial Radial { get; private set; }

        public void Awake()
        {
            Radial = GetComponentInChildren<CooldownRadial>();
        }
    }
}