using Godot;

public abstract partial class ForbitState
{
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
	public virtual void ActOnInput(double delta, Vector2 axisInputs, bool pressedJump, bool pressedRoll, bool pressedTongue)
	{
	}
	public virtual void StartState(){}
	public virtual void ExitState(){}
}