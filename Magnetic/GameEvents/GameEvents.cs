using Godot;
using System;

public class GameEvents : Node
{
    public static Action GameEnd;
    public static Action GameStart;
    public static Action<Vector2> InputDragHeppend;
    public static Action<string> PlaySFX;
    public static Action PlayerPrefsInited;
    public static Action ShowSetNamePopup;
    public static Action ShowStartScene;
    public static Action StartGetingLeaderboardTop10Data;
    public static Action ShowLeaderboard;
    public static Action PointCollected;
}
