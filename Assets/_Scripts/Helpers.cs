using UnityEngine;

namespace Depth
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
            if (player == null)
                return;
            
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

        private RectTransform GetPlayerArea(Sub player)
        {
            return PlayerAreas[player.Player];
        }

        public static Vector2 WorldPointToScreenSpace(Vector3 worldPoint, Camera cam)
        {
            var camSpace = cam.WorldToScreenPoint(worldPoint);
            var normalized = new Vector2(camSpace.x / cam.pixelWidth, camSpace.y / cam.pixelHeight);
            return normalized;
        }

        public static Vector2 CameraSpaceToMultiplyerSpace(Vector2 cameraSpace)
        {
            return new Vector2(960f * cameraSpace.x, 540f * cameraSpace.y);
        }

        public static void FullscreenQuad()
        {
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0.1f);

            GL.TexCoord2(1, 0);
            GL.Vertex3(1, 0, 0.1f);

            GL.TexCoord2(1, 1);
            GL.Vertex3(1, 1, 0.1f);

            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 1, 0.1f);
            GL.End();
        }
    }
}
