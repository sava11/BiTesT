using Godot;
using System;

public partial class Functions : Node
{
    static public Vector2 Move(double ang){
        return new Vector2(
            (float)Math.Cos(ang * (Math.PI/180)),
            (float)Math.Sin(ang * (Math.PI/180))
            );
    }
    static public double Angle(Vector2 vector){
        return Math.Atan2(-vector.Y,vector.X)* (180/Math.PI);
    }
    static public int ISearch(Array a, Object i){
	int inte=-1;
	foreach(Object k in a){
		inte+=1;
		if (k==i){
			return inte;
        }
    }
	return inte;
    }
}
