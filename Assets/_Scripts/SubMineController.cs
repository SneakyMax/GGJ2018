using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class SubMineController : MonoBehaviour
    {
        public GameObject MinePrefab;

        private float lastFireTime;
        private Sub sub;
        private Transform minePoint;

        public void Awake()
        {
            sub = GetComponent<Sub>();
            minePoint = GetComponentInChildren<MinePoint>().transform;
        }

        public void Update()
        {
            if (sub.IsDestroyed)
                return;

            if (!GameplayManager.Instance.AllowInput)
                return;

            if (sub.InputState.Buttons.Y == ButtonState.Pressed && Time.time - lastFireTime > sub.Parameters.FireInterval)
            {
                lastFireTime = Time.time;
                FireMine();
            }
        }

        private void FireMine()
        {
            var newMineObj = Instantiate(MinePrefab, minePoint.position, minePoint.rotation);
            var mine = newMineObj.GetComponent<Mine>();
            mine.Parent = sub;

            SoundManager.PlaySound("Deploy_or drop item");

            TaggableManager.Instance.TagForAllBut(sub.Taggable, sub.Player, 2);
        }
    }
}