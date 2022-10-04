using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Energy Effect", menuName = "GOAP/Effects/Energy", order = 58)]

public class EnergyEffectSO : EffectSO
{
    public float energy;
    public override void ExecuteEffect(WorldState ws)
    {
        ws.energy = ExecuteInNumber(ws.energy, energy, 0, 9999);
    }
}
