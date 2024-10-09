using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

public partial class ForbitUnit : Node3D
{
	public class ForbitFrameInput
	{
		public ForbitFrameInput( Vector2 relativeAxisInput, bool pressedJump, bool pressedRoll, bool pressedTongue)
		{
			this.relativeAxisInput = relativeAxisInput;
			this.pressedJump = pressedJump;
			this.pressedRoll = pressedRoll;
			this.pressedTongue = pressedTongue;
		}
		public readonly Vector2 relativeAxisInput;
        public readonly bool pressedJump;
		public readonly bool pressedRoll;
		public readonly bool pressedTongue;
	}
	double lastCycleDelta = 0f;
	Vector2 axisInputThisCycle = Vector2.Zero;
	bool pressedJumpThisCycle = false;
	bool pressedRollThisCycle = false;
	bool pressedTongueThisCycle = false;
	[Export] Camera3D gameCamera;
	public Basis CameraOrientation
	{
		get
		{
			return gameCamera.GlobalBasis;
		}
	}
	[Export] GameModel forbitModel;
	public GameModel ForbitModel
	{
		get
		{
			return forbitModel;
		}
	}

    ForbitState presentState;
	public ForbitRunningState runningState;
	public ForbitStandingState standingState;
	public ForbitJumpingState jumpingState;
	public ForbitRollingState rollingState;
	List<GravityField> affectingGravityFields = new();
	GravityField activeGravityField = null;
	[Export] public Vector3 defaultGravity;
	public Vector3 PresentGravity
	{
		get
		{
			if(activeGravityField == null)
			{
				return defaultGravity;
			}
			return activeGravityField.GetForbitGravity(this);
		}
	}	
	Basis presentOrientation;
	public Basis PresentOrientation
	{
		get
		{
			return presentOrientation;
		}
		set
		{
			presentOrientation = value;
		}
	}
	[Export] ForbitBody standingBody;
	public ForbitBody StandingBody { get => standingBody;}
	[Export] ForbitBody rollingBody;
	public ForbitBody ActiveBody
	{
		get
		{
			return presentState.GetActiveBody();
		}
	}
	public ForbitBody RollingBody { get => rollingBody;}
	[Export] float maxRunningSpeed;
	public float MaxRunningSpeed { get => maxRunningSpeed;}
	[Export] float runningAcceleration;
	public float RunningAcceleration { get => runningAcceleration;}
	[Export] float runningDecelerationFactor;
	public float RunningDecelerationFactor { get => runningDecelerationFactor;}
	[Export] float runningTurningSpeed;
	public float RunningTurningSpeed { get => runningTurningSpeed;}

	[ExportGroup("Movement Attributes")]
	[Export] float jumpingSpeed;
	void ClearCycleInputs()
	{
		axisInputThisCycle = Vector2.Zero;
		pressedJumpThisCycle = false;
		pressedRollThisCycle = false;
		pressedTongueThisCycle = false;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		presentOrientation = StandingBody.Basis;
		standingBody.parentForbit = this;
		rollingBody.parentForbit = this;
		ClearCycleInputs();
		runningState = new(this);
		standingState = new(this);
		jumpingState = new(this);
		rollingState = new(this);
		presentState = standingState;
		presentState.StartState();
	}
	public void AddGravityField(GravityField newGF)
	{
		affectingGravityFields.Add(newGF);
	}
	/// <summary>
	/// Attempts to set the active gravity field for the Forbit and returns if it was successful or not
	/// </summary>
	/// <param name="newActiveGF">
	/// 	The new active gravity field
	/// </param>
	/// <returns>
	/// 	Returns if the attempt was successful
	/// </returns>
	public bool AttemptToSetActiveGravityField(GravityField newActiveGF)
	{
		if(!affectingGravityFields.Contains(newActiveGF)) return false;
		activeGravityField = newActiveGF;
		return true;
	}
	public void RemoveGravityField(GravityField removedGF)
	{
		affectingGravityFields.Remove(removedGF);
		if(removedGF == activeGravityField && affectingGravityFields.Count > 0)
		{
			activeGravityField = affectingGravityFields[0];
		}
		else
		{
			activeGravityField = null;
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		presentState.AlignModelToBody();
	}
    public override void _PhysicsProcess(double delta)
    {
		lastCycleDelta = delta;
        base._PhysicsProcess(delta);
		SufferGravityEffect(delta);
		HandleInput();
		presentState.ActOnInput(delta, new ForbitFrameInput(axisInputThisCycle, pressedJumpThisCycle, pressedRollThisCycle, pressedTongueThisCycle));
		ClearCycleInputs();
		ActiveBody.MoveAndSlide();
		GD.Print("IS STANDINGBODY ON FLOOR: " + standingBody.IsOnFloor());
    }
    void HandleInput()
    {
		Vector2 newInputVector = Vector2.Zero;
		newInputVector.Y = Input.GetAxis("ui_down", "ui_up");
		newInputVector.X = Input.GetAxis("ui_left", "ui_right");
		axisInputThisCycle = newInputVector;
		if(Input.IsActionPressed("jump"))
		{
			pressedJumpThisCycle = true;
		}
		if(Input.IsActionPressed("roll"))
		{
			pressedRollThisCycle = true;
		}
		if(Input.IsActionPressed("tongue"))
		{
			pressedTongueThisCycle = true;
		}
		GD.Print("INPUTS: "+ axisInputThisCycle + pressedJumpThisCycle +  pressedRollThisCycle + pressedTongueThisCycle);
    }
	public void ChangeToState(ForbitState state)
	{
		this.presentState= state;
		state.StartState();
		presentState.ActOnInput(lastCycleDelta, new ForbitFrameInput(axisInputThisCycle, pressedJumpThisCycle, pressedRollThisCycle, pressedTongueThisCycle));
	}
	public void ExecuteJump()
	{
		forbitModel.PlayNamedAnimation("head", "look_up", blend: .1f);
		forbitModel.PlayNamedAnimation("shoe", "jump", blend: .1f);
		StandingBody.Velocity += presentOrientation.Y * jumpingSpeed;
		standingBody.MoveAndSlide();
	}
	void SufferGravityEffect(double delta)
    {
		GD.Print("GRAVITY: "  + PresentGravity);
        standingBody.Velocity += PresentGravity * (float)delta;
		standingBody.UpDirection = -PresentGravity.Normalized();
		return;
    }
}
