using Godot;
using System;

public class GameOwer : Control
{
    [Export] public NodePath leaderboardButtonNP;
    [Export] public NodePath restartButtonNP;
    [Export] public NodePath highscoreLabelNP;


    Button restartButton;
    Button leaderboardButton;
    Label highscoreLabele;
    float time;
    public override void _Ready()
    {
        leaderboardButton = GetNode<Button>(leaderboardButtonNP);
        restartButton = GetNode<Button>(restartButtonNP);
        highscoreLabele = GetNode<Label>(highscoreLabelNP);

        // leaderboardButton.Disabled = true;
        // restartButton.Disabled = true;

        leaderboardButton.Connect("button_up", this, nameof(PressLeaderboard));
        restartButton.Connect("button_up", this, nameof(PressRestart));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        time += delta;

        float pulse = Mathf.Sin(time*4) *0.1f;
        leaderboardButton.RectScale = Vector2.One * (1f+pulse);
    }

    void PressRestart()
    {
        GameEvents.PressRestartButton?.Invoke();
        QueueFree();
    }

    void PressLeaderboard()
    {
        GameEvents.PressLeaderboardButton?.Invoke();
        QueueFree();
    }
}
