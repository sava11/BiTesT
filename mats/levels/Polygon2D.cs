using System.Collections.Generic;
using Godot;

public partial class Polygon2D : Godot.Polygon2D
{
	[Export] int w=1;
	[Export] Vector2 pos;
	[Export] Vector2[] lines;
	public override void _Ready()
	{
		if (pos!=Vector2.Zero)
			Position=Functions.Move(pos.X)*pos.Y;
		var pol=new List<Vector2>();
		for( int i=0;i<lines.Length;i++)
		{
			Vector2 e=lines[i];
			Vector2 ps=Functions.Move(e.X+90)*e.Y;
			pol.Add(ps);
		}
		if (w>=1){
			for( int o=0;o<lines.Length;o++){
				Vector2 i=lines[lines.Length-o-1];
				pol.Add(Functions.Move(i.X+90)*(i.Y+w));}
		}
		Polygon=pol.ToArray();
	}
}
