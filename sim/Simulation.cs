using System;
using Godot;
using NewGameProject.sim.creature;
using NewGameProject.sim.intention.cell;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;
using NewGameProject.sim.util;

namespace NewGameProject.sim;

public static class Simulation
{
    public const int WorldSize = 70;

    public static int TickCount = 0; 
    public static int SimulationPoints = 0;
    
    private static readonly TerrainType[,] TerraLayer = new TerrainType[WorldSize, WorldSize];
    private static readonly PlantCell[,] FloraLayer = new PlantCell[WorldSize, WorldSize];
    private static readonly CreatureCell[,] FaunaLayer = new CreatureCell[WorldSize, WorldSize];

    private static readonly MoveIntentionCell[,] MoveIntentionLayer = new MoveIntentionCell[WorldSize, WorldSize];
    private static readonly EatIntentionCell[,] EatIntentionLayer = new EatIntentionCell[WorldSize, WorldSize];
    private static readonly ProcreateIntentionCell[,] ProcreateIntentionLayer = new ProcreateIntentionCell[WorldSize, WorldSize];

    private static readonly Random Random = new();

    private static void ForEachWorldPosition(Action<int, int> action)
    {
        for (var x = 0; x < WorldSize; ++x)
        {
            for (var y = 0; y < WorldSize; ++y)
            {
                action(x, y);
            }
        }
    }

    static Simulation()
    {
        for (int x = 0; x < WorldSize; ++x)
        {
            for (int y = 0; y < WorldSize; ++y)
            {
                TerraLayer[x, y] = TerrainType.Plains;
                FloraLayer[x, y] = null;
                FaunaLayer[x, y] = null;

                MoveIntentionLayer[x, y] = new MoveIntentionCell();
                EatIntentionLayer[x, y] = new EatIntentionCell();
                ProcreateIntentionLayer[x, y] = new ProcreateIntentionCell();
            }
        }

        FloraLayer[35, 35] = new GrassCell();
        FaunaLayer[35, 36] = new SheepCell(1);
    }

    public static void OnTick()
    {
        ForEachWorldPosition((x, y) => FaunaLayer[x, y]?.ComputeMoveIntentions(x, y, TerraLayer, FloraLayer, FaunaLayer, MoveIntentionLayer));
        ForEachWorldPosition((x, y) => MoveIntentionLayer[x, y].DeconflictMoveIntentions(x, y, SimulationLayer.Fauna, TerraLayer, FloraLayer, FaunaLayer, MoveIntentionLayer));
        
        ForEachWorldPosition((x, y) => FaunaLayer[x, y]?.ComputeEatIntentions(x, y, TerraLayer, FloraLayer, FaunaLayer, EatIntentionLayer));
        ForEachWorldPosition((x, y) => EatIntentionLayer[x, y].DeconflictEatIntentions(x, y, SimulationLayer.Fauna, TerraLayer, FloraLayer, FaunaLayer, EatIntentionLayer));
        
        ForEachWorldPosition((x, y) => FloraLayer[x, y]?.ComputeProcreateIntentions(x, y, TerraLayer, FloraLayer, FaunaLayer, ProcreateIntentionLayer));
        ForEachWorldPosition((x, y) => ProcreateIntentionLayer[x, y].DeconflictProcreateIntentions(x, y, SimulationLayer.Flora, TerraLayer, FloraLayer, FaunaLayer, ProcreateIntentionLayer));
        
        ForEachWorldPosition((x, y) => FaunaLayer[x, y]?.ComputeProcreateIntentions(x, y, TerraLayer, FloraLayer, FaunaLayer, ProcreateIntentionLayer));
        ForEachWorldPosition((x, y) => ProcreateIntentionLayer[x, y].DeconflictProcreateIntentions(x, y, SimulationLayer.Fauna, TerraLayer, FloraLayer, FaunaLayer, ProcreateIntentionLayer));
        count_entities();
        TickCount++;
    }
    
    public static void count_entities()
    {
        int num_plants = 0;
        int num_animals = 0;
        ForEachWorldPosition((x, y) =>
        {
            num_plants += FloraLayer[x, y] == null ? 0 : FloraLayer[x, y].Health;
            num_animals += FaunaLayer[x, y] == null ? 0 : FaunaLayer[x, y].Quantity;
        });

        SimulationPoints += num_plants;
        SimulationPoints += num_animals;
        GD.Print($"Tick: {TickCount}: Plants: {num_plants}, Animals: {num_animals}");
    }
    
    public static TerrainType GetTerrainCell(SimulationPosition i) => TerraLayer[i.X, i.Y];
    
    public static PlantCell GetPlantCell(SimulationPosition i) => FloraLayer[i.X, i.Y];
    
    public static CreatureCell GetCreatureCell(SimulationPosition i) => FaunaLayer[i.X, i.Y];

    
    
    public static void SetTerrainCell(SimulationPosition position, TerrainType terrainType) => TerraLayer[position.X, position.Y] = terrainType;

    public static void SetPlantCell(SimulationPosition position, PlantCell cell) => FloraLayer[position.X, position.Y] = cell;

    public static void SetCreatureCell(SimulationPosition position, CreatureCell cell) => FaunaLayer[position.X, position.Y] = cell;



    public static int RandomIntBetween(int min, int max) => Random.Next(min, max);
}