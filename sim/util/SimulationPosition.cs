using System;
using Godot;

namespace NewGameProject.sim.util;

public readonly struct SimulationPosition(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public SimulationPosition Offset(SimulationDirection direction)
    {
        return direction switch
        {
            SimulationDirection.Up => new SimulationPosition(X, Y - 1),
            SimulationDirection.Right => new SimulationPosition(X + 1, Y),
            SimulationDirection.Down => new SimulationPosition(X, Y + 1),
            SimulationDirection.Left => new SimulationPosition(X - 1, Y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public void ForEachDirectionBreaking(Predicate<SimulationPosition> predicate)
    {
        if (Y > 0)
            if (predicate(Offset(SimulationDirection.Up))) return;
        
        if (X < Simulation.WorldSize - 1)
            if (predicate(Offset(SimulationDirection.Right))) return;
        
        if (X > 0)
            if (predicate(Offset(SimulationDirection.Left))) return;
        
        if (Y < Simulation.WorldSize - 1)
            predicate(Offset(SimulationDirection.Down));
    }


    public static implicit operator SimulationPosition(Vector2I i) => new(i.X, i.Y);
}