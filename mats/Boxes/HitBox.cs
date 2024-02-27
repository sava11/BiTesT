using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class HitBox : Area2D
{
	[Export(PropertyHint.Range, "0,100,1,or_greater")] 
	public uint DamagePoints=1;
	[Export(PropertyHint.Range, "0,1,0.001")] float Enabled=1;
	[Export(PropertyHint.Range, "0,1,0.001")] float Hited=0.8f;
	[Export] public Vector2 Speed;
	[Export] public float RotationSpeed=0;
	Vector2 safeAreaSize;
	Vector2 safeAreaStart=new Vector2(-640,-640);
    public override void _Ready()
    {
       safeAreaSize= GetTree().Root.GetViewport().GetVisibleRect().Size;
    }
    public override void _Draw()
    {
		foreach(Node n in GetChildren()){
			if(n is CollisionShape2D){
				if(((CollisionShape2D)n).Shape is RectangleShape2D){
					RectangleShape2D s=(RectangleShape2D)((CollisionShape2D)n).Shape;
					DrawRect(new Rect2(-s.Size/2,s.Size),SelfModulate,true);
				}
				if(((CollisionShape2D)n).Shape is CircleShape2D){
					CircleShape2D s=(CircleShape2D)((CollisionShape2D)n).Shape;
					DrawCircle(new Vector2(0,0),s.Radius,SelfModulate);
				}
				if(((CollisionShape2D)n).Shape is CapsuleShape2D){			
					CapsuleShape2D s=(CapsuleShape2D)((CollisionShape2D)n).Shape;
					DrawCircle(new Vector2(0,-s.Height/2+s.Radius),s.Radius,SelfModulate);
					DrawCircle(new Vector2(0,s.Height/2-s.Radius),s.Radius,SelfModulate);
					DrawRect(new Rect2(
						new Vector2(-s.Radius,-s.Height/2+s.Radius),
						new Vector2(s.Radius*2,s.Height-s.Radius*2)
						),SelfModulate,true);
				}
				if(n is CollisionPolygon2D){
					DrawColoredPolygon(((CollisionPolygon2D)n).Polygon,SelfModulate,null,null);
				}
			}
		}
    }
    public override void _Process(double delta)
    {
		SelfModulate=new Color(SelfModulate.R,SelfModulate.G,SelfModulate.B,Enabled);
		QueueRedraw();
		SetDeferred(
			PropertyName.Monitorable,Enabled>=Hited);
		SetDeferred(
			PropertyName.Monitoring,Enabled>=Hited);
        GlobalPosition+=Speed*(float)delta;
		RotationDegrees+=(float)(RotationSpeed * (180/Math.PI));
		if( GlobalPosition.X > safeAreaSize.X-safeAreaStart.X || 
			GlobalPosition.X < safeAreaStart.X || 
			GlobalPosition.Y > safeAreaSize.Y-safeAreaStart.Y || 
			GlobalPosition.Y < safeAreaStart.Y)
			QueueFree();
    }
}
