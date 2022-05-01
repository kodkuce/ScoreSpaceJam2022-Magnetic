using Godot;
using System;

public class MagnetForceGuageHud : TextureRect
{
    [Export] NodePath arrow1NP;
    [Export] NodePath arrow2NP;
    TextureRect arrow1;
    TextureRect arrow2;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        arrow1 = GetNode<TextureRect>(arrow1NP);
        arrow2 = GetNode<TextureRect>(arrow2NP);
    }


    public override void _Process(float delta)
    {
        // float a1angle = Mathf.Rad2Deg(Game.oldMagnetForceDir.Angle());
        // float a2angle = Mathf.Rad2Deg(Game.newMagnetForceDir.Angle());
        float a1angle = Game.oldMagnetForceDir.Angle();
        float a2angle = Game.newMagnetForceDir.Angle();
        arrow1.SetRotation(a1angle);
        arrow2.SetRotation(a2angle);
    }
}
