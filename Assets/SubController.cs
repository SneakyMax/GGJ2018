using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SubController : MonoBehaviour
{
    public RaiseState RaiseState;

    public TurnState TurnState;

    public AccelState AccelState;

    public StrafeState StrafeState;

    public float TurnFactor;

    public float RaiseFactor;

    public float StrafeFactor;

    /// <summary>Max pitch in degrees away from center</summary>
    public float MaxPitch;

    [Header("Movement Limits")]
    public float MaxRaiseLowerSpeed;

    public float MaxTurnRate;

    public float MaxSpeed;

    public float MaxStrafe;

    [Header("Rates")]
    public float AccelerationRate;

    public float BrakeRate;

    public float TurnAcceleration;

    public float FloatSinkRate;

    public float PitchRate;

    public float StrafeRate;

    private Rigidbody body;
    private Sub sub;
    private GameObject modelObject;

    public void Awake()
    {
        body = GetComponent<Rigidbody>();
        sub = GetComponent<Sub>();
        modelObject = GetComponentInChildren<SubBody>().gameObject;
    }

    public void FixedUpdate()
    {
        var forward = transform.forward;

        if (AccelState == AccelState.Accellerating)
        {
            var facingVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), forward) * body.velocity;
            if (facingVelocity.sqrMagnitude < MaxSpeed * MaxSpeed)
            {
                body.AddForce(forward * AccelerationRate * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
        else if (AccelState == AccelState.Reversing)
        {
            var facingVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), -forward) * body.velocity;
            if (facingVelocity.sqrMagnitude < MaxSpeed * MaxSpeed)
            {
                body.AddForce(-forward * AccelerationRate * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }

        if (TurnState == TurnState.TurnLeft)
        {
            if (body.angularVelocity.sqrMagnitude < MaxTurnRate * MaxTurnRate)
            {
                body.AddTorque(TurnFactor * -Vector3.up * TurnAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
            } 
        }
        else if (TurnState == TurnState.TurnRight)
        {
            if (body.angularVelocity.sqrMagnitude < MaxTurnRate * MaxTurnRate)
            {
                body.AddTorque(TurnFactor * Vector3.up * TurnAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }

        var pitch = modelObject.transform.localRotation.eulerAngles.x;
        if (pitch > 180)
            pitch -= 360;

        if (RaiseState == RaiseState.Raising)
        {
            if (Mathf.Abs(body.velocity.z) < MaxRaiseLowerSpeed)
            {
                body.AddForce(RaiseFactor * Vector3.up * FloatSinkRate * Time.fixedDeltaTime, ForceMode.Acceleration);
            }

            
            if (pitch > -MaxPitch)
            {
                modelObject.transform.localRotation = Quaternion.Euler(pitch - (PitchRate * Time.deltaTime * RaiseFactor), 0, 0);
            }
        }
        else if (RaiseState == RaiseState.Lowering)
        {
            if (Mathf.Abs(body.velocity.z) < MaxRaiseLowerSpeed)
            {
                body.AddForce(RaiseFactor * Vector3.down * FloatSinkRate * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            
            if (pitch < MaxPitch)
            {
                modelObject.transform.localRotation = Quaternion.Euler(pitch + (PitchRate * Time.deltaTime * RaiseFactor), 0, 0);
            }
        }
        else if (RaiseState == RaiseState.Centering)
        {
            if (Mathf.Abs(pitch) > 1)
            {
                modelObject.transform.localRotation = Quaternion.Euler(pitch + (PitchRate * Time.deltaTime * -Math.Sign(pitch)), 0, 0);
            }
        }

        if (StrafeState == StrafeState.Left)
        {
            var leftVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), -transform.right) * body.velocity;
            if (leftVelocity.sqrMagnitude < MaxStrafe * MaxStrafe)
            {
                body.AddForce(-transform.right * StrafeFactor * StrafeRate * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
        else if (StrafeState == StrafeState.Right)
        {
            var rightVelocity = Vector3.Dot(new Vector3(body.velocity.x, body.velocity.y, 0), -transform.right) * body.velocity;
            if (rightVelocity.sqrMagnitude < MaxStrafe * MaxStrafe)
            {
                body.AddForce(transform.right * StrafeFactor * StrafeRate * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
    }

    public void Update()
    {
        var accel = Input.GetAxis("Vertical 2 " + sub.Input);
        if (accel < 0)
        {
            AccelState = AccelState.Accellerating;
        }
        else if (accel > 0)
        {
            AccelState = AccelState.Reversing;
        }
        else
        {
            AccelState = AccelState.Stopping;
        }

        var horizontal = Input.GetAxis("Horizontal P" + (sub.Player + 1));
        if (horizontal < 0)
        {
            TurnState = TurnState.TurnLeft;
            TurnFactor = Mathf.Abs(horizontal);
        }
        else if (horizontal > 0)
        {
            TurnState = TurnState.TurnRight;
            TurnFactor = horizontal;
        }
        else
        {
            TurnState = TurnState.Centering;
        }

        var vertical = Input.GetAxis("Vertical P" + (sub.Player + 1));
        if (vertical > 0)
        {
            RaiseState = RaiseState.Raising;
            RaiseFactor = vertical;
        }
        else if (vertical < 0)
        {
            RaiseState = RaiseState.Lowering;
            RaiseFactor = Mathf.Abs(vertical);
        }
        else
        {
            RaiseState = RaiseState.Centering;
        }

        var strafe = Input.GetAxis("Horizontal 2 " + sub.Input);
        if (strafe < 0)
        {
            StrafeState = StrafeState.Left;
            StrafeFactor = Mathf.Abs(strafe);
        }
        else if (strafe > 0)
        {
            StrafeState = StrafeState.Right;
            StrafeFactor = strafe;
        }
        else
        {
            StrafeState = StrafeState.Centering;
        }
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