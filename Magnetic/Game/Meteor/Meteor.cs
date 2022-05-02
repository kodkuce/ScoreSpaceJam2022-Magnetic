using Godot;
using System;

public class Meteor : RigidBody2D
{
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(LinearVelocity.Length() < 20)
            AddCentralForce(Game.oldMagnetForceDir*0.001f);
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        base._IntegrateForces(state);
        if(
            GlobalPosition.x < -1000 ||
            GlobalPosition.x > 1000 ||
            GlobalPosition.y > 1000 ||
            GlobalPosition.y < -1000
        )
        {
            //reset postion 
            Vector2 dir = Game.oldMagnetForceDir.LinearInterpolate(Game.newMagnetForceDir,1).Normalized().Rotated((float)GD.RandRange(-1.0,1.0));
            Vector2 randomNewPos = new Vector2(180,320) - dir * 370;

            state.LinearVelocity = Vector2.Zero;
            state.AngularVelocity = 0;
            Transform2D transform2D = state.Transform;
            transform2D.origin = randomNewPos;
            state.Transform = transform2D;
        }
    }
}
