using Godot;
using System;

public abstract partial class ForbitState
{
	public ForbitState (ForbitUnit parent)
	{
		parentForbitUnit = parent;
	}
	protected ForbitUnit parentForbitUnit;
	public ForbitUnit ParentForbitUnit
	{
		get
		{
			return parentForbitUnit;
		}
		set
		{
			parentForbitUnit = value;
		}
	}
	public virtual void ActOnInput(double delta, ForbitUnit.ForbitFrameInput frameInput)
	{
	}
	public virtual void StartState(){}
	public virtual void ExitState(){}
	public virtual void AlignModelToBody()
    {
        parentForbitUnit.ForbitModel.GlobalPosition = parentForbitUnit.StandingBody.GlobalPosition;
        parentForbitUnit.ForbitModel.GlobalBasis = parentForbitUnit.PresentOrientation.Rotated(parentForbitUnit.PresentOrientation.Y, MathF.PI);
    }
}