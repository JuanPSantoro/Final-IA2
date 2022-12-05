using IA2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private EventFSM<ActionEntity> _fsm;
    private PlayerModel _model;

    private Inventory inventory;
    private Stamina stamina;
    private IEnumerable<GoapActionSO> _plan;
    private GoapActionSO _currentStep;
    private Entity entity;
    private Item _target;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        stamina = GetComponent<Stamina>();
        entity = GetComponent<Entity>();
        EventManager.instance.AddEventListener(EventType.FSM_NEXT_STEP, OnNextStep);
        EventManager.instance.AddEventListener(EventType.FSM_FAIL_STEP, OnFailStep);
        CreateFSM();
    }

    public void OnNextStep(params object[] parameter)
    {
        _fsm.Feed(ActionEntity.NextStep);
    }

    public void OnFailStep(params object[] parameter)
    {
        _fsm.Feed(ActionEntity.FailedStep);
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
            entity.GoTo(_target.transform.position);
            entity.OnReach += stamina.Rest;
        };

        idleState.OnExit += a =>
        {
            entity.OnReach -= stamina.Rest;
        };

        chopState.OnEnter += a =>
        {
            entity.GoTo(_target.transform.position);
            entity.OnReach += inventory.ChopWood;
        };

        chopState.OnExit += a =>
        {
            entity.OnReach -= inventory.ChopWood;
        };

        huntState.OnEnter += a =>
        {
            entity.GoTo(_target.transform.position);
            entity.OnReach += inventory.Hunt;
        };

        huntState.OnExit += a =>
        {
            entity.OnReach -= inventory.Hunt;
        };

        farmState.OnEnter += a =>
        {
            entity.GoTo(_target.transform.position);
            entity.OnReach += inventory.Farm;
        };

        farmState.OnExit += a =>
        {
            entity.OnReach -= inventory.Farm;
        };

        pickupState.OnEnter += a =>
        {
            entity.GoTo(_target.transform.position);
            inventory.TargetTool(_currentStep.target.ToString());
            entity.OnReach += inventory.PickUp;
        };

        pickupState.OnExit += a =>
        {
            entity.OnReach -= inventory.PickUp;
        };

        sleepState.OnEnter += a =>
        {
            entity.GoTo(_target.transform.position);
            entity.OnReach += stamina.Sleep;
        };

        sleepState.OnExit += a =>
        {
            entity.OnReach -= stamina.Sleep;
        };

        nextStepState.OnEnter += a => {
            var step = _plan.FirstOrDefault();

            if (step != null)
            {

                _plan = _plan.Skip(1);
                var oldTarget = _target;
                _target = Navigation.instance.GetNearestItem(transform.position, step.destination);
                _currentStep = step;
                if (!_fsm.Feed(step.actionEntity))
                {
                    Debug.Log("Couldn't transition to: " + step.actionEntity);
                    _fsm.Feed(ActionEntity.FailedStep);
                }
            }
            else
            {
                var destination = Navigation.instance.GetNearestItem(transform.position, Destination.IDLE_SPOT);
                entity.GoTo(destination.transform.position);
                entity.OnReach += Finish;
            }
        };

        buildState.OnEnter += a =>
        {
            Debug.Log("Enter Build");
            entity.GoTo(_target.transform.position);
            if (_currentStep.destination == Destination.UNBUILDED_FARM)
                _target.destination = Destination.FARM;
            else if (_currentStep.destination == Destination.UNBUILDED_HOUSE)
                _target.destination = Destination.HOUSE;
            entity.OnReach += entity.Build;
        };

        buildState.OnExit += a =>
        {
            Debug.Log("FINISH BUILDING " + _target);
            _target.transform.Find("Model").gameObject.SetActive(true);
            entity.OnReach -= entity.Build;
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

    void Finish()
    {
        entity.OnReach -= Finish;
        Debug.Log("No More Actions");
        _fsm.Feed(ActionEntity.Success);
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
