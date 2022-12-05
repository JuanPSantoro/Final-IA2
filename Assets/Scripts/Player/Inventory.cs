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
    private UnityEvent<int> _onWoodUpdate;
    [SerializeField]
    private UnityEvent<int> _onFoodUpdate;
    [SerializeField]
    private UnityEvent<string> _onToolChange;

    public void ChopWood()
    {
        StartCoroutine(OnChopWood());
    }

    private IEnumerator OnChopWood()
    {
        yield return new WaitForSeconds(1);
        wood += 30;
        _onWoodUpdate?.Invoke(wood);
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void Hunt()
    {
        StartCoroutine(OnHunt());
    }

    private IEnumerator OnHunt()
    {
        yield return new WaitForSeconds(1);
        food += 30;
        _onFoodUpdate?.Invoke(food);
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void Farm()
    {
        StartCoroutine(OnFarm());
    }

    private IEnumerator OnFarm()
    {
        yield return new WaitForSeconds(1);
        food += 50;
        _onFoodUpdate?.Invoke(food);
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void PickUp()
    {
        StartCoroutine(OnPickup());
    }

    private IEnumerator OnPickup()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("PICKUP " + toolToReach);
        _onToolChange?.Invoke(toolToReach);
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }

    public void TargetTool(string tool)
    {
        toolToReach = tool;
    }
}
