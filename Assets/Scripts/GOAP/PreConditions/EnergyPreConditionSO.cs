using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Precondition", menuName = "GOAP/Preconditions/Energy", order = 58)]
public class EnergyPreConditionSO : PreConditionSO
{
    public float energyAmount;
    public override bool ExecutePreCondition(WorldState ws)
    {
        return ExectuteNumberComparative(ws.energy, energyAmount);
    }
}
