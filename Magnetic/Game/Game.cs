using Godot;
using System;

public class Game : Node2D
{
    [Export] public NodePath dragLineNP;
    Line2D dragLine;

    public static Vector2 oldMagnetForceDir = Vector2.One;
    public static Vector2 newMagnetForceDir = Vector2.One;
    float speed = 1;
    float switchDirSpeed = 0.2f;
    bool gameStarted;
    bool gameEnded;
    float passedTime = 0f;

    public static int score;


    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.GameEnd += OnGameEnd;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public void OnGameEnd()
    {
        gameEnded = true;
    }

    public void StartGame()
    {
        gameStarted = true;
        GameEvents.GameStart?.Invoke();
    }

    public override void _Ready()
    {
        dragLine = GetNode<Line2D>(dragLineNP);
        newMagnetForceDir = GetNewRandomDir();
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
            GD.PrintErr("SwitchDir");
            newMagnetForceDir = GetNewRandomDir();
        }
    }


    Vector2 clickStartMousePos;
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        MouseDragInput(@event);
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
                if(!gameStarted) StartGame();
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
}
