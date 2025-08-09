using Godot;
using NewGameProject.sim;

public static class LoseReferences
{
    public static Vector2I SimSize => new Vector2I(Simulation.WorldSize, Simulation.WorldSize);


    public static WhateverTheFuckTileDataLooksLike GetTile(int x, int y) => null;

    public class WhateverTheFuckTileDataLooksLike
    {
        
    }
}