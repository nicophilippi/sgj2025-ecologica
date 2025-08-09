using Godot;

public partial class Camera2d : Camera2D
{
    public static Camera2d Instance { get; private set; }
    
    
    [Export] private Vector2 _zoomSpeed = Vector2.One;
    [Export] private float _zoomMin = 0.5f;
    [Export] private float _zoomMax = 2f;
    private const int scroll_pixels = 1000;
    
    private Vector2 leftstep = new Vector2(-scroll_pixels, 0);
    private Vector2 rightstep = new Vector2(scroll_pixels, 0);
    private Vector2 upstep = new Vector2(0, -scroll_pixels);
    private Vector2 downstep = new Vector2(0, scroll_pixels);

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("zoom in"))
        {
            Zoom -= _zoomSpeed;
        } if (Input.IsActionJustPressed("zoom out"))
        {
            Zoom += _zoomSpeed;
        } if (Input.IsActionPressed("camera up"))
        {
            Position += upstep / Zoom * (float) delta;
        } if (Input.IsActionPressed("camera down"))
        {
            Position += downstep / Zoom * (float) delta;
        } if (Input.IsActionPressed("camera left"))
        {
            Position += leftstep / Zoom * (float) delta;
        } if (Input.IsActionPressed("camera right"))
        {
            Position += rightstep / Zoom * (float) delta;
        }

        Zoom = new Vector2(Mathf.Clamp(Zoom.X, _zoomMin, _zoomMax),
            Mathf.Clamp(Zoom.Y, _zoomMin, _zoomMax));

        if (Input.IsActionPressed("camera return"))
        {
            Zoom = Vector2.One;
            Position = Vector2.Zero;
        }
    }


    public override void _EnterTree()
    {
        base._EnterTree();
        if (Instance != null) GD.PrintErr("Singleton has second instance!");
        Instance = this;
    }
}
