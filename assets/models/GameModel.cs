using Godot;
using System;
using System.Collections.Generic;

public partial class GameModel : Node3D
{
	[Export] Godot.Collections.Dictionary<string, NodePath> animationPlayers;

	public void PlayNamedAnimation(string animatorName, string animationName, float animationSpeed = 1.0f, double blend = 0f)
	{
		((AnimationPlayer)GetNode(animationPlayers[animatorName])).Play(animationName, customSpeed: animationSpeed, customBlend: blend);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
