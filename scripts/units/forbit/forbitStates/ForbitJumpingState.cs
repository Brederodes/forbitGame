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
    }

}
