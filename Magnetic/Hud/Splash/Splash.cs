using Godot;
using System;

public class Splash : Control
{
    float time;
    [Export] public NodePath gameNameNP;
    Label gameName;

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

    public override void _Ready()
    {
        base._Ready();
        gameName = GetNode<Label>(gameNameNP);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        time += delta;
        float pulse = Mathf.Sin(time*4) *0.1f;
        gameName.RectScale = Vector2.One * (1f+pulse);
    }
}
