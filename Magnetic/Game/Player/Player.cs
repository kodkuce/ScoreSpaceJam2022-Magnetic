using Godot;
using System;

public class Player : RigidBody2D
{

    bool gameEnd;
    bool gameStarted;

    public override void _EnterTree()
    {
        base._EnterTree();
        GameEvents.InputDragHeppend += OnInputDragHeppend;
        GameEvents.GameStart += OnGameStart;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        GameEvents.InputDragHeppend -= OnInputDragHeppend;
        GameEvents.GameStart -= OnGameStart;
    }

    private void OnInputDragHeppend(Vector2 dir)
    {
        if(gameStarted && !gameEnd)
            AddForce(Vector2.Zero, dir*0.75f);
    }

    private void OnGameStart()
    {
        gameStarted = true;
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
            gameStarted && !gameEnd &&
            (
            Position.x < 20 || 
            Position.x > 340 ||
            Position.y < 20 ||
            Position.y > 620
            )
        )
        {
            GD.PrintErr("CRASHED");
            SetProcess(false);
            SetPhysicsProcess(false);
            SetPhysicsProcessInternal(false);
            LinearVelocity = Vector2.Zero;
            AngularVelocity = 0;
            CustomIntegrator = true;
            gameEnd = true;

            GetNode<CPUParticles2D>("DamageParticle").Emitting = true;
            GameEvents.GameEnd?.Invoke();
        }
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);
        state.AngularVelocity = 0;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(gameStarted && !gameEnd)
            AddCentralForce(Game.oldMagnetForceDir*delta*0.1f);
    }
}
