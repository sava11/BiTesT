using Godot;
using Godot.Collections;
using System;

public partial class settings : Control
{
    [Export]
    Dictionary data=new Dictionary{
	{"Size",new Dictionary{
        {"X",800},
        {"Y",600}
        }
    },
	{"FullSize",false},
	{"Lang",0},
	{"Sound",new Dictionary{
        {"EFFECTS",100},
        {"MUSIC",100}
            }
        }
    };
    Vector2I[] wins={
        new Vector2I(800, 600), new Vector2I(854, 480), new Vector2I(960, 540), 
        new Vector2I(1024, 600), new Vector2I(1024, 768), new Vector2I(1152, 864), 
        new Vector2I(1200, 600), new Vector2I(1280, 720), new Vector2I(1280, 768), 
        new Vector2I(1280, 1024), new Vector2I(1408, 1152), new Vector2I(1440, 900),
        new Vector2I(1400, 1050), new Vector2I(1440, 1080), new Vector2I(1536, 960), 
        new Vector2I(1536, 1024), new Vector2I(1600, 900), new Vector2I(1600, 1024),
        new Vector2I(1600, 1200), new Vector2I(1680, 1050), new Vector2I(1920, 1080), 
        new Vector2I(1920, 1200), new Vector2I(2048, 1080), new Vector2I(2048, 1152), 
        new Vector2I(2048, 1536), new Vector2I(2560, 1080), new Vector2I(2560, 1440), 
        new Vector2I(2560, 1600), new Vector2I(2560, 2048), new Vector2I(3200, 1800), 
        new Vector2I(3200, 2048), new Vector2I(3200, 2400), new Vector2I(3440, 1440), 
        new Vector2I(3840, 2160), new Vector2I(3840, 2400), new Vector2I(4096, 2160), 
        new Vector2I(5120, 2880), new Vector2I(5120, 4096), new Vector2I(6400, 4096), 
        new Vector2I(6400, 4800), new Vector2I(7680, 4320), new Vector2I(7680, 4800), 
        new Vector2I(10240, 6480), new Vector2I(11520, 6480)
    };
    public override void _Ready()
    {
        if (!FileAccess.FileExists("settings.json"))
            sls.SaveData("settings.json",data);
        else
            data=sls.LoadData("settings.json");
        foreach( Vector2I w in wins){
            if (w<=DisplayServer.ScreenGetSize())
                GetNode<OptionButton>("menu/scr_sz/sz").AddItem(
                    w.X.ToString()+"x"+w.Y.ToString()
                    );
            }
        SetData(data);
    }
    
    void SetData(Dictionary d){
        DisplayServer.WindowSetSize(
            new Vector2I(
            (int)((Dictionary)d["Size"])["X"],
            (int)((Dictionary)d["Size"])["Y"])
        );
        DisplayServer.WindowSetPosition(
            DisplayServer.ScreenGetSize()/2-DisplayServer.WindowGetSize()/2
            );
        if ((bool)d["FullSize"])
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
        else
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        //LngItemSelected(d["Lang"]);
        GetNode<OptionButton>("menu/lng/lng").Selected=(int)d["Lang"];
        GetNode<HSlider>("menu/snd/c/music").Value=(float)((Dictionary)d["Sound"])["MUSIC"];
        GetNode<HSlider>("menu/snd/c/sound").Value=(float)((Dictionary)d["Sound"])["EFFECTS"];
        GetNode<CheckButton>("menu/fscr/cb").ButtonPressed=(bool)d["FullSize"];
        int id=0;
        for(int i=0;i<wins.Length;i++)
            if (wins[i]==DisplayServer.WindowGetSize())
                id=i;
        GetNode<OptionButton>("menu/scr_sz/sz").Select(id);
    }
    void MusicValueChanged(float value){
        float db=LinearToDecibel((float)GetNode<HSlider>("menu/snd/c/music").Value/100);
        ((Dictionary)data["Sound"])["MUSIC"]=value;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("music"), db
        );
    }
    void EffectsValueChanged(float value){
        float db=LinearToDecibel((float)GetNode<HSlider>("menu/snd/c/sound").Value/100);
        ((Dictionary)data["Sound"])["EFFECTS"]=value;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("effects"), db
        );
    }
    void EffectsDragEnded(float value){
        GetNode<AudioStreamPlayer>("snd").Play();
    }

    float LinearToDecibel(float linear){
        float db;
        if (linear != 0.0f)
            db = 20.0f * (float)Math.Log10(linear);
        else
            db = -144.0f; 
        return db;
    }
    void Apply(){
        SetData(data);
        sls.SaveData("settings.json",data);
        Back();
    }
    void SizeItemSelected(int id){
        ((Dictionary)data["Size"])["X"]=wins[id].X;
        ((Dictionary)data["Size"])["Y"]=wins[id].Y;
    } 
    void LanguageChanger(int id){
        data["Lang"]=GetNode<OptionButton>("menu/lng/lng").Selected;
        if((int)data["Lang"]==0){TranslationServer.SetLocale("ru");}
        if((int)data["Lang"]==1){TranslationServer.SetLocale("en");}
    }
    void FullScreanToggled(bool toggled){
        data["FullSize"]=toggled;
    }
    void Back(){
        SetData(data);
        Hide();
        GetParent().GetNode<AnimationPlayer>("anims").Play("settings",0.3,-1,true);
    }
    }
