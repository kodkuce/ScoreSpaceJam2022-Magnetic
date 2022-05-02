using Godot;
using System;

public class Point : Area2D
{
    [Export] bool IsFirst;
    float selfDestoryTime = 12f;
    public override void _Ready()
    {
        base._Ready();
        Connect("body_entered", this, nameof(OnBodyEnter));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        selfDestoryTime -= delta;
        if(!IsFirst && selfDestoryTime<0)
        {
            QueueFree();
        }
    }

    public void OnBodyEnter(Node body)
    {
        if(body is Player)
        {
            GameEvents.PointCollected?.Invoke();
            QueueFree();
        }
    }
}
