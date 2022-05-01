using Godot;
using System;

public class Player : Node2D
{
    [Export] public NodePath head;

    [Export] public NodePath[] armLeftConnectors;
    [Export] public NodePath armLeft;

    RigidBody2D headRB;
    RigidBody2D armLeftRB;
    PinJoint2D[] armLeftPinJoints;

    bool lockArm;
    Transform2D lockArmTransform;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        headRB = GetNode<RigidBody2D>(head);
        armLeftRB = GetNode<RigidBody2D>(armLeft);
        
        armLeftPinJoints = new PinJoint2D[armLeftConnectors.Length];
        for(int i=0;i<armLeftConnectors.Length;i++)
        {
            armLeftPinJoints[i] = GetNode<PinJoint2D>(armLeftConnectors[i]);
        }
    }

    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsKeyPressed((int)KeyList.J))
        {
            GD.Print("KISSa");
            softness = 16;
            armLeftRB.AddForce(Vector2.Zero, new Vector2(-1,-2)*100);
        }

        if(Input.IsKeyPressed((int)KeyList.K))
        {
            GD.Print("Stop");
            lockArmTransform = armLeftRB.Transform;
            lockArm = true;
            headRB.Mass = 1;
            armLeftRB.LinearVelocity = Vector2.Zero;
            armLeftRB.AngularVelocity = 0;
            armLeftRB.CustomIntegrator = true;
            armLeftRB.Mass = 10;
        }

        if(Input.IsKeyPressed((int)KeyList.L))
        {
            GD.Print("Unstop");
            lockArm = false;
            armLeftRB.CustomIntegrator = false;
            armLeftRB.Mass = 1;
            headRB.Mass = 10;
        }

        if(lockArm)
        {
            armLeftRB.Transform = lockArmTransform;
        }

        ProcessSmoothens(delta);
    }

    float softness = 0;
    void ProcessSmoothens(float delta)
    {
        if(softness>0)
        {
            softness -= delta*2;
            foreach(PinJoint2D pinJoint2D in armLeftPinJoints)
            {
                pinJoint2D.Softness = softness;
            }
            if(delta % 5 == 0)
            {
                GD.Print(softness);
            }
        }
    }
}
