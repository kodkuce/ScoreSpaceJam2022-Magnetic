using Godot;
using System;

public class GameEvents : Node
{
    public static Action GameOwer;
    public static Action StartGame;
    public static Action<string> PlaySFX;
    public static Action PlayerPrefsInited;
    public static Action ShowSetNamePopup;
    public static Action ShowStartScene;
    public static Action StartGetingLeaderboardTop10Data;
    public static Action ShowLeaderboard;
}
