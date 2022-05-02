using Godot;
using System;

public class AudioPlayer : Node
{
    AudioStreamPlayer[] sfxPlayers;
    [Export] public Godot.Collections.Dictionary<string,AudioStream> sounds;

    public override void _Ready()
    {
        base._Ready();
        Godot.Collections.Array sfxP = GetNode("SFX").GetChildren();
        sfxPlayers = new AudioStreamPlayer[sfxP.Count];
        for(int i=0;i<sfxP.Count;i++)
        {
            sfxPlayers[i] = sfxP[i] as AudioStreamPlayer;
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.PlaySFX += OnPlaySFX;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.PlaySFX -= OnPlaySFX;
    }
    void OnPlaySFX(string audioClipName)
    {
        if(!sounds.ContainsKey(audioClipName))
        {
            GD.PrintErr($"missing sound {audioClipName}");
            return;
        }

        foreach(AudioStreamPlayer asp in sfxPlayers)
        {
            if(!asp.Playing)
            {
                asp.Stream = (sounds[audioClipName]);
                asp.Play();
                return;
            }
        }
    }

    // public override void _Input(InputEvent @event)
    // {
    //     base._Input(@event);
    //     if(@event is InputEventKey key && key.Scancode == (uint)KeyList.O)
    //     {
    //         GD.PrintErr("OO");
    //         OnPlaySFX("coin1");       
    //     }
    // }

    

}
