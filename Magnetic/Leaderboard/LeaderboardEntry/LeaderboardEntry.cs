using Godot;
using System;

public class LeaderboardEntry : Control
{
    public void SetEntryValues(int pos, string name, int score)
    {
        GetNode<Label>("pos").Text = $"{pos}.";
        GetNode<Label>("name").Text = $"{name}";
        GetNode<Label>("score").Text = $"{score}";
    }
}
