using System;
using UnityEngine;

namespace Depth
{
    public class SubController : MonoBehaviour
    {
        public RaiseState RaiseState;
        public TurnState TurnState;
        public AccelState AccelState;
        public StrafeState StrafeState;

        public float TurnFactor;
        public float RaiseFactor;
        public float StrafeFactor;
        public float AccelFactor;
        public float PitchFactor;

        /// <summary>Max pitch in degrees away from center</summary>

        private SubParameters parameters;

        private SubModifiers modifiers;
        private Rigidbody body;
        private Sub sub;
        private GameObject modelObject;

        public void Awake()
        {
            modifiers = GetComponentInChildren<SubModifiers>();
            parameters = modifiers.GetParameters();
            body = GetComponent<Rigidbody>();
            sub = GetComponent<Sub>();
            modelObject = GetComponentInChildren<SubBody>().gameObject;
            SetBubbles(false, false);
        }

        private void SetBubbles(bool? forward, bool? backward)
        {
            if (modifiers.Bubbles != null && forward != null)
                modifiers.Bubbles.Emitting = forward.Value;

            if (modifiers.BackwardBubbles != null && backward != null)
                modifiers.BackwardBubbles.Emitting = backward.Value;
        }

        public void FixedUpdate()
        {
            if (sub.IsDestroyed)
            {
                body.velocity = new Vector3();
                body.isKinematic = true;
                SetBubbles(false, false);
                return;
            }

            if (body.isKinematic)
                body.isKinematic = false;

            var forward = transform.forward;

            var totalForce = new Vector3();

            if (AccelState == AccelState.Accellerating)
            {
                var facingVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), forward) * body.velocity;
                if (facingVelocity.magnitude < parameters.MaxSpeed)
                {
                    totalForce += AccelFactor * forward * parameters.AccelerationRate * Time.fixedDeltaTime;
                }

                SetBubbles(true, false);
            }
            else if (AccelState == AccelState.Reversing)
            {
                var facingVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), -forward) * body.velocity;
                if (facingVelocity.magnitude < parameters.MaxSpeed)
                {
                    totalForce += AccelFactor * -forward * parameters.AccelerationRate * Time.fixedDeltaTime;
                }

                SetBubbles(false, true);
            }
            if (AccelState == AccelState.Stopping)
            {
                SetBubbles(false, false);
            }

            if (TurnState == TurnState.TurnLeft)
            {
                if (body.angularVelocity.y > -parameters.MaxTurnRate)
                {
                    var backFactor = Mathf.Sign(body.angularVelocity.y) > 0 ? 4 : 1; 
                    body.AddTorque(backFactor * TurnFactor * -Vector3.up * parameters.TurnAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
                } 
            }
            else if (TurnState == TurnState.TurnRight)
            {
                if (body.angularVelocity.y < parameters.MaxTurnRate)
                {
                    var backFactor = Mathf.Sign(body.angularVelocity.y) < 0 ? 4 : 1;
                    body.AddTorque(backFactor * TurnFactor * Vector3.up * parameters.TurnAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
                }
            }
            else if ( body.angularVelocity.y > parameters.MaxTurnRate / 4.0f)
            {
                body.AddTorque(Vector3.up * -Mathf.Sign(body.angularVelocity.y) * parameters.TurnAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
            }

            var pitch = modelObject.transform.localRotation.eulerAngles.x;
            if (pitch > 180)
                pitch -= 360;

            if (RaiseState == RaiseState.Raising)
            {
                if (Mathf.Abs(body.velocity.y) < parameters.MaxRaiseLowerSpeed)
                {
                    totalForce += Vector3.up * parameters.FloatSinkRate * Time.fixedDeltaTime;
                }
            
                if (pitch > -parameters.MaxPitch)
                {
                    //modelObject.transform.localRotation = Quaternion.Euler(pitch - (PitchRate * Time.deltaTime * RaiseFactor), 0, 0);
                }
            }
            else if (RaiseState == RaiseState.Lowering)
            {
                if (Mathf.Abs(body.velocity.y) < parameters.MaxRaiseLowerSpeed)
                {
                    totalForce += Vector3.down * parameters.FloatSinkRate * Time.fixedDeltaTime;
                }
            
                if (pitch < parameters.MaxPitch)
                {
                    //modelObject.transform.localRotation = Quaternion.Euler(pitch + (PitchRate * Time.deltaTime * RaiseFactor), 0, 0);
                }
            }
            else if (RaiseState == RaiseState.Centering)
            {
                if (Mathf.Abs(pitch) > 1)
                {
                    // modelObject.transform.localRotation = Quaternion.Euler(pitch + (PitchRate * Time.deltaTime * -Math.Sign(pitch)), 0, 0);
                }
            }

            if (StrafeState == StrafeState.Left)
            {
                var leftVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), -transform.right) * body.velocity;
                if (leftVelocity.sqrMagnitude < parameters.MaxStrafe * parameters.MaxStrafe)
                {
                    totalForce += -transform.right * StrafeFactor * parameters.StrafeRate * Time.fixedDeltaTime;
                }
            }
            else if (StrafeState == StrafeState.Right)
            {
                var rightVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), -transform.right) * body.velocity;
                if (rightVelocity.sqrMagnitude < parameters.MaxStrafe * parameters.MaxStrafe)
                {
                    totalForce += transform.right * StrafeFactor * parameters.StrafeRate * Time.fixedDeltaTime;
                }
            }

            if (PitchFactor > 0.05 || PitchFactor < -0.05)
            {
                body.AddRelativeTorque(PitchFactor * parameters.PitchRate * Time.fixedDeltaTime, 0, 0, ForceMode.Acceleration);
            }

            if(totalForce.sqrMagnitude > 0)
            {
                body.AddForce(totalForce, ForceMode.Acceleration);
            }

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        }

        public void Update()
        {
            if (sub.IsDestroyed)
                return;

            if (!GameplayManager.Instance.AllowInput)
                return;

            var state = sub.InputState;

            var accel = state.ThumbSticks.Left.Y;

            if (accel > 0.1f)
            {
                AccelState = AccelState.Accellerating;
                AccelFactor = Mathf.Abs(accel);
            }
            else if (accel < -0.1f)
            {
                AccelState = AccelState.Reversing;
                AccelFactor = Mathf.Abs(accel);
            }
            else
            {
                AccelState = AccelState.Stopping;
            }

            var horizontal = state.ThumbSticks.Right.X;
            if (horizontal < -0.1f)
            {
                TurnState = TurnState.TurnLeft;
                TurnFactor = Mathf.Abs(horizontal);
            }
            else if (horizontal > 0.1f)
            {
                TurnState = TurnState.TurnRight;
                TurnFactor = horizontal;
            }
            else
            {
                TurnState = TurnState.Centering;
            }
        
            if (state.Triggers.Right > 0.5f)
            {
                RaiseState = RaiseState.Raising;
            }
            else if (state.Triggers.Left > 0.5f)
            {
                RaiseState = RaiseState.Lowering;
            }
            else
            {
                RaiseState = RaiseState.Centering;
            }

            PitchFactor = state.ThumbSticks.Right.Y;

            var strafe = state.ThumbSticks.Left.X;
            if (strafe < -0.1f)
            {
                StrafeState = StrafeState.Left;
                StrafeFactor = Mathf.Abs(strafe);
            }
            else if (strafe > 0.1f)
            {
                StrafeState = StrafeState.Right;
                StrafeFactor = strafe;
            }
            else
            {
                StrafeState = StrafeState.Centering;
            }
        }

        public void HitCeiling()
        {
            body.AddForce(Vector3.down * parameters.CeilingForce, ForceMode.VelocityChange);
            body.AddRelativeTorque(Vector3.right * parameters.CeilingCorrect, ForceMode.VelocityChange );
        }
    }

    [Serializable]
    public enum RaiseState
    {
        Raising,
        Centering,
        Lowering
    }
    [Serializable]
    public enum TurnState
    {
        TurnLeft,
        Centering,
        TurnRight
    }
    [Serializable]
    public enum AccelState
    {
        Accellerating,
        Stopping,
        Reversing
    }
    [Serializable]
    public enum StrafeState
    {
        Left,
        Centering,
        Right
    }
}