using Godot;
using System;

public partial class HurtBox : Area2D
{
	[Signal] 
	public delegate void InvulnerabilityStartedEventHandler();
	[Signal] 
	public delegate void InvulnerabilityEndedEventHandler();
	[Signal] 
	public delegate void HitedEventHandler();
	[Signal] 
	public delegate void ZeroLimitEventHandler();
	[Export(PropertyHint.Range, "1,100,1,or_greater")]
	public int HeartPoints=1;
	public int MaxHeartPoints=1;
	Timer t=null;
	[Export(PropertyHint.Range, "0,10,0.001,or_greater")]
	public double InvulnerableTimer=1.5;
	private bool _invulnerable;
	public bool Invulnerable{
		set => SetInvulnerability(value);
		get => _invulnerable;
	}
	public bool SetInvulnerability(bool v){
		_invulnerable=v;
		SetDeferred(PropertyName.Monitorable,!v);
		SetDeferred(PropertyName.Monitoring,!v);
		if (v)
			EmitSignal("InvulnerabilityStarted");
		else
			EmitSignal("InvulnerabilityEnded");
		return _invulnerable;
	}

	public void StartInvulnerability(double dir){
		Invulnerable=true;
		t.Start(dir);
		}
	public void TimerTimeout(){
		Invulnerable=false;

	}
	public override void _Ready()
	{
		t=(Timer)GetNode("Timer");
		MaxHeartPoints=HeartPoints;
	}
	public new void BodyEntered(HitBox HiB){
		if (!Invulnerable){
			HeartPoints-=(int)HiB.Get("DamagePoints");
			if (HeartPoints<=0){
				EmitSignal("ZeroLimit");
				HeartPoints=0;
			}else{
				EmitSignal("Hited");
			}
			StartInvulnerability(InvulnerableTimer);
		}
	}
}
