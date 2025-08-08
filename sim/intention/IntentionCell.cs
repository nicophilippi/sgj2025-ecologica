using System.Collections.Generic;

namespace NewGameProject.sim.intention;

public struct IntentionCell(List<Intention> intentions)
{
    private List<Intention> Intentions { get; set; } = intentions;

    public MoveIntention DeconflictMoveIntentions()
    {
        return null;
    }

    public EatIntention DeconflictEatIntentions()
    {
        return null;
    }

    public ProcreateIntention DeconflictProcreateIntentions()
    {
        return null;
    }
}