using UnityEngine;

namespace Depth
{
    public class BaseSubParameters : MonoBehaviour
    {
        public static BaseSubParameters Instance { get; private set; }

        [Header("Movement Limits")]
        public float MaxRaiseLowerSpeed = 7;
        public float MaxTurnRate = 1;
        public float MaxSpeed = 1000;
        public float MaxStrafe = 10;
        public float MaxPitch = 10;

        [Header("Rates")]
        public float AccelerationRate = 5000;
        public float BrakeRate = 1;
        public float TurnAcceleration = 150;
        public float FloatSinkRate = 5000;
        public float PitchRate = 70;
        public float StrafeRate = 1000;
        public float CeilingForce = 100;
        public float CeilingCorrect = 1;

        [Header("Abilities")]
        public float FireInterval = 1;
        public float MineInterval = 10;
        public float IdentifiedTime = 3;
        public float PingTime = 3.5f;
        public float PingRange = 200;
        
        [Header("Ping")]
        public float GradientRange = 25;
        public float MinViewDistance = 25;
        public float MinDistTransisionDist = 10;
        public Color FogColor = new Color(0, 0.06274f, 0.16078f, 1);

        public void Awake()
        {
            Instance = this;
        }
    }
}