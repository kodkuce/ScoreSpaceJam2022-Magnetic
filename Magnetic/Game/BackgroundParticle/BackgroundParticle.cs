using Godot;
using System;

public class BackgroundParticle : CPUParticles2D
{
    public override void _Process(float delta)
    {
        base._Process(delta);
        Gravity = Game.oldMagnetForceDir;
    }
}
