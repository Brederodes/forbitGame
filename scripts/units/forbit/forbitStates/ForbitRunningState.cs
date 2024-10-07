using Godot;
using System;

public partial class ForbitRunningState : ForbitState
{
    CharacterBody3D runningBody;
    float maxRunningSpeedSquared;
    float runningAcceleration;
    float runningDeceleration;
    public override void StartState()
    {
        base.StartState();
        runningBody = parentForbitUnit.StandingBody;
        maxRunningSpeedSquared = parentForbitUnit.MaxRunningSpeed * parentForbitUnit.MaxRunningSpeed;
        runningAcceleration = parentForbitUnit.RunningAcceleration;
        runningDeceleration = parentForbitUnit.RunningDeceleration;
        parentForbitUnit.ForbitModel.PlayNamedAnimation("head", "bobbing_head");
    }
    public override void ActOnInput(double delta, Vector2 axisInputs, bool pressedJump, bool pressedRoll, bool pressedTongue)
    {
        if(runningBody.Velocity.LengthSquared() > maxRunningSpeedSquared)
        {
            runningBody.Velocity = runningBody.Velocity - runningBody.Velocity.Normalized() * (float)delta * runningDeceleration;
            //to-do
        }
    }
}
