using Godot;
using System;

public partial class ThirdPersonCamera : Node3D
{
    [ExportGroup("Required Components")]
    [Export] public Node3D anchorNode;
    [Export] public Camera3D camera = null;
    [Export] public SpringArm3D cameraArm = null;
    [ExportGroup("Parameters")]
    [Export] public Vector2 cammeraSensitivity;

    [Export] public Vector3 cameraArmOriginOffset;
    [Export] public Vector3 cameraAngularOffset;

    [Export] public float cameraArmLength;
    private bool isActive = true;
    [Export]
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            if (value)
            {
                camera.Current = true;
            }
        }
    }
    protected bool hasMovedMouse = false;
    protected Vector2 mouseMovement = new Vector2(0f, 0f);

    public const float BOTTOM_VERTICAL_ANGLE_LIMIT = -75f;
    public const float TOP_VERTICAL_ANGLE_LIMIT = 60f;
    public override void _Ready()
    {
        //triggering the property because godot exports do not trigger it naturally
        Input.MouseMode= Input.MouseModeEnum.Captured;
        this.IsActive = this.IsActive;
        cameraArm.SpringLength = cameraArmLength;

        camera.RotateX(cameraAngularOffset.X);
        cameraArm.RotateX(-cameraAngularOffset.X);

        camera.RotateY(cameraAngularOffset.Y);
        cameraArm.RotateY(-cameraAngularOffset.Y);

        camera.RotateZ(-cameraAngularOffset.Z);
        cameraArm.RotateZ(cameraAngularOffset.Z);
    }
    public override void _Process(double delta)
    {
        if (!isActive) return;
        if (hasMovedMouse)
        {
            RotateCameraWithMouse(mouseMovement, cammeraSensitivity);
            hasMovedMouse = false;
        }
        FollowCameraTarget();
    }
    public void FollowCameraTarget()
    {
        GlobalTransform = new Transform3D(GlobalTransform.Basis, anchorNode.GlobalTransform.Translated(cameraArmOriginOffset).Origin);
    }
    public void RotateCameraWithMouse(Vector2 mouseMovement, Vector2 cammeraSensitivity)
    {
        //horizontal rotation
        GlobalRotate(Vector3.Up, Mathf.DegToRad(-cammeraSensitivity.X * mouseMovement.X));

        //vertical rotation
        float verticalRotationInput = Mathf.DegToRad(-cammeraSensitivity.Y * mouseMovement.Y);
        float verticalcameraAngularOffsetAngle = GlobalBasis.GetEuler(EulerOrder.Yxz).X;

        float finalVerticalRotationAngle = Mathf.Clamp(verticalcameraAngularOffsetAngle + verticalRotationInput, Mathf.DegToRad(BOTTOM_VERTICAL_ANGLE_LIMIT), Mathf.DegToRad(TOP_VERTICAL_ANGLE_LIMIT));
        float verticalRotationAngle = finalVerticalRotationAngle - verticalcameraAngularOffsetAngle;

        GlobalRotate(GlobalBasis.X, verticalRotationAngle);

        GlobalTransform = GlobalTransform.Orthonormalized();
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            hasMovedMouse = true;
            mouseMovement = eventMouseMotion.Relative;
        }
    }
}