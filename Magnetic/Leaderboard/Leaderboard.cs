using Godot;
using System;

public class Leaderboard : Control
{
    [Export] public NodePath leaderboardEntriesRoot;
    [Export] public NodePath leaderboardRoot;
    [Export] public NodePath closeButton;


    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.ShowLeaderboard += OnShowLeaderboard;
    }


    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.ShowLeaderboard -= OnShowLeaderboard;
    }

    public override void _Ready()
    {
        base._Ready();
        GetNode<Button>(closeButton).Connect("button_up", this, nameof(CloseLeaderboard));
    }

    private void OnShowLeaderboard()
    {
        FillWithData();
        GetNode<Control>(leaderboardRoot).Visible = true;
    }

    private void CloseLeaderboard()
    {
        GetNode<Control>(leaderboardRoot).Visible = false;
    }

    public void FillWithData()
    {
        PackedScene lEntry = GD.Load<PackedScene>("res://Leaderboard/LeaderboardEntry/LeaderboardEntry.tscn");

        for(int i=0;i<10;i++)
        {
            LeaderboardEntry leaderboardEntry = lEntry.Instance<LeaderboardEntry>();
            leaderboardEntry.SetEntryValues(i+1, "EMPTY", 100 - i*10);
            GetNode(leaderboardEntriesRoot).AddChild(leaderboardEntry);
        }
    }
}
