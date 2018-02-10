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

        public SubParameters(SubModifiers modifiers)
        {
            var baseParams = BaseSubParameters.Instance;

            MaxRaiseLowerSpeed = baseParams.MaxRaiseLowerSpeed * modifiers.MaxRaiseLowerSpeed;
            MaxTurnRate = baseParams.MaxTurnRate * modifiers.MaxTurnRate;
            MaxSpeed = baseParams.MaxSpeed * modifiers.MaxSpeed;
            MaxStrafe = baseParams.MaxStrafe * modifiers.MaxStrafe;
            MaxPitch = baseParams.MaxPitch * modifiers.MaxPitch;
            AccelerationRate = baseParams.AccelerationRate * modifiers.AccelerationRate;
            BrakeRate = baseParams.BrakeRate * modifiers.BrakeRate;
            TurnAcceleration = baseParams.TurnAcceleration * modifiers.TurnAcceleration;
            FloatSinkRate = baseParams.FloatSinkRate * modifiers.FloatSinkRate;
            PitchRate = baseParams.PitchRate * modifiers.PitchRate;
            StrafeRate = baseParams.StrafeRate * modifiers.StrafeRate;
            CeilingForce = baseParams.CeilingForce * modifiers.CeilingForce;
            CeilingCorrect = baseParams.CeilingCorrect * modifiers.CeilingCorrect;
            FireInterval = baseParams.FireInterval * modifiers.FireInterval;
            MineInterval = baseParams.MineInterval * modifiers.MineInterval;
            IdentifiedTime = baseParams.IdentifiedTime * modifiers.IdentifiedTime;
            PingRange = baseParams.PingRange * modifiers.PingRange;
            PingTime = baseParams.PingTime * modifiers.PingTime;
        }
    }
}