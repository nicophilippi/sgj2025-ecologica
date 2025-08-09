using Godot;
using NewGameProject.sim;
using NewGameProject.sim.plant;

public abstract partial class SetCard : Card
{
    // 0 => 1x1; 1 => 3x3; 2 => 5x5
    [Export] public int AddRadius = 0;


    public override void Play()
    {
        base.Play();
        var mousePos = Camera2d.Instance.GetGlobalMousePosition();
        var mouseIndex = Root.Instance.GlobalToIndex(mousePos);

        for (var x = mouseIndex.X - AddRadius; x <= mouseIndex.X + AddRadius; x++)
        for (var y = mouseIndex.Y - AddRadius; y <= mouseIndex.Y + AddRadius; y++)
        {
            Simulation.SetPlantCell(new Vector2I(x, y), new GrassCell
            {
                Health = GrassCell.MAX_HEALTH,
            });
        }
    }


    public abstract void Set(Vector2I where);
}