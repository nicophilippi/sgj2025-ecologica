using Godot;

public partial class Root : Node
{
    public static Root Instance;
    [field: Export] public Vector2 GridSize { get; private set; } = new(64f, 64f);
    
    
    public Vector2I GlobalToIndex(Vector2 global)
    {
        global /= GridSize;
        return new Vector2I(Mathf.RoundToInt(global.X), Mathf.RoundToInt(global.Y));
    }
    
    
    private SimTimer timer;
    public override void _EnterTree()
    {
        base._EnterTree();
        timer = new SimTimer();
        AddChild(timer);
        timer.ChangeTimer();
        Instance = this;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("pause"))
        {
            GD.Print("Paused");
            timer.ChangeTimer();
        }
    }
}
