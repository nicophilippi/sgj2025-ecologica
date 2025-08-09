namespace NewGameProject.sim.plant;

public static class PlantCellTypeUtil
{
    public static int ProcreatePriority(PlantCell plantCell)
    {
        if (plantCell == null) return -1;

        return plantCell.Type switch
        {
            PlantCellType.Grass => 2,
            _ => -1
        };
    }
}