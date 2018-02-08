using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class SubTorpedoController : MonoBehaviour
    {
        public GameObject TorpedoPrefab;

        private float lastFireTime;

        private Sub sub;

        private Transform torpedoPoint;

        public void Awake()
        {
            sub = GetComponent<Sub>();
            torpedoPoint = GetComponentInChildren<TorpedoPoint>().transform;
        }

        public void Update()
        {
            if (sub.IsDestroyed)
                return;

            if (!GameplayManager.Instance.AllowInput)
                return;

            if (sub.InputState.Buttons.B == ButtonState.Pressed && Time.time - lastFireTime > sub.Parameters.FireInterval)
            {
                lastFireTime = Time.time;
                FireTorpedo();
            }
        }

        private void FireTorpedo()
        {
            var newTorpedoObj = Instantiate(TorpedoPrefab, torpedoPoint.position, torpedoPoint.rotation);
            var torpedo = newTorpedoObj.GetComponent<Torpedo>();
            torpedo.Parent = sub;

            SoundManager.PlaySound("TorpedoFire_2,5sec");

            TaggableManager.Instance.TagForAllBut(sub.Taggable, sub.Player, 2);
        }
    }
}