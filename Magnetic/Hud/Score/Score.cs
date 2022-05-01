using Godot;
using System;

public class Score : Label
{
    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.PointCollected += OnPointCollected;
        GameEvents.GameStart += OnStartGame;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.PointCollected -= OnPointCollected;
        GameEvents.GameStart -= OnStartGame;

    }
    void OnPointCollected()
    {
        UpdateText();
    }

    void OnStartGame()
    {
        Visible = true;
        UpdateText();
    }

    void UpdateText()
    {
        Text = "score:" + Game.score.ToString("0000") + "   hscore:" + Game.oldScore.ToString("0000") ;
    }
}
