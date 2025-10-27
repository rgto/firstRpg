using Godot;
using System;

public partial class Player : CharacterBody3D
{
	//[Export] public SpringArm3D CameraSpringArm { get; set; }
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	// Stores the x/y direction to player is trying to look in.
	public Vector2 _look = Vector2.Zero;
	public float MouseSensitivity = 0.00075f;
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

    public override void _UnhandledInput(InputEvent @event)
	{
		FrameCameraRotation();
		// Verifica se o evento é um "action press" para "ui_cancel"
		if (@event.IsActionPressed("ui_cancel"))
		{
			// Se "ui_cancel" foi pressionado, torna o cursor do mouse visível
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		
		if (Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            if (@event is InputEventMouseMotion mouseMotion)
            {
				_look += -mouseMotion.Relative * MouseSensitivity;
				GD.Print(_look);
            }
        }
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_a", "ui_d", "ui_w", "ui_s");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void FrameCameraRotation()
	{
		
		var SpringArm3D = GetNode<SpringArm3D>("SpringArm3D");
        // 1. Gira o SpringArm3D em torno do eixo Y
        // O $SpringArm3D em GDScript vira o acesso à sua variável CameraSpringArm em C#.
        // rotate_y() vira RotateY().
        // _look.x vira _look.X.
        if (SpringArm3D != null) // Sempre bom verificar se a referência não é nula
        {
            SpringArm3D.RotateY(_look.X);
        }
        else
        {
            GD.PrintErr("Erro: CameraSpringArm não foi atribuído!");
        }

        // 2. Reseta a variável _look para zero
        // Vector2.ZERO vira Vector2.Zero (PascalCase).
        _look = Vector2.Zero;
    }
}
