using Godot;
using System;

public class Splash : Control
{
    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.InputDragHeppend += OnInputDragHeppend;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.InputDragHeppend -= OnInputDragHeppend;
    }
    void OnInputDragHeppend(Vector2 dir)
    {
        QueueFree();
    }
}
