using Godot;
using System;

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
	Vector3 presentGravity = new Vector3(0f, -5f, 0f);
	public Vector3 PresentGravity
	{
		get
		{
			return presentGravity;
		}
		set
		{
			presentGravity = value;
		}
	}
	public float gravityValue = 10f;
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
	[Export] CharacterBody3D standingBody;
	public CharacterBody3D StandingBody { get => standingBody;}
	[Export] CharacterBody3D rollingBody;
	CharacterBody3D activeBody;
	public CharacterBody3D RollingBody { get => rollingBody;}
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
		ClearCycleInputs();
		runningState = new(this);
		standingState = new(this);
		jumpingState = new(this);
		rollingState = new(this);
		presentState = standingState;
		presentState.StartState();
		activeBody = standingBody;
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
		HandleInput();
		presentState.ActOnInput(delta, new ForbitFrameInput(axisInputThisCycle, pressedJumpThisCycle, pressedRollThisCycle, pressedTongueThisCycle));
		ClearCycleInputs();
		activeBody.MoveAndSlide();
		SufferGravityEffect(delta);
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
        standingBody.Velocity += presentGravity * gravityValue * (float)delta;
    }
}
