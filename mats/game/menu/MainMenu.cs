using Godot;

public partial class MainMenu : Control
{
    public override void _Ready()
    {
        GetNode<Label>("menu/gm_txt").Text=
        (string)ProjectSettings.GetSetting("application/config/name");//.ToUpper();
        //GetNode<Label>("menu/gm_txt2").Text=GetNode<Label>("menu/gm_txt").Text;
    }
    public void OnSinglePlayer(){
        GetNode<AnimationPlayer>("anims").Play("play");
    }
    public void ToGame(){
        GetTree().ChangeSceneToFile("res://mats/game/MainGame.tscn");
        //GetNode<Control>("game").Show();
        
    }
    public void OnSetting(){
        GetNode<AnimationPlayer>("anims").Play("settings");
        
    }
    public void ToSettings(){
        GetNode<Control>("settings").Show();
    }
    public void OnExit(){
        GetNode<AnimationPlayer>("anims").Play("exit");
    }
    public void ToExit(){
         GetTree().Quit();
    }
}
