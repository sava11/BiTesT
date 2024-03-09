using System;
using Godot;
using Godot.Collections;
public partial class MainGame : Control
{
    Control ui=null;
    item selectedItem=null;
    [Export] static int LvlAcess=1;
    Dictionary data=new Dictionary {
        {"trained",false},
        {"lvlac",LvlAcess},
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
            GetNode<Label>("ui/menu/lvl_run/runned").Text="0";
            sls.SaveData(spath+"/save.json",data);
        }
        else
        {
            int totalRun=0;
            data=sls.LoadData(spath+"/save.json");
            LvlAcess=(int)data["lvlac"];
            foreach(HBoxContainer _floor in vbc.GetChildren())
            {
                int lvlsRunnedOnFloor=0;
                foreach(item lvl in _floor.GetChildren())
                {
                    Dictionary lvlData = (Dictionary)((Dictionary)data["lvls"])[(string)lvl.GetPath()];
                    lvl.Runned=(bool)lvlData["runned"];
                    if (lvl.Runned) lvlsRunnedOnFloor++;
            
                    if (lvlsRunnedOnFloor==_floor.GetChildCount())
                    {
                        LvlAcess+=1;
                    }
                    totalRun+=lvlsRunnedOnFloor;
                }
            }
            GetNode<Label>("ui/menu/lvl_run/runned").Text=totalRun.ToString();
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
        GetNode<item>(np).Runned=value;
        int totalRun=0;
        VBoxContainer vbc = GetNode<VBoxContainer>("ui/menu/scnt/vbc");
        foreach(HBoxContainer _floor in vbc.GetChildren())
        {
            int lvlsRunnedOnFloor=0;
            foreach(item lvl in _floor.GetChildren())
            {
                if (lvl.Runned) lvlsRunnedOnFloor++;
            }
            if (lvlsRunnedOnFloor==_floor.GetChildCount())
            {
                LvlAcess+=1;
            }
            totalRun+=lvlsRunnedOnFloor;
        }
        GetNode<Label>("ui/menu/lvl_run/runned").Text=totalRun.ToString();
        sls.SaveData(spath+"/save.json",data);
    }
    void BackToMenu()
    {GetTree().ChangeSceneToFile("res://mats/game/menu/MainMenu.tscn");}
}