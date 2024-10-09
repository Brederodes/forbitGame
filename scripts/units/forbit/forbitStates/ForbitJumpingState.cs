using Godot;
using System;

public partial class ForbitJumpingState : ForbitState
{
    public ForbitJumpingState(ForbitUnit parent) : base (parent)
    {
    }
    public override void ActOnInput(double delta, ForbitUnit.ForbitFrameInput frameInput)
    {
        base.ActOnInput(delta, frameInput);
        if(parentForbitUnit.StandingBody.IsOnFloor())
        {
            parentForbitUnit.ChangeToState(parentForbitUnit.standingState);
        }
        if((-parentForbitUnit.PresentGravity).Dot(parentForbitUnit.PresentOrientation.Y) > 0.9)
        {
            return;
        }

        Basis targetOrientation = new
        (
            parentForbitUnit.PresentOrientation.Z.Cross(-parentForbitUnit.PresentGravity),
            -parentForbitUnit.PresentGravity,
            parentForbitUnit.PresentOrientation.Z.Cross(-parentForbitUnit.PresentGravity).Cross(-parentForbitUnit.PresentGravity)
        );

        parentForbitUnit.PresentOrientation = parentForbitUnit.PresentOrientation.Slerp(targetOrientation.Orthonormalized(), Math.Clamp(10f * (float)delta,0f,1f)).Orthonormalized();
    }

}
