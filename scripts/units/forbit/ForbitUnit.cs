using Godot;
using System;

public partial class ForbitUnit : Node3D
{
	[Export] Camera3D gameCamera;
	public Vector3 cameraDirection
	{
		get
		{
			return gameCamera.Transform.Basis.Z;
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

    

    ForbitState presentState = new ForbitRunningState();
	Vector3 presentUpwardsDirection = Vector3.Up;
	[Export] CharacterBody3D standingBody;
	public CharacterBody3D StandingBody { get => standingBody;}
	[Export] CharacterBody3D rollingBody;
	public CharacterBody3D RollingBody { get => rollingBody;}
	[Export] float maxRunningSpeed;
	public float MaxRunningSpeed { get => maxRunningSpeed;}
	[Export] float runningAcceleration;
	public float RunningAcceleration { get => runningAcceleration;}
	[Export] float runningDeceleration;
	public float RunningDeceleration { get => runningDeceleration;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
