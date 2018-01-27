using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class TorpedoTargetable : MonoBehaviour
    {
        public void Start()
        {
            SubManager.Instance.Targetable.Add(this);
        }

        public void OnDestroy()
        {
            SubManager.Instance.Targetable.Remove(this);
        }
    }
}
