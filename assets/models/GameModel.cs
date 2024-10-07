using Godot;
using System;
using System.Collections.Generic;

public partial class GameModel : Node
{
	[Export] Godot.Collections.Dictionary<string,AnimationPlayer> animationPlayers;

	public void PlayNamedAnimation(string animatorName, string animationName, float animationSpeed = 1.0f)
	{
			animationPlayers[animatorName].Play(animationName, customSpeed: animationSpeed);
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
