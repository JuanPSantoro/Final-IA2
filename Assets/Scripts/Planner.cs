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

    private void Start()
    {
        EventManager.instance.AddEventListener(EventType.RE_PLAN, OnReplan);
    }

    private void OnReplan(object[] parameters)
    {
        StartPlan();
    }

    public void StartPlan()
    {
        if (_goal != null && _heuristic != null)
            StartCoroutine(Plan());
    }

    private IEnumerator Plan() {
		yield return new WaitForSeconds(0.2f);
		
        GoapState initial = new GoapState();
        initial.worldState = GetWorldState();

        Func<GoapState, float> h = (curr) =>
        {
            return _heuristic.ProcessHeuristic(curr.worldState);
        };

        var currentSleepCount = 0;
        var replanOnEnd = false;
        Func<GoapState, bool> objective = (curr) =>
        {
            if (_goal.Satisfies(curr.worldState))
            {
                return true;
            }
            else
            {
                if ((curr.generator != null && curr.generator.actionName == "Sleep"))
                {
                    currentSleepCount++;
                    if (currentSleepCount > _goal.sleepsToInterrupt)
                    {
                        replanOnEnd = true;
                        return true;
                    }
                }
            }
            return false;
        };

		var plan = Goap.Execute(initial, null, objective, h, actions);

		if (plan == null)
			Debug.Log("Couldn't plan");
		else {
            FindObjectOfType<ActionsUI>().ShowUI();
            FindObjectOfType<PlayerController>().ExecutePlan(plan, replanOnEnd);
		}
	}

    private WorldState GetWorldState()
    {
        WorldState ws = new WorldState();
        var inventory = GetComponent<Inventory>();
        var city = FindObjectOfType<CityManager>();
        ws.energy = GetComponent<Stamina>().CurrentEnergy;
        ws.wood = inventory.wood;
        ws.food = inventory.food;
        ws.tool = inventory.tool;
        ws.farms = city.FarmsAmount();
        ws.houses = city.IsHouseBuilded();
        return ws;
    }
}
