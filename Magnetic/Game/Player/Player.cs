using Godot;
using System;

public class Player : RigidBody2D
{

    bool gameEnd;

    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.InputDragHeppend += OnInputDragHeppend;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.InputDragHeppend -= OnInputDragHeppend;
    }

    // public override void _Ready()
    // {
    //     base._Ready();
    //     Connect("body_entered", this, nameof(OnBodyEnter));
    // }

    // public void OnBodyEnter(Node body)
    // {
    //     if(body is StaticBody2D)
    //     {
    //         GD.PrintErr("WallHit");
    //     }
    // }

    private void OnInputDragHeppend(Vector2 dir)
    {
        AddForce(Vector2.Zero, dir);
    }



    public override void _Process(float delta)
    {
        base._Process(delta);
        CheckPlayerHitWall();
    }

    void CheckPlayerHitWall()
    {
        if
        (
            !gameEnd &&
            (
            Position.x < 20 || 
            Position.x > 340 ||
            Position.y < 20 ||
            Position.y > 620
            )
        )
        {
            GD.PrintErr("CRASHED");
            gameEnd = true;
            GameEvents.GameEnd?.Invoke();
        }
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }
}
