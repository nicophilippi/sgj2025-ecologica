using Godot;

public partial class Card : Node2D
{
    public virtual void Play()
    {
        GD.Print("Played: " + Name);
    }
}
