using UnityEngine;

public class ACM_IdleState : StateAction<ChainAssassination>
{
    public override void OnEnter(ChainAssassination telekinesis)
    {
        Time.timeScale = 1f;
    }
    public override void OnUpdate(ChainAssassination assassination)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            assassination.ChangeState(new ACM_SelectionState());
            return;
        }
        assassination.Move.HandleMovementUpdate();
    }

    public override void OnLateUpdate(ChainAssassination assassination)
    {
        assassination.Move.HandleCameraLateUpdate();
    }
}