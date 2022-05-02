using Godot;
using System;

public class Game : Node2D
{
    [Export] public NodePath scorePointsRootNP;
    Node2D scorePointsRoot;
    [Export] public NodePath meteorsRootNP;
    Node2D meteorsRoot;
    PackedScene[] meteors;
    int whatMeteor;
    [Export] public NodePath dragLineNP;
    Line2D dragLine;
    PackedScene scorePointResource;
    public static Vector2 oldMagnetForceDir = Vector2.One;
    public static Vector2 newMagnetForceDir = Vector2.One;
    float speed = 1;
    float switchDirSpeed = 0.2f;
    bool canStartGame;
    bool gameStarted;
    bool gameEnded;
    float passedTime = 0f;
    bool restarting;


    public static int score;
    public static int oldScore;
    public static bool newHighScore; 

    float spawnCollectableTimer = 6;
    float SpawnMeteorsTimer = 1;

    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.GameEnd += OnGameEnd;
        GameEvents.PointCollected += OnPointCollected;
        GameEvents.PressRestartButton += RestartGame;
        GameEvents.ShowStartScene += OnShowStartScene;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.GameEnd -= OnGameEnd;
        GameEvents.PointCollected -= OnPointCollected;
        GameEvents.PressRestartButton -= RestartGame;
        GameEvents.ShowStartScene -= OnShowStartScene;
    }

    void OnGameEnd()
    {
        gameEnded = true;
        if(oldScore<score)
        {

        }
    }
    void OnPointCollected()
    {
        score++;
        if(oldScore<score)
        {
            newHighScore = true;
            oldScore = score;
            PlayerPrefs.SetString("score",score.ToString());
        }
    }

    void OnShowStartScene()
    {
        GD.PrintErr("CAN START GAME");
        canStartGame = true;
    }

    public void StartGame()
    {
        gameStarted = true;
        GameEvents.GameStart?.Invoke();
        oldScore = PlayerPrefs.GetString("score") == null ? 0 : int.Parse(PlayerPrefs.GetString("score"));
        newHighScore = false;
    }

    public override void _Ready()
    {
        dragLine = GetNode<Line2D>(dragLineNP);
        
        scorePointsRoot = GetNode<Node2D>(scorePointsRootNP);
        scorePointResource = GD.Load<PackedScene>("res://Game/Point/Point.tscn");

        meteorsRoot = GetNode<Node2D>(meteorsRootNP);
        meteors = new PackedScene[3];
        for(int i=0;i<3;i++)
        {
            meteors[i] = GD.Load<PackedScene>($"res://Game/Meteor/Meteor{i+1}.tscn");
        }

        newMagnetForceDir = GetNewRandomDir();
        score = 0;

        if(!string.IsNullOrEmpty(LootLockerHandler.playerName))
        {
            canStartGame = true;
        }
    }

    Vector2 GetNewRandomDir()
    {
        float rangle = (float)GD.RandRange(90.0, 270.0);
        return oldMagnetForceDir.Rotated(Mathf.Deg2Rad(rangle)).Normalized()* (Mathf.Clamp( GD.Randf()*100*speed,20,1000));
    }

    public override void _Process(float delta)
    {
        passedTime += delta;
        speed = Mathf.Clamp( speed + delta*0.1f, 1, 5);


        base._Process(delta);
        oldMagnetForceDir = oldMagnetForceDir.LinearInterpolate(newMagnetForceDir,delta*switchDirSpeed);
        if(oldMagnetForceDir.DistanceSquaredTo(newMagnetForceDir) < 5)
        {
            // GD.PrintErr("SwitchDir");
            newMagnetForceDir = GetNewRandomDir();
        }

        SpawnCollectablePoints(delta);
        SpawnMeteors(delta);
    }

    void SpawnCollectablePoints(float delta)
    {
        if(score>0 && !gameEnded)//player collected first test point
        {
            spawnCollectableTimer -= delta;
            if(spawnCollectableTimer < 0)
            {
                spawnCollectableTimer = 3 + GD.Randf()*6f;
                Vector2 pos = new Vector2( Mathf.Clamp(GD.Randf()*310, 30, 310), Mathf.Clamp(GD.Randf()*610f,30,610));
                Point scorP = scorePointResource.Instance<Point>();
                scorP.Position = pos;
                scorePointsRoot.AddChild(scorP);
            }
        }
    }

    void SpawnMeteors(float delta)
    {
        if(score>0 && !gameEnded && meteorsRoot.GetChildCount() < 30)//player collected first test point
        {
            SpawnMeteorsTimer -= delta;
            if(SpawnMeteorsTimer < 0)
            {
                SpawnMeteorsTimer = 3 + GD.Randf()*6f;
                // whatMeteor = Mathf.RoundToInt((float)GD.RandRange(0,2));
                whatMeteor++;
                whatMeteor = whatMeteor > 2 ? 0 : whatMeteor;
                PackedScene metP = meteors[whatMeteor];
                Meteor met = metP.Instance<Meteor>();
                met.Position = Vector2.One * 1100;
                meteorsRoot.AddChild(met);
            }
        }
    }


    Vector2 clickStartMousePos;
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        MouseDragInput(@event);
        CheckForRestartBTN(@event);
    }
    Godot.Collections.Array<Vector2> arr = new Godot.Collections.Array<Vector2>();
    Vector2[] atmLines;
    void MouseDragInput(InputEvent @event)
    {
        atmLines = dragLine.Points;
        if(@event is InputEventMouseButton mouseInput)
        {
            if(mouseInput.Pressed)
            {
                clickStartMousePos = mouseInput.Position;
                atmLines[0] = clickStartMousePos;
                atmLines[1] = clickStartMousePos;
                dragLine.SetPoints(atmLines);
                dragLine.Visible = true;
            }else
            {
                Vector2 dir = clickStartMousePos - mouseInput.Position;
                if(!gameStarted && canStartGame)
                {
                    StartGame();
                } 
                GameEvents.InputDragHeppend?.Invoke(dir);
                dragLine.Visible = false;
            }
        
        }

        if(@event is InputEventMouseMotion mouseMotion && dragLine.Visible)
        {
            atmLines[1] = mouseMotion.Position;
            dragLine.SetPoints(atmLines);
        }
    }

    void RestartGame()
    {
        GetTree().ReloadCurrentScene();
    }

    void CheckForRestartBTN(InputEvent @event)
    {
        if(@event is InputEventKey key && key.Scancode == (uint)KeyList.R && !restarting && canStartGame)
        {
            restarting = true;
            GD.PrintErr("Pritisnuo R");
            RestartGame();
        }
    }
}
