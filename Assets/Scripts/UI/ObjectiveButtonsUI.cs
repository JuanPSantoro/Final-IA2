public class ObjectiveButtonsUI : HiddeableUI
{
    override protected void Start()
    {
        EventManager.instance.AddEventListener(EventType.FSM_FINISH, OnFSMFinished);
        base.Start();
    }

    private void OnFSMFinished(object[] parameters)
    {
        ShowUI();
    }
}
