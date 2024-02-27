using Godot;

public partial class scenario : Node2D
{
    [Signal] public delegate void ScenarioEndEventHandler(bool FullRun);
    [Export] bool Endless;
    [Export] float EndTime;
    [Export] float[] Checkpoints;
    double timeTemp=0;
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("ap").Play("na");
    }
    public override void _Process(double delta)
    {
        timeTemp+=delta;
        if (EndTime<=timeTemp)
        {
            if (!Endless)
                EmitSignal("ScenarioEnd",true);
            else
            {
                timeTemp=0;
                GetNode<AudioStreamPlayer>("asp").Play();
            }
        }
    }
}
