namespace NewGameProject.sim.creature;

public static class CreatureCellTypeUtil
{
    // High number = high priority
    public static int MovePriority(CreatureCellType type) => type switch
    {
        CreatureCellType.Sheep => 2,
        _ => 0
    };
    
    public static int ProcreatePriority(CreatureCellType type) => type switch
    {
        CreatureCellType.Sheep => 2,
        _ => 0
    };
}