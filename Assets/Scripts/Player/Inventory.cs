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

    [SerializeField]
    private UnityEvent<int> _onWoodUpdate;
    [SerializeField]
    private UnityEvent<int> _onFoodUpdate;
    [SerializeField]
    private UnityEvent<string> _onToolChange;

    private EventFSM<ActionEntity> _fsm;

    public void SetFSM(EventFSM<ActionEntity> fsm)
    {
        _fsm = fsm;
    }

    public void ChopWood()
    {
        StartCoroutine(OnChopWood());
    }

    private IEnumerator OnChopWood()
    {
        yield return new WaitForSeconds(1);
        wood += 30;
        _onWoodUpdate?.Invoke(wood);
        _fsm.Feed(ActionEntity.NextStep);
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
        _fsm.Feed(ActionEntity.NextStep);

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
        _fsm.Feed(ActionEntity.NextStep);

    }

    public void PickUp()
    {
        StartCoroutine(OnPickup(Items.AXE));
    }

    public void PickUp(Items tool)
    {
        StartCoroutine(OnPickup(tool));
    }

    private IEnumerator OnPickup(Items newTool)
    {
        yield return new WaitForSeconds(1);
        tool = newTool.ToString();
        _onToolChange?.Invoke(tool);
        _fsm.Feed(ActionEntity.NextStep);

    }
}
