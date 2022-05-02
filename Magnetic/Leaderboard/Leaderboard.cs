using Godot;
using System;
using System.Linq;

public class Leaderboard : Control
{
    [Export] public NodePath leaderboardEntriesRoot;
    [Export] public NodePath leaderboardRoot;
    [Export] public NodePath closeButton;


    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.ShowLeaderboard += OnShowLeaderboard;
        GameEvents.GotLeaderboardData += OnGotLeaderboardData;
    }


    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.ShowLeaderboard -= OnShowLeaderboard;
        GameEvents.GotLeaderboardData -= OnGotLeaderboardData;
    }

    public override void _Ready()
    {
        base._Ready();
        GetNode<Button>(closeButton).Connect("button_up", this, nameof(CloseLeaderboard));
    }

    private void OnShowLeaderboard()
    {
        GD.PrintErr("zzz");
        GetNode<Control>(leaderboardRoot).Visible = true;
    }

    private void OnGotLeaderboardData(Godot.Collections.Dictionary data)
    {
        FillWithData(data);
    }

    private void CloseLeaderboard()
    {
        GameEvents.PressRestartButton();
    }

    public void FillWithData(Godot.Collections.Dictionary data)
    {
        PackedScene lEntry = GD.Load<PackedScene>("res://Leaderboard/LeaderboardEntry/LeaderboardEntry.tscn");
        Godot.Collections.Array rows = (Godot.Collections.Array)data["items"];


        for(int i=0;i<rows.Count;i++)
        {
            Godot.Collections.Dictionary row = (Godot.Collections.Dictionary)rows[i];
            string name;
            int score = int.Parse( row["score"].ToString());
            GD.PrintErr( int.Parse( row["score"].ToString()) );
            GD.PrintErr(row["score"].GetType());
            if(row["player"] is Godot.Collections.Dictionary pdata)
            {
                name = (string)pdata["name"];
                LeaderboardEntry leaderboardEntry = lEntry.Instance<LeaderboardEntry>();
                leaderboardEntry.SetEntryValues(i+1, name, score);
                GetNode(leaderboardEntriesRoot).AddChild(leaderboardEntry);
            }
        }
    }
}
