using UnityEngine;

namespace Assets
{
    public class Helpers : MonoBehaviour
    {
        public static Helpers Instance { get; private set; }

        public GameObject PingPrefab;

        public RectTransform[] PlayerAreas;

        public void Awake()
        {
            Instance = this;
        }

        public void ShowPing(Sub player, Vector3 worldSpacePoint)
        {
            var direction = worldSpacePoint - player.transform.position;
            if (Vector3.Dot(direction, player.transform.forward) < 0)
                return; // Behind you

            var screenSpace = WorldPointToScreenSpace(worldSpacePoint, player.Cam.GetComponent<Camera>());
            if (screenSpace.x < 0 || screenSpace.y < 0 || screenSpace.x > 1 || screenSpace.y > 1)
                return; //outside screen
            
            var instance = Instantiate(PingPrefab, GetPlayerArea(player), false);
            var instanceTransform = (RectTransform) instance.transform;

            instanceTransform.anchoredPosition = CameraSpaceToMultiplyerSpace(screenSpace);
        }

        public RectTransform GetPlayerArea(Sub player)
        {
            return PlayerAreas[player.Player];
        }

        public Vector2 WorldPointToScreenSpace(Vector3 worldPoint, Camera cam)
        {
            var camSpace = cam.WorldToScreenPoint(worldPoint);
            var normalized = new Vector2(camSpace.x / cam.pixelWidth, camSpace.y / cam.pixelHeight);
            return normalized;
        }

        public Vector2 CameraSpaceToMultiplyerSpace(Vector2 cameraSpace)
        {
            return new Vector2(960f * cameraSpace.x, 540f * cameraSpace.y);
        }
    }
}
