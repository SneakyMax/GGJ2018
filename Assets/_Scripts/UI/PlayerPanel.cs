using UnityEngine;

namespace Depth.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        public DeadPanel DeadPanel { get; private set; }
        public KillIndicator KillIndicator { get; private set; }
        public MineIndicator MineIndicator { get; private set; }
        public PingIndicator PingIndicator { get; private set; }
        public WonPanel WonPanel { get; private set; }
        public TorpedoIndicator TorpedoIndicator { get; private set; }

        public void Awake()
        {
            DeadPanel = GetComponentInChildren<DeadPanel>();
            KillIndicator = GetComponentInChildren<KillIndicator>();
            MineIndicator = GetComponentInChildren<MineIndicator>();
            PingIndicator = GetComponentInChildren<PingIndicator>();
            WonPanel = GetComponentInChildren<WonPanel>();
            TorpedoIndicator = GetComponentInChildren<TorpedoIndicator>();
        }
    }
}