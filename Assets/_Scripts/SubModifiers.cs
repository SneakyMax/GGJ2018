using UnityEngine;

namespace Depth
{
    public class SubModifiers : MonoBehaviour
    {
        [Header("Sub-Specific")]
        public ParticleGroup Bubbles;
        public ParticleGroup BackwardBubbles;

        [Header("Movement Limits")]
        [Range(0, 3)]
        public float MaxRaiseLowerSpeed = 1;

        [Range(0, 3)]
        public float MaxTurnRate = 1;

        [Range(0, 3)]
        public float MaxSpeed = 1;

        [Range(0, 3)]
        public float MaxStrafe = 1;

        [Range(0, 3)]
        public float MaxPitch = 1;

        [Header("Rates")]
        [Range(0, 3)]
        public float AccelerationRate = 1;
        [Range(0, 3)]
        public float BrakeRate = 1;

        [Range(0, 3)]
        public float TurnAcceleration = 1;

        [Range(0, 3)]
        public float FloatSinkRate = 1;

        [Range(0, 3)]
        public float PitchRate = 1;

        [Range(0, 3)]
        public float StrafeRate = 1;

        [Range(0, 3)]
        public float CeilingForce = 1;

        [Range(0, 3)]
        public float CeilingCorrect = 1;

        [Header("Abilities")]
        [Range(0, 3)]
        public float FireInterval = 1;

        [Range(0, 3)]
        public float MineInterval = 1;

        [Range(0, 3)]
        public float IdentifiedTime = 1;

        [Range(0, 3)]
        public float PingTime = 1;

        [Range(0, 3)]
        public float PingRange = 1;

        [Header("Ping")]
        [Range(0, 3)]
        public float GradientRange = 1;

        [Range(0, 3)]
        public float MinViewDistance = 1;

        [Range(0, 3)]
        public float MinDistTransisionDist = 1;

        public Color FogColor = new Color(1,1,1,1);

        public SubParameters GetParameters()
        {
            return new SubParameters(this);
        }
    }
}