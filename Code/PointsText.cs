using Godot;
using System;

public partial class PointsText : RichTextLabel
{
    private static long DisplayPoints = 0;

    public override void _EnterTree()
    {
        base._EnterTree();
        Text = $"Points: {DisplayPoints}";
    }

    public static void updatePoints(long points)
    {
        DisplayPoints = points;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Text = $"Points: {DisplayPoints}";
    }
    
}
