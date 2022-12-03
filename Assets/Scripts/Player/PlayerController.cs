using IA2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private EventFSM<ActionEntity> _fsm;
    private PlayerModel _model;

    public Item _tree;
    public Item _farm;
    public Item _deposit;
    public Item _masterHouse;
    public Item _house;
    public Item _animal;

    private Inventory inventory;
    private Stamina stamina;
    private IEnumerable<GoapActionSO> _plan;
    private Entity entity;
    private Item _target;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        stamina = GetComponent<Stamina>();
        entity = GetComponent<Entity>();
        CreateFSM();
        stamina.SetFSM(_fsm);
        inventory.SetFSM(_fsm);
        entity.SetFSM(_fsm);
    }

    public void CreateFSM()
    {
        var idleState = new State<ActionEntity>("idle");
        var chopState = new State<ActionEntity>("chop");
        var huntState = new State<ActionEntity>("hunt");
        var farmState = new State<ActionEntity>("farm");
        var pickupState = new State<ActionEntity>("pickup");
        var sleepState = new State<ActionEntity>("sleep");
        var buildState = new State<ActionEntity>("build");

        var anyState = new State<ActionEntity>("any");
        var nextStepState = new State<ActionEntity>("nextStep");
        var endState = new State<ActionEntity>("end");

        idleState.OnEnter += a =>
        {
            Debug.Log("Enter Idle");
            stamina.Rest();
        };

        chopState.OnEnter += a =>
        {
            Debug.Log("Enter Chop");
            var destination = Navigation.instance.GetNearestItem(transform.position, Destination.TREE);
            entity.GoTo(destination.transform.position);
            entity.OnReach += inventory.ChopWood;
        };

        chopState.OnExit += a =>
        {
            entity.OnReach -= inventory.ChopWood;
        };

        huntState.OnEnter += a =>
        {
            Debug.Log("Enter Hunt");
            var destination = Navigation.instance.GetNearestItem(transform.position, Destination.PIG);
            entity.GoTo(destination.transform.position);
            entity.OnReach += inventory.Hunt;
        };

        huntState.OnExit += a =>
        {
            entity.OnReach -= inventory.Hunt;
        };

        farmState.OnEnter += a =>
        {
            Debug.Log("Enter Farm");
            var destination = Navigation.instance.GetNearestItem(transform.position, Destination.FARM);
            entity.GoTo(_farm.transform.position);
            entity.OnReach += inventory.Farm;
        };

        farmState.OnExit += a =>
        {
            entity.OnReach -= inventory.Farm;
        };

        pickupState.OnEnter += a =>
        {
            Debug.Log("Enter Pickup ");
            var destination = Navigation.instance.GetNearestItem(transform.position, Destination.DEPOSIT);
            entity.GoTo(destination.transform.position);
            entity.OnReach += inventory.PickUp;
        };

        pickupState.OnExit += a =>
        {
            entity.OnReach -= inventory.PickUp;
        };


        sleepState.OnEnter += a =>
        {
            Debug.Log("Enter Sleep");
            var destination = Navigation.instance.GetNearestItem(transform.position, Destination.HOUSE);
            entity.GoTo(destination.transform.position);
            entity.OnReach += stamina.Sleep;
        };

        sleepState.OnExit += a =>
        {
            entity.OnReach -= stamina.Sleep;
        };

        nextStepState.OnEnter += a => {
            var step = _plan.FirstOrDefault();

            Debug.Log("Enter Next");

            if (step != null)
            {

                _plan = _plan.Skip(1);
                var oldTarget = _target;
                _target = _tree;
                if (!_fsm.Feed(step.actionEntity))
                    _target = oldTarget;
            }
            else
            {
                Debug.Log("No More Actions");

                _fsm.Feed(ActionEntity.Success);
            }
        };

        buildState.OnEnter += a =>
        {
            Debug.Log("Enter Build");

            entity.GoTo(_farm.transform.position);
            entity.OnReach += entity.Build;
        };

        StateConfigurer.Create(anyState)
            .SetTransition(ActionEntity.NextStep, nextStepState)
            .SetTransition(ActionEntity.FailedStep, endState)
            .Done();

        StateConfigurer.Create(nextStepState)
            .SetTransition(ActionEntity.Chop, chopState)
            .SetTransition(ActionEntity.PickUp, pickupState)
            .SetTransition(ActionEntity.Hunt, huntState)
            .SetTransition(ActionEntity.Farm, farmState)
            .SetTransition(ActionEntity.Build, buildState)
            .SetTransition(ActionEntity.Idle, idleState)
            .SetTransition(ActionEntity.Sleep, sleepState)
            .SetTransition(ActionEntity.Success, endState)
            .Done();

        _fsm = new EventFSM<ActionEntity>(endState, anyState);
    }

    public void ExecutePlan(IEnumerable<GoapActionSO> plan)
    {
        _plan = plan;
        _fsm.Feed(ActionEntity.NextStep);
    }

    void Update()
    {
        _fsm.Update();
    }
}

public enum ActionEntity
{
    Kill,
    NextStep,
    FailedStep,
    Open,
    Success,

    PickUp,
    Chop,
    Hunt,
    Farm,
    Build,
    Sleep,
    Idle
}
