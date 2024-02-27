using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal] 
	public delegate void DeadEventHandler();
	[Export(PropertyHint.Range, "1,1000,1,or_greater")] 
	float Speed=500f;
	[Export(PropertyHint.Range, "1,1000,1,or_greater")] 
	float Jump=500f;
	[Export(PropertyHint.Range, "1,1000,1,or_greater")] 
	float Acceleration=2200f;
	[Export(PropertyHint.Range, "0.001,1000,0.001,or_greater")] 
	float SpiritTimer=0.5f;
	[Export(PropertyHint.Range, "1,1000,1,or_greater")] 
	float SpiritSpeed=700f;
	[Export(PropertyHint.Range, "1,1000,1,or_greater")] 
	float SpiritAcceleration=2500f;
	public override void _Ready()
	{
		GetNode<ColorRect>("Skin").Size=((RectangleShape2D)GetNode<CollisionShape2D>("HurtBox/c").Shape).Size;
		GetNode<ColorRect>("Skin").Position=-((RectangleShape2D)GetNode<CollisionShape2D>("HurtBox/c").Shape).Size/2;
	}
	bool alive=true;
   	Vector2 mv=Vector2.Zero;//move vector
	double SpiritTimerTemp=0;
	[Export]
	bool physicalBody=true;
    public override void _PhysicsProcess(double delta)
    {
		if (alive){
			//move vector direction
			Vector2 mvd=new Vector2(
				Input.GetActionStrength("Right") - Input.GetActionStrength("Left"),
				Input.GetActionStrength("Down") - Input.GetActionStrength("Up"));
			if(MotionMode==CharacterBody2D.MotionModeEnum.Floating){
				//Current speed + acceleration
				if (Input.IsActionJustPressed("spirit")){
					physicalBody=false;
					GetNode<HurtBox>("HurtBox").SetDeferred(
						HurtBox.PropertyName.Monitorable,physicalBody);
					GetNode<HurtBox>("HurtBox").SetDeferred(
						HurtBox.PropertyName.Monitoring,physicalBody);
				}
				if(!physicalBody){
					Color c=GetNode<ColorRect>("Skin").Color;
					GetNode<ColorRect>("Skin").Color=new Color(c.R,c.G,c.B,0.5f);
					SpiritTimerTemp+=delta;
					if (SpiritTimerTemp>=SpiritTimer){
						SpiritTimerTemp=0;
						physicalBody=true;
						mv=mvd*Speed;
						GetNode<HurtBox>("HurtBox").SetDeferred(
							HurtBox.PropertyName.Monitorable,physicalBody);
						GetNode<HurtBox>("HurtBox").SetDeferred(
							HurtBox.PropertyName.Monitoring,physicalBody);
					}
					else{
						mv=mv.MoveToward(mvd*SpiritSpeed, (float)(SpiritAcceleration*delta));
					}
				}
				else
				{
					Color c=GetNode<ColorRect>("Skin").Color;
					GetNode<ColorRect>("Skin").Color=new Color(c.R,c.G,c.B,1f);
					mv=mv.MoveToward(mvd*Speed,(float)(delta*Acceleration));
				}
			}
			if(MotionMode==CharacterBody2D.MotionModeEnum.Grounded){
				bool ground = IsOnFloor();
				if (ground)
					mv.X=mv.MoveToward(mvd*Speed,(float)(delta*Acceleration)).X;
				else
					mv.X=mv.MoveToward(mvd*Speed,(float)(delta*Acceleration/4)).X;
				if (IsOnWall()){
					//GD.Print(GetWallNormal().X);
					if (
						(GetWallNormal().X<0 && mv.X>0) ||
						(GetWallNormal().X>0 && mv.X<0) ){
						mv.X=0;
					}
				}
				if (Input.IsActionJustPressed("spirit")){
					mv.Y-=Jump;
				}
				if (!ground){
					mv.Y+=(float)(980*delta);
				}
				if (ground && !Input.IsActionJustPressed("spirit")){
					mv.Y=0;
				}
			}
			Velocity=mv;
			//GD.Print(Velocity);
		}
		else
		{
			mv=mv.MoveToward(Vector2.Zero,(float)(delta*Acceleration));
			Velocity=mv;
		}
		MoveAndSlide();
	}
	public void ZeroLimit(){
		if(alive){
			alive=false;
			GetNode<ColorRect>("Skin").Material.Set("shader_parameter/sector",1f);
			GetNode<ColorRect>("Skin").Color=new Color(0.5f,0.5f,0.5f,1);
			GetNode<AudioStreamPlayer>("asp").Stream=
			ResourceLoader.Load<AudioStreamWav>("res://mats/sounds/twrauw.wav");
			GetNode<AudioStreamPlayer>("asp").Play();
		}
	}
	public void Hited(){
		GetNode<AnimationPlayer>("ap").Play("hit");
		GetNode<ColorRect>("Skin").Material.Set("shader_parameter/sector",
		GetNode<HurtBox>("HurtBox").HeartPoints/(float)GetNode<HurtBox>("HurtBox").MaxHeartPoints);
		GetNode<AudioStreamPlayer>("asp").Stream=
		ResourceLoader.Load<AudioStream>("res://mats/sounds/twrauw.wav");
		GetNode<AudioStreamPlayer>("asp").Play();
		}
}
