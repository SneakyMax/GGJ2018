using System.Collections;
using UnityEngine;

namespace Depth.ChooseSubScreen
{
    public class Flash : MonoBehaviour
    {
        public float Rate = 1;
        public MonoBehaviour[] Targets;

        public void Start()
        {
            StartCoroutine(Loop());
        }

        public IEnumerator Loop()
        {
            while (true)
            {
                foreach (var target in Targets)
                {
                    target.enabled = true;
                }
                yield return new WaitForSeconds(Rate);
                foreach (var target in Targets)
                {
                    target.enabled = false;
                }
                yield return new WaitForSeconds(Rate);
            }
        }
    }
}
