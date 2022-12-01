using System.Collections.Generic;
using System.Linq;

public class GoapState
{
    public WorldState worldState;


    public GoapActionSO generator = null;

    public int step = 0;

    #region CONSTRUCTOR
    public GoapState()
    {
        worldState = new WorldState();
    }

    public GoapState(GoapState source)
    {
        worldState = source.worldState.Clone();
    }
    #endregion

    public override bool Equals(object obj)
    {
        var result =
            obj is GoapState other
            && other.generator == generator
            && other.worldState.energy == worldState.energy
            && other.worldState.food == worldState.food
            && other.worldState.wood == worldState.wood
            && other.worldState.tool == worldState.tool
            && other.worldState.farms == worldState.farms
            && other.worldState.houses == worldState.houses;
        return result;
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public override string ToString()
    {
        var str = "";
        return ("--->" + (generator != null ? generator.actionName : "NULL") + "\n" + str);
    }
}

public struct WorldState
{
    public float energy;
    public int wood;
    public int food;
    public int farms;
    public bool houses;
    public string tool;

    public WorldState Clone()
    {
        return new WorldState()
        {
            energy = this.energy,
            tool = this.tool,
            wood = this.wood,
            food = this.food,
            farms = this.farms,
            houses = this.houses
        };
    }
}
