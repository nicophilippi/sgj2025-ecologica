using System;
using NewGameProject.sim.creature;
using NewGameProject.sim.intention;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim;

public static class Simulation
{
    private const int WorldSize = 70;
    
    private static readonly TerrainCell[,] TerraLayer = new TerrainCell[WorldSize, WorldSize];
    private static readonly PlantCell[,] FloraLayer = new PlantCell[WorldSize, WorldSize];
    private static readonly CreatureCell[,] FaunaLayer = new CreatureCell[WorldSize, WorldSize];

    private static readonly IntentionCell[,] IntentionLayer = new IntentionCell[WorldSize, WorldSize];

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

    public static void OnTick()
    {
        ForEachWorldPosition((x, y) => TerraLayer[x, y].ComputeMoveIntentions(x, y, TerraLayer, FloraLayer, FaunaLayer, IntentionLayer));
    }
}