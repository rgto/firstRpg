using Godot;
using System;

public partial class SmoothCameraArm : SpringArm3D
{

	[Export]
	public Node3D Target { get; set; }

	[Export]
	public float Decay { get; set; } = 20.0f;

	[Export]
    public float InterpolationWeight { get; set; } = 0.3f;

	public override void _PhysicsProcess(double delta)
	{
		if (Target == null)
		{
			// GD.PrintErr("SmoothCameraArm: Target não atribuído, interpolação de transformação ignorada.");
			return; // Sai do método se não houver Target
		}

        // 2. Cálculo do peso da interpolação
        //    - exp() em GDScript é Mathf.Exp() em C# (ou Math.Exp() para double, mas Mathf é Godot-specific)
        //    - delta é double, então a multiplicação vai se adaptar.
        //    - Certifique-se de que 1.0f seja um float, pois InterpolateWith espera um float para o peso.
        float interpolationWeight = 1.0f - Mathf.Exp(-Decay * (float)delta); // Cast delta para float para consistência com Decay

		GlobalTransform = GlobalTransform.InterpolateWith(
			Target.GlobalTransform,
			1.0f - interpolationWeight
		);
	}

}
