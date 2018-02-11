 using System;
 using System.Collections.Generic;
 using UnityEngine;

namespace Depth
{
    public class CanBeLockedOnTo : MonoBehaviour
    {
        public event Action<ICanLockOn> OnLockedOn;
        public event Action<CanBeLockedOnTo> OnLockedOff;

        public int CurrentLocks
        {
            get { return lockers.Count; }
        }

        public bool IsDestroyed { get { return sub != null && sub.IsDestroyed; } }

        public event Action<Torpedo> OnHitByTorpedo;
        public event Action<Mine> OnHitByMine;

        private IList<ICanLockOn> lockers;

        private Sub sub;

        public void Awake()
        {
            lockers = new List<ICanLockOn>();
            sub = GetComponent<Sub>();
        }

        public void Start()
        {
            SubManager.Instance.Targetable.Add(this);
        }

        public void OnDestroy()
        {
            SubManager.Instance.Targetable.Remove(this);
        }

        public void HitByTorpedo(Torpedo torpedo)
        {
            if (OnHitByTorpedo != null)
                OnHitByTorpedo(torpedo);
        }

        public void HitByMine(Mine mine)
        {
            if (OnHitByMine != null)
                OnHitByMine(mine);
        }

        public void IsLockedOn(ICanLockOn locker)
        {
            if (lockers.Contains(locker) == false)
            {
                lockers.Add(locker);

                if (OnLockedOn != null)
                    OnLockedOn(locker);
            }
        }

        public void LockedOff(ICanLockOn locker)
        {
            if (lockers.Contains(locker))
            {
                lockers.Remove(locker);

                if (OnLockedOff != null)
                    OnLockedOff(this);
            }
        }
    }
}
