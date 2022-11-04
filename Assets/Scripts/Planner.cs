using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Planner : MonoBehaviour 
{
    public List<GoapActionSO> actions;
    private GoapObjectiveSO _goal;
    private GoapHeuristicSO _heuristic;

    public GoapObjectiveSO Objective { set { _goal = value; } }
    public GoapHeuristicSO Heuristic { set { _heuristic = value; } }

    public void StartPlan()
    {
        if (_goal != null && _heuristic != null)
            StartCoroutine(Plan());
    }

    private IEnumerator Plan() {
		yield return new WaitForSeconds(0.2f);

		var observedState = new WorldState();
		
        GoapState initial = new GoapState();
        initial.worldState = new WorldState();
        observedState.energy = 100;

        initial.worldState = observedState;

        Func<GoapState, float> h = (curr) =>
        {
            return _heuristic.ProcessHeuristic(curr.worldState);
        };

        Func<GoapState, bool> objective = (curr) =>
        {
            return _goal.Satisfies(curr.worldState);
        };

		var plan = Goap.Execute(initial, null, objective, h, actions);

		if (plan == null)
			Debug.Log("Couldn't plan");
		else {
			/*GetComponent<Guy>().ExecutePlan(
				plan
				.Select(a => 
                {
                    Item i2 = everything.FirstOrDefault(i => i.target == a.target);
                    if (actDict.ContainsKey(a.actionName) && i2 != null)
                    {
                        return Tuple.Create(actDict[a.actionName], i2);
                    }
                    else
                    {
                        return null;
                    }
				}).Where(a => a != null)
				.ToList()
			);*/
		}
	}
}
