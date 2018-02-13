using UnityEngine;

namespace Depth
{
    public class Spin : MonoBehaviour
    {
        public float SpinSpeed = 100;

        public void Update()
        {
            transform.rotation = Quaternion.AngleAxis(SpinSpeed * Time.deltaTime, Vector3.up) * transform.rotation;
        }
    }
}