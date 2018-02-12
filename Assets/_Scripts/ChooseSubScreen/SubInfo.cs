using DG.Tweening;
using UnityEngine;

namespace Depth.ChooseSubScreen
{
    public class SubInfo : MonoBehaviour
    {
        public string Name;

        public string Discription;

        public float SpinSpeed;

        public GameObject InGameSub;

        public Sprite SubImage;

        public string Ability;

        public void Update()
        {
            transform.rotation = Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, Vector3.up) * transform.rotation;
        }
    }
}