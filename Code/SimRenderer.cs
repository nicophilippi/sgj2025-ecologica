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
        public Sprite2D Ground;
        public Sprite2D Animal;
    }
    
    [Export] private Texture2D _barren; 
    [Export] private Texture2D[] _grass;
    [Export] private Texture2D[] _sheep;
    [Export] private Texture2D[] _wolf;
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

        o.Ground = new Sprite2D();
        o.Animal = new Sprite2D();
        AddChild(o.Ground); // IMPORTANT!
        AddChild(o.Animal);
        o.Ground.Position = at;
        o.Animal.Position = at;
        return o;
    }


    private void DrawCell(RenderedCell rendCell, CreatureCell creature, PlantCell plant, TerrainType terrain)
    {
        // Plants
        if (plant == null)
        {
            rendCell.Ground.Texture = _barren;
        }
        else if (plant.Type == PlantCellType.Grass)
        {
            var hp = Mathf.Min(plant.Health, plant.MaxHealth);
            var i = Mathf.FloorToInt(hp / (float)(plant.MaxHealth + 1) * 4f);
            rendCell.Ground.Texture = _grass[i];
        } 
        
        // Animals
        if (creature != null && creature.Type == CreatureCellType.Sheep)
        {
            var quant = Mathf.Min(creature.Quantity, creature.MaxQuantity);
            var i = Mathf.FloorToInt(quant / (float)(creature.MaxQuantity + 1) * 4f);
            rendCell.Animal.Texture = _sheep[i];
        }
        else
        {
            if (creature != null && creature.Type == CreatureCellType.Wolf)
            {
                var quant = Mathf.Min(creature.Quantity, creature.MaxQuantity);
                var i = Mathf.FloorToInt(quant / (float)(creature.MaxQuantity + 1) * 4f);
                rendCell.Animal.Texture = _wolf[i];
            }
        }
    }
}