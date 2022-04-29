using Godot;
using System;

public class Leaderboard : Control
{
    [Export] public NodePath leaderboardEntriesRoot;

    public override void _Ready()
    {
        FillWithData();
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
