namespace NewGameProject.sim.creature;

public static class CreatureCellTypeUtil
{
    // High number = high priority
    public static int MovePriority(CreatureCell creatureCell)
    {
        if (creatureCell == null) return -1;

        return creatureCell.Type switch
        {
            CreatureCellType.Sheep => 2,
            _ => -1
        };
    }
    
    public static int ProcreatePriority(CreatureCell creatureCell)
    {
        if (creatureCell == null) return -1;

        return creatureCell.Type switch
        {
            CreatureCellType.Sheep => 2,
            _ => -1
        };
    }
}