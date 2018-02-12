using UnityEngine;

namespace Depth
{
    public class SubParameters
    {
        public float MaxRaiseLowerSpeed { get; private set; }
        public float MaxTurnRate { get; private set; }
        public float MaxSpeed { get; private set; }
        public float MaxStrafe { get; private set; }
        public float MaxPitch { get; private set; }
        
        public float AccelerationRate { get; private set; }
        public float BrakeRate { get; private set; }
        public float TurnAcceleration { get; private set; }
        public float FloatSinkRate { get; private set; }
        public float PitchRate { get; private set; }
        public float StrafeRate { get; private set; }
        public float CeilingForce { get; private set; }
        public float CeilingCorrect { get; private set; }
        public float FireInterval { get; private set; }
        public float MineInterval { get; private set; }
        public float IdentifiedTime { get; private set; }
        public float PingRange { get; private set; }
        public float PingTime { get; private set; }
        public float GradientRange { get; private set; }
        public float MinViewDistance { get; private set; }
        public float MinDistTransisionDist { get; private set; }
        public Color FogColor { get; private set; }

        public SubParameters(SubModifiers modifiers, float movementModifier, float torpedoRate)
        {
            var baseParams = BaseSubParameters.Instance;

            MaxRaiseLowerSpeed = baseParams.MaxRaiseLowerSpeed * modifiers.MaxRaiseLowerSpeed * movementModifier;
            MaxTurnRate = baseParams.MaxTurnRate * modifiers.MaxTurnRate * movementModifier;
            MaxSpeed = baseParams.MaxSpeed * modifiers.MaxSpeed * movementModifier;
            MaxStrafe = baseParams.MaxStrafe * modifiers.MaxStrafe * movementModifier;
            MaxPitch = baseParams.MaxPitch * modifiers.MaxPitch * movementModifier;
            AccelerationRate = baseParams.AccelerationRate * modifiers.AccelerationRate * movementModifier;
            BrakeRate = baseParams.BrakeRate * modifiers.BrakeRate * movementModifier;
            TurnAcceleration = baseParams.TurnAcceleration * modifiers.TurnAcceleration * movementModifier;
            FloatSinkRate = baseParams.FloatSinkRate * modifiers.FloatSinkRate * movementModifier;
            PitchRate = baseParams.PitchRate * modifiers.PitchRate * movementModifier;
            StrafeRate = baseParams.StrafeRate * modifiers.StrafeRate * movementModifier;
            CeilingForce = baseParams.CeilingForce * modifiers.CeilingForce;
            CeilingCorrect = baseParams.CeilingCorrect * modifiers.CeilingCorrect;
            FireInterval = baseParams.FireInterval * modifiers.FireInterval * torpedoRate;
            MineInterval = baseParams.MineInterval * modifiers.MineInterval;
            IdentifiedTime = baseParams.IdentifiedTime * modifiers.IdentifiedTime;
            PingRange = baseParams.PingRange * modifiers.PingRange;
            PingTime = baseParams.PingTime * modifiers.PingTime;
            GradientRange = baseParams.GradientRange * modifiers.GradientRange;
            MinViewDistance = baseParams.MinViewDistance * modifiers.MinViewDistance;
            MinDistTransisionDist = baseParams.MinDistTransisionDist * modifiers.MinDistTransisionDist;
            FogColor = baseParams.FogColor * modifiers.FogColor;
        }
    }
}