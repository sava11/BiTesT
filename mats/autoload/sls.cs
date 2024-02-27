using Godot;
using Godot.Collections;
using System;
using System.Text.Json;
public partial class sls : Node
{
    
    static public void SaveData(string path,Dictionary data){
        FileAccess save_game = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        save_game.StoreLine(Json.Stringify(data));
        save_game.Close();
        }
    static public Dictionary LoadData(string path){
        if (FileAccess.FileExists(path)){
            FileAccess SaveGame = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            if (SaveGame.GetLength()!=0){
                Dictionary d=(Dictionary)Json.ParseString(SaveGame.GetLine());
                SaveGame.Close();
                return d;
            }
            else
            {
                GD.Print("save is clear");
                SaveGame.Close();
                return new Dictionary{};
            }
        }
        else
        {
            GD.Print("save isn't exists");
            return new Dictionary{};
        }
    }
    public override void _Ready(){
        DirAccess.MakeDirAbsolute("saves");
    }

}
