using Godot;
using System;

public partial class ForbitRunningState : ForbitState
{
    public ForbitRunningState(ForbitUnit parent) : base (parent)
    {
    }
    CharacterBody3D runningBody;
    float maxRunningSpeedSquared;
    float runningAcceleration;
    float runningDecelerationFactor;
    float turningSpeed;
    public override void StartState()
    {
        base.StartState();
        runningBody = parentForbitUnit.StandingBody;
        maxRunningSpeedSquared = parentForbitUnit.MaxRunningSpeed * parentForbitUnit.MaxRunningSpeed;
        runningAcceleration = parentForbitUnit.RunningAcceleration;
        runningDecelerationFactor = parentForbitUnit.RunningDecelerationFactor;
        turningSpeed = parentForbitUnit.RunningTurningSpeed;
        parentForbitUnit.ForbitModel.PlayNamedAnimation(new StringName("head"), "bobbing_head_fast", blend: 0.5f);
        parentForbitUnit.ForbitModel.PlayNamedAnimation(new StringName("shoe"), "running", blend: 0.5f);
    }
    public override void ActOnInput(double delta, ForbitUnit.ForbitFrameInput frameInput)
    {
        if(frameInput.pressedJump)
        {
            parentForbitUnit.ExecuteJump();
            parentForbitUnit.ChangeToState(parentForbitUnit.jumpingState);
            return;
        }
        if(frameInput.pressedRoll)
        {
            //transition to rolling state
        }
        if(frameInput.pressedTongue)
        {
            //transition to tongue state
        }
        if(frameInput.relativeAxisInput.LengthSquared() < 0.3f)
        {
            parentForbitUnit.StandingBody.Velocity -= parentForbitUnit.StandingBody.Velocity.Normalized() * parentForbitUnit.RunningDecelerationFactor * (float)delta;
            if(parentForbitUnit.StandingBody.Velocity.LengthSquared() < 5f)
            {
                parentForbitUnit.ChangeToState(parentForbitUnit.standingState);
                return;
            }
        }
        ManageTurning(delta, frameInput.relativeAxisInput);
        if(!parentForbitUnit.StandingBody.IsOnFloor())
        {
            parentForbitUnit.ChangeToState(parentForbitUnit.jumpingState);
            return;
        }
        
        Basis presentCameraRotation = parentForbitUnit.CameraOrientation;
        //The minus sign in the Z is there because Godot's camera Z axis is inverted
        Vector3 ThreeDInput = new Vector3(-frameInput.relativeAxisInput.X, 0f, frameInput.relativeAxisInput.Y).Normalized();
        if(presentCameraRotation.Y.Y * parentForbitUnit.PresentGravity.Y > 0)
        {
            ThreeDInput = -ThreeDInput;
        }
        ThreeDInput = presentCameraRotation * ThreeDInput;
        Basis targetOrientation = new
        (
            ThreeDInput.Cross(-parentForbitUnit.PresentGravity),
            -parentForbitUnit.PresentGravity,
            ThreeDInput.Cross(-parentForbitUnit.PresentGravity).Cross(-parentForbitUnit.PresentGravity)
        );
        targetOrientation = targetOrientation.Orthonormalized();
        parentForbitUnit.StandingBody.UpDirection = targetOrientation.Y;
        //Adding the running velocity
        runningBody.Velocity += targetOrientation.Z * runningAcceleration * (float) delta;
        if(runningBody.Velocity.LengthSquared() > maxRunningSpeedSquared)
        {
            parentForbitUnit.StandingBody.Velocity -= parentForbitUnit.StandingBody.Velocity.Normalized() * parentForbitUnit.RunningDecelerationFactor * (float)delta;
        }
    }
    void ManageTurning(double delta, Vector2 relativeAxisInput)
    {
        if(relativeAxisInput.X == 0f && relativeAxisInput.Y == 0f) return;
        Basis presentCameraRotation = parentForbitUnit.CameraOrientation;
        Vector3 ThreeDInput = new Vector3(relativeAxisInput.X, 0f, -relativeAxisInput.Y).Normalized();
        ThreeDInput = presentCameraRotation * ThreeDInput;
        Basis targetOrientation = new
        (
            ThreeDInput.Cross(-parentForbitUnit.PresentGravity),
            -parentForbitUnit.PresentGravity,
            ThreeDInput.Cross(-parentForbitUnit.PresentGravity).Cross(-parentForbitUnit.PresentGravity)
        );
        targetOrientation = targetOrientation.Orthonormalized();
        parentForbitUnit.PresentOrientation = parentForbitUnit.PresentOrientation.Slerp(targetOrientation.Orthonormalized(), Math.Clamp(turningSpeed * (float)delta,0f,1f)).Orthonormalized();
        return;
    }
}