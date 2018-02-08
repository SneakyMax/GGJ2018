using System.Collections.Generic;
using UnityEngine;

namespace Depth
{
    public class RenderController : MonoBehaviour
    {
        public static RenderController Instance { get; private set; }

        private readonly IList<RenderTexture> playerCams;

        public RenderController()
        {
            playerCams = new List<RenderTexture> { null, null, null, null };
        }

        public void Awake()
        {
            Instance = this;
        }

        public RenderTexture GetPlayerCam(int player)
        {
            return playerCams[player];
        }

        public void SetPlayerCam(int player, RenderTexture texture)
        {
            playerCams[player] = texture;
        }
    }
}
