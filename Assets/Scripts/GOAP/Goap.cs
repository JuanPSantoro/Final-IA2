using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Goap : MonoBehaviour
{
    public static IEnumerable<GoapActionSO> Execute(GoapState from, GoapState to, Func<GoapState, bool> satisfies, Func<GoapState, float> h, IEnumerable<GoapActionSO> actions)
    {
        int watchdog = 500;

        IEnumerable<GoapState> seq = AStarNormal<GoapState>.Run (
            from,
            to,
            (curr, goal) => h(curr),
            satisfies,
            curr =>
            {
                if (watchdog == 0)
                    return Enumerable.Empty<AStarNormal<GoapState>.Arc>();
                else
                    watchdog--;

                var validActions = actions.Where(a => a.preconditions.All(pre => pre.ExecutePreCondition(curr.worldState))).ToList();

                //DebugState(curr);
                //DebugWS(curr.worldState);
                //DebugValidActions(validActions);

                return validActions.Aggregate(FList.Create<AStarNormal<GoapState>.Arc>(), (possibleList, action) =>
                    {
                        var newState = new GoapState(curr);
                        foreach (EffectSO currentEffect in action.effects)
                        {
                            newState.worldState = currentEffect.ExecuteEffect(newState.worldState);
                        }
                        newState.generator = action;
                        newState.step = curr.step + 1;
                        return possibleList + new AStarNormal<GoapState>.Arc(newState, action.cost);
                    }
                );
            }
        );

        Debug.Log("WATCHDOG " + watchdog);

        if (seq == null)
        {
            Debug.Log("Imposible planear");
            return null;
        }

        foreach (var act in seq.Skip(1))
        {
            Debug.Log(act);
        }

        return seq.Skip(1).Select(x => x.generator);
    }

    public static void DebugState(GoapState state)
    {
        if (state.generator != null)
            Debug.Log("COMES FROM: " + state.generator.actionName);
    }

    public static void DebugValidActions(IEnumerable<GoapActionSO> actions)
    {
        Debug.Log("---------------------------");
        foreach (var current in actions)
        {
            Debug.Log(current.actionName);
        }
        Debug.Log("---------------------------");
    }

    public static void DebugWS(WorldState ws)
    {
        Debug.Log("------------- WORLD STATE --------------");
        Debug.Log("E: " + ws.energy);
        Debug.Log("F: " + ws.food);
        Debug.Log("W: " + ws.wood);
        Debug.Log("Tool: " + ws.tool);
        Debug.Log("------------- WORLD STATE --------------");
    }
}
