using Depth.Assets._Scripts.UI;
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
        public LockedOnWarning LockedOnWarning { get; private set; }
        public AbilityIndicator AbilityIndicator { get; private set; }
        public HiddenIndicator HiddenIndicator { get; private set; }
        public RectTransform Middle { get; private set; }

        public void Awake()
        {
            DeadPanel = GetComponentInChildren<DeadPanel>(true);
            KillIndicator = GetComponentInChildren<KillIndicator>(true);
            MineIndicator = GetComponentInChildren<MineIndicator>(true);
            PingIndicator = GetComponentInChildren<PingIndicator>(true);
            WonPanel = GetComponentInChildren<WonPanel>(true);
            TorpedoIndicator = GetComponentInChildren<TorpedoIndicator>(true);
            LockedOnWarning = GetComponentInChildren<LockedOnWarning>(true);
            AbilityIndicator = GetComponentInChildren<AbilityIndicator>(true);
            HiddenIndicator = GetComponentInChildren<HiddenIndicator>(true);
            Middle = (RectTransform)transform.Find("Middle");
        }
    }
}