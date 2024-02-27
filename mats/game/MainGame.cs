using System.Collections.Generic;
using Godot;
using Godot.Collections;
public partial class MainGame : Control
{
    Control ui=null;
    item selectedItem=null;
    [Export] static int LvlAcess=1;
    Dictionary data=new Dictionary {
        {"trained",false},
        {"ad",LvlAcess},
        {"lvls",new Dictionary{}}
    };
    string spath="saves";
    public override void _Ready()
    {
        DirAccess.MakeDirRecursiveAbsolute(spath);
        VBoxContainer vbc = GetNode<VBoxContainer>("ui/menu/scnt/vbc");
        if (!FileAccess.FileExists(spath+"/save.json")){
            foreach(HBoxContainer c1 in vbc.GetChildren())
            {
                foreach(item c2 in c1.GetChildren())
                {
                    Dictionary lvls = (Dictionary)data["lvls"];
                    lvls.Add((string)c2.GetPath(),new Dictionary{{"runned",false}});
                    
                }
            }
            sls.SaveData(spath+"/save.json",data);
        }
        else
        {
            data=sls.LoadData(spath+"/save.json");
            foreach(HBoxContainer c1 in vbc.GetChildren())
            {
                foreach(item c2 in c1.GetChildren())
                {
                    Dictionary lvl = (Dictionary)((Dictionary)data["lvls"])[(string)c2.GetPath()];
                    c2.Disabled=(bool)(lvl["runned"]);
                }
            }
        }
        ui=GetNode<Control>("ui");
        selectedItem=
        ui.GetNode<Container>("menu/scnt/vbc").GetChild(
            ui.GetNode<Container>("menu/scnt/vbc").GetChildCount()-1
        ).GetChild<item>(0);
        selectedItem.GrabFocus();
        
    }
    public override void _PhysicsProcess(double delta)
    {
        int c=ui.GetNode<VBoxContainer>("menu/scnt/vbc").GetChildCount();
        foreach(HBoxContainer cnt in ui.GetNode<VBoxContainer>("menu/scnt/vbc").GetChildren())
        {cnt.Visible=cnt.GetIndex()>=c-LvlAcess;}
    }
    public void runned(NodePath np, bool value)
    {
        ((Dictionary)((Dictionary)((Dictionary)data)["lvls"])[(string)np])["runned"]=value;
        GetNode<item>(np).Disabled=true;
        sls.SaveData(spath+"/save.json",data);
    }
    void BackToMenu()
    {GetTree().ChangeSceneToFile("res://mats/game/menu/MainMenu.tscn");}
}