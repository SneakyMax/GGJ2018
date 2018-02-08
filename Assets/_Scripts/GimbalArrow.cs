using UnityEngine;

namespace Depth
{
    public class GimbalArrow : MonoBehaviour
    {
        public void Update ()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
    }
}
