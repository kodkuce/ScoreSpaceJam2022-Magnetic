using Godot;
using System;

public class Score : Label
{
    public override void _Ready()
    {
        base._Ready();
        UpdateText();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.PointCollected += OnPointCollected;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.PointCollected -= OnPointCollected;
    }
    void OnPointCollected()
    {
        UpdateText();
    }

    void UpdateText()
    {
        Text = "score:" + Game.score.ToString("0000");
    }
}
