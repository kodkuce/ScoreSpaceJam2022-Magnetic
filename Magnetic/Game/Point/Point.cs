using Godot;
using System;

public class Point : Area2D
{
    public override void _Ready()
    {
        base._Ready();
        Connect("body_entered", this, nameof(OnBodyEnter));
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
