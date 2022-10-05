using System.Collections.Generic;
using System.Linq;

public class GoapState
{
    public WorldState worldState;


    public GoapAction generatingAction = null;
    public GoapActionSO generator = null;

    public int step = 0;

    #region CONSTRUCTOR
    public GoapState(GoapAction gen = null)
    {
        generatingAction = gen;
        worldState = new WorldState()
        {
            values = new Dictionary<string, bool>() // Muy importane inicializarlo en este caso
        };
    }

    public GoapState(GoapState source, GoapAction gen = null)
    {
        worldState = source.worldState.Clone();
        generatingAction = gen;
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
            && other.worldState.night == worldState.night
            && other.worldState.hasPendingTask == worldState.hasPendingTask
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
    public int houses;
    public bool hasPendingTask;
    public string tool;
    public bool night;

    public Dictionary<string, bool> values;

    public WorldState Clone()
    {
        return new WorldState()
        {
            energy = this.energy,
            tool = this.tool,
            wood = this.wood,
            food = this.food,
            farms = this.farms,
            houses = this.houses,
            hasPendingTask = this.hasPendingTask,
            night = this.night,

            //values = this.values.ToDictionary(kv => kv.Key, kv => kv.Value)
        };
    }
}
