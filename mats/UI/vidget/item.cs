using Godot;
public partial class item : Button
{
    [Export] PackedScene Scenario;
    [Export] bool ExtendToName=false;
    Label txt;
    StaticBody2D Arena=null;
    public override void _Ready()
    {
        txt=GetNode<Label>("pnl1/txt");
        if (ExtendToName)
        CustomMinimumSize=new Vector2(((Font)Get("theme_override_fonts/font")
        ).GetStringSize(txt.Text).X+16,CustomMinimumSize.Y);
    }
    public void OnSelect()
    {
        if(Scenario!=null)
        {
            Node2D wrld=GetTree().CurrentScene.GetNode<Node2D>("world");
            Arena=
            (StaticBody2D)ResourceLoader.Load<PackedScene>("res://mats/levels/arena.tscn").Instantiate();
            scenario scnObj=(scenario)Scenario.Instantiate();
            scnObj.ScenarioEnd+=end;
            Arena.AddChild(scnObj);
            Arena.GetNode<Player>("Player").MotionMode=CharacterBody2D.MotionModeEnum.Floating;
            wrld.AddChild(Arena);
            wrld.MoveChild(Arena,0);
            GetTree().CurrentScene.GetNode<Control>("ui/menu").Hide();
            GetTree().CurrentScene.GetNode<Control>("ui/gm").Show();
        }
    }
    private void end()
    {
        ((MainGame)GetTree().CurrentScene).runned(GetPath(),true);
        Arena.QueueFree();
        GetTree().CurrentScene.GetNode<Control>("ui/menu").Show();
        GetTree().CurrentScene.GetNode<Control>("ui/gm").Hide();

    }
}
