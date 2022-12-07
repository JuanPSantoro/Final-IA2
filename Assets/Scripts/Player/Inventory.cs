using IA2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int wood;
    public int food;
    public string tool;
    
    private string toolToReach;

    [SerializeField]
    private ToolContainer _toolContainer;

    [SerializeField]
    private Transform _toolPositionReference;

    private void Start()
    {
        EventManager.instance.TriggerEvent(EventType.FOOD_CHANGE, new object[] { food });
        EventManager.instance.TriggerEvent(EventType.WOOD_CHANGE, new object[] { wood });
        EventManager.instance.TriggerEvent(EventType.TOOL_CHANGE, new object[] { tool });
    }

    public void ChopWood()
    {
        StartCoroutine(OnChopWood());
    }

    private IEnumerator OnChopWood()
    {
        EventManager.instance.TriggerEvent(EventType.CHOP_PARTICLE_PLAY, new object[] { transform.position });
        yield return new WaitForSeconds(1);
        EventManager.instance.TriggerEvent(EventType.CHOP_PARTICLE_STOP);
        wood += 200;
        EventManager.instance.TriggerEvent(EventType.STAMINA_SPENT, new object[] { 60f });
        EventManager.instance.TriggerEvent(EventType.WOOD_CHANGE, new object[] { wood });
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void Hunt()
    {
        StartCoroutine(OnHunt());
    }

    private IEnumerator OnHunt()
    {
        EventManager.instance.TriggerEvent(EventType.HUNT_PARTICLE_PLAY, new object[] { transform.position });
        yield return new WaitForSeconds(1);
        EventManager.instance.TriggerEvent(EventType.HUNT_PARTICLE_STOP);
        food += 50;
        EventManager.instance.TriggerEvent(EventType.STAMINA_SPENT, new object[] { 5f });
        EventManager.instance.TriggerEvent(EventType.FOOD_CHANGE, new object[] { food });
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void Farm()
    {
        StartCoroutine(OnFarm());
    }

    private IEnumerator OnFarm()
    {
        EventManager.instance.TriggerEvent(EventType.FARM_PARTICLE_PLAY, new object[] { transform.position });
        yield return new WaitForSeconds(1);
        EventManager.instance.TriggerEvent(EventType.FARM_PARTICLE_STOP);
        food += 10;
        EventManager.instance.TriggerEvent(EventType.STAMINA_SPENT, new object[] { 30f });
        EventManager.instance.TriggerEvent(EventType.FOOD_CHANGE, new object[] { food });
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void PickUp()
    {
        StartCoroutine(OnPickup());
    }

    private IEnumerator OnPickup()
    {
        yield return new WaitForSeconds(1);
        if (tool != "")
            _toolContainer.DropTool(tool);

        tool = toolToReach;
        _toolContainer.PickupTool(tool, _toolPositionReference, Vector3.zero);
        EventManager.instance.TriggerEvent(EventType.TOOL_CHANGE, new object[] { tool });
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void TargetTool(string tool)
    {
        toolToReach = tool;
    }

    public void ConsumeWood(int amount)
    {
        wood -= amount;
        EventManager.instance.TriggerEvent(EventType.WOOD_CHANGE, new object[] { wood });
    }

    public void ConsumeFood(int amount)
    {
        food -= amount;
        EventManager.instance.TriggerEvent(EventType.FOOD_CHANGE, new object[] { food });
    }
}
