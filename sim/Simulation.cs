using NewGameProject.sim.creature;
using NewGameProject.sim.plant;
using NewGameProject.sim.terrain;

namespace NewGameProject.sim;

public static class Simulation
{
    private const int WorldSize = 70;
    
    private static readonly Terrain[,] TerraLayer = new Terrain[WorldSize, WorldSize];
    private static readonly Plant[,] FloraLayer = new Plant[WorldSize, WorldSize];
    private static readonly Creature[,] FaunaLayer = new Creature[WorldSize, WorldSize];

    public static void OnTick()
    {
        
    }
}