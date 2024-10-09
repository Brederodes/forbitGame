using Godot;
using System;

public partial class ForbitRollingState : ForbitState
{
    public ForbitRollingState(ForbitUnit parent) : base (parent)
    {
    }
    public override ForbitBody GetActiveBody()
    {
        return parentForbitUnit.RollingBody;
    }
}
