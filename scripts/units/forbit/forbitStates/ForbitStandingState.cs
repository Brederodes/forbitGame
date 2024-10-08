using Godot;
using System;

public partial class ForbitStandingState : ForbitState
{
    public ForbitStandingState(ForbitUnit parent) : base(parent)
    {
    }
    public override void ActOnInput(double delta, ForbitUnit.ForbitFrameInput frameInput)
    {
        base.ActOnInput(delta, frameInput);
        if(frameInput.pressedJump)
        {
            parentForbitUnit.ExecuteJump();
            parentForbitUnit.ChangeToState(parentForbitUnit.jumpingState);
            return;
        }
        if(frameInput.pressedRoll)
        {
            parentForbitUnit.ChangeToState(parentForbitUnit.rollingState);
            return;
        }
        if(frameInput.relativeAxisInput.LengthSquared() > 0.5f)
        {
            parentForbitUnit.ChangeToState(parentForbitUnit.runningState);
        }
    }
    public override void StartState()
    {
        base.StartState();
        parentForbitUnit.StandingBody.Velocity = Vector3.Zero;
        parentForbitUnit.ForbitModel.PlayNamedAnimation(new StringName("head"), "bobbing_head_slow", blend: 0.5f);
        parentForbitUnit.ForbitModel.PlayNamedAnimation(new StringName("shoe"), "standing", blend: 0.5f);
    }
}
