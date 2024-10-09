using Godot;
using System;

public abstract partial class GravityField : Area3D
{
    public override void _Ready()
    {
        base._Ready();
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }
    public virtual Vector3 GetForbitGravity(ForbitUnit forbitUnity)
    {
        return Vector3.Down;
    }
    public void OnBodyEntered(Node3D body)
    {
        if(!body.GetType().IsAssignableFrom(typeof(ForbitBody))) return;
        ForbitBody forbitBody = (ForbitBody) body;
        GD.Print("CAUGHT FORBIT!");
        forbitBody.parentForbit.AddGravityField(this);
        forbitBody.parentForbit.AttemptToSetActiveGravityField(this);
    }
    public void OnBodyExited(Node3D body)
    {
        if(!body.GetType().IsAssignableFrom(typeof(ForbitBody))) return;
        ForbitBody forbitBody = (ForbitBody) body;
        forbitBody.parentForbit.RemoveGravityField(this);
    }
}
