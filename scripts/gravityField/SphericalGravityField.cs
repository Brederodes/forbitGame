using Godot;
using System;

public partial class SphericalGravityField : GravityField
{
    [Export] Vector3 localCenter;
    [Export] float gravityValue;
    public override Vector3 GetForbitGravity(ForbitUnit forbitUnity)
    {
        return (localCenter + GlobalPosition - forbitUnity.ActiveBody.GlobalPosition).Normalized() * gravityValue;
    }
}
