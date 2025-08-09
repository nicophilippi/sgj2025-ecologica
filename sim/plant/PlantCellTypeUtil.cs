namespace NewGameProject.sim.plant;

public static class PlantCellTypeUtil
{
    public static int ProcreatePriority(PlantCellType type) => type switch
    {
        PlantCellType.Barren => 0,
        PlantCellType.Grass => 1,
        _ => 0
    };
}