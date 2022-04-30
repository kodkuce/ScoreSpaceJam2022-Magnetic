using Godot;
using System;
using System.Collections.Generic;

public class PlayerPrefs : Node
{
    static Godot.Collections.Dictionary<string,string> data = new Godot.Collections.Dictionary<string, string>();

    public override void _Ready()
    {
        Load();
        GameEvents.PlayerPrefsInited?.Invoke();
    }

    public static void Save()
    {
        File saveGame = new File();
        saveGame.Open("user://pprefs.save", File.ModeFlags.Write);
        saveGame.StorePascalString( JSON.Print(data) );
        saveGame.Close();
        GD.Print("pprefs saved");
    }

    public static void Load()
    {
        File saveGame = new File();
        saveGame.Open("user://pprefs.save", File.ModeFlags.WriteRead);
        string stringData = saveGame.GetPascalString();
        saveGame.Close();

        if(string.IsNullOrEmpty(stringData))
        {
            GD.Print("pprefs nothing to load fresh install");
            return;
        }
        try
        {
            JSONParseResult json = JSON.Parse(stringData);
            data = json.Result as Godot.Collections.Dictionary<string,string>;
            GD.Print("pprefs loaded");
        }
        catch(Exception e)
        {
            GD.PrintErr(e.Message);
        }
    }

    public static void SetString(string key, string value)
    {
        data[key] = value;
        Save();
    }

    public static string GetString(string key)
    {
        return data.ContainsKey(key) ? data[key] : null;
    }
}
