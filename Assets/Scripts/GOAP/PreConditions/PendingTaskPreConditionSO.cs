using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pending Task Precondition", menuName = "GOAP/Preconditions/Pending Task", order = 58)]
public class PendingTaskPreConditionSO : PreConditionSO
{
    public bool pendingTask;
    public override bool ExecutePreCondition(WorldState ws)
    {
        return ExecuteBoolComparative(ws.hasPendingTask, pendingTask);
    }
}
