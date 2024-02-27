using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;

public partial class rain : Node2D
{
	[Export] Color DamageColor=new Color(1f,0.2f,0.75f,1);
	[Export] NodePath AddingNodePath;
	Node AddingNode;
	[Export] float SummonAreaSize=10;
	[Export] public bool Summoning=false;
	[Export] float TimeBetweenSummon=0.1f;
	[Export] bool angled=false;
	[Export] Dictionary settings;
	[Export] public float ItemSpeed;
	[Export] public float ItemRotation=0;
	[Export] public float ItemRotationSpeed=0;
	double timer=0;
	RandomNumberGenerator rnd=new RandomNumberGenerator();
	public override void _Ready(){
		rnd.Seed=0;
		AddingNode=GetNode(AddingNodePath);
	}
	public override void _Process(double delta)
	{
		if(Summoning){
			timer+=delta;
			if(timer>=TimeBetweenSummon){
				HitBox hb=(HitBox)((PackedScene)ResourceLoader.Load("res://mats/Boxes/HitBox.tscn")).Instantiate();
				if (settings.ContainsKey(0)){
					CollisionShape2D c=new CollisionShape2D();
					c.Shape=new RectangleShape2D();
					((RectangleShape2D)c.Shape).Size=(Vector2)((Dictionary)settings[0])["Size"];
					hb.AddChild(c);
				}
				if (settings.ContainsKey(1)){
					CollisionShape2D c=new CollisionShape2D();
					c.Shape=new CircleShape2D();
					((CircleShape2D)c.Shape).Radius=(float)((Dictionary)settings[1])["Radius"];
					hb.AddChild(c);
				}
				if (settings.ContainsKey(2)){
					CollisionShape2D c=new CollisionShape2D();
					c.Shape=new CapsuleShape2D();
					((CapsuleShape2D)c.Shape).Radius=(float)((Dictionary)settings[2])["Radius"];
					((CapsuleShape2D)c.Shape).Height=(float)((Dictionary)settings[2])["Height"];
					hb.AddChild(c);
				}
				hb.Speed=new Vector2(
					(float)Math.Cos(GlobalRotationDegrees*(Math.PI/180)),
					(float)Math.Sin(GlobalRotationDegrees*(Math.PI/180))
					)*ItemSpeed;
				hb.RotationSpeed=ItemRotationSpeed;
				Vector2 lp=new Vector2(0,rnd.RandfRange(-SummonAreaSize/2,SummonAreaSize/2));
				if (angled){
					hb.GlobalPosition=GlobalPosition+Functions.Move(
						GlobalRotationDegrees+Functions.Angle(lp)
					)*lp.DistanceTo(Vector2.Zero);
				}else{
					hb.GlobalPosition=GlobalPosition+lp;
				}
				hb.SelfModulate=DamageColor;
				hb.CollisionLayer=8;
				hb.RotationDegrees=ItemRotation;
				AddingNode.AddChild(hb);
				timer=0;
			}
		}
	}
}
