using Godot;
using System;

public class Leaderboard : Control
{
    [Export] public NodePath leaderboardEntriesRoot;
    [Export] public NodePath leaderboardRoot;


    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
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
