using Godot;
using System;
using System.Collections.Generic;

public partial class GameModel : Node
{
	[Export] Godot.Collections.Array<AnimationPlayer> animationPlayers;
	public void PlayNamedAnimation(string animationName)
	{
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
