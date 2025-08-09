using Godot;
using NewGameProject.sim;
using NewGameProject.sim.creature;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

public partial class SimRenderer : Node2D
{
    private struct RenderedCell
    {
        public Sprite2D Sprite;
    }


    [Export] private Texture2D _barren; 
    [Export] private Texture2D[] _grass;
    private RenderedCell[,] _sprites;


    public override void _EnterTree()
    {
        base._EnterTree();
        
        _sprites = new RenderedCell[Simulation.WorldSize, Simulation.WorldSize];

        for (var x = 0; x < Simulation.WorldSize; x++)
        for (var y = 0; y < Simulation.WorldSize; y++)
        {
            _sprites[x, y] = InitCell(new Vector2(x, y) * Root.Instance.GridSize);
        }
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        for (var x = 0; x < _sprites.GetLength(0); x++)
        for (var y = 0; y < _sprites.GetLength(1); y++)
        {
            var sprite = _sprites[x, y];
            var pos = new SimulationPosition(x, y);
            var creature = Simulation.GetCreatureCell(pos);
            var plant = Simulation.GetPlantCell(pos);
            var terrain = Simulation.GetTerrainCell(pos);
            DrawCell(sprite, creature, plant, terrain);
        }
    }


    private RenderedCell InitCell(Vector2 at)
    {
        var o = new RenderedCell();

        o.Sprite = new Sprite2D();
        AddChild(o.Sprite); // IMPORTANT!
        o.Sprite.Position = at;
        return o;
    }


    private void DrawCell(RenderedCell rendCell, CreatureCell creature, PlantCell plant, TerrainType terrain)
    {
        // Plants
        if (plant == null)
        {
            rendCell.Sprite.Texture = _barren;
        }
        else if (plant.Type == PlantCellType.Grass)
        {
            var i = Mathf.FloorToInt(plant.Health / (float)(plant.MaxHealth + 1) * 4f);
            rendCell.Sprite.Texture = _grass[i];
        } 
    }
}