using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Item
{
    [SerializeField]
    private GameObject _model;
    [SerializeField]
    private int _woodAmount;
    [SerializeField]
    private int _foodAmount;
    [SerializeField] 
    private float _staminaAmount;
    [SerializeField]
    private Destination _destinationAfterBuild;
    [SerializeField]
    private Inventory _inventory;

    private void Start()
    {
        if (destination == Destination.UNBUILDED_FARM || destination == Destination.UNBUILDED_HOUSE)
            _model.SetActive(false);
    }

    public void Build()
    {
        StartCoroutine(OnBuild());
    }

    private IEnumerator OnBuild()
    {
        EventManager.instance.TriggerEvent(EventType.BUILD_PARTICLE_PLAY, new object[] { transform.position });
        yield return new WaitForSeconds(5);
        EventManager.instance.TriggerEvent(EventType.BUILD_PARTICLE_STOP);
        destination = _destinationAfterBuild;
        _inventory.ConsumeFood(_foodAmount);
        _inventory.ConsumeWood(_woodAmount);
        _model.SetActive(true);
        EventManager.instance.TriggerEvent(EventType.STAMINA_SPENT, new object[] { 90f });
        EventManager.instance.TriggerEvent(EventType.FSM_NEXT_STEP);
    }
}
