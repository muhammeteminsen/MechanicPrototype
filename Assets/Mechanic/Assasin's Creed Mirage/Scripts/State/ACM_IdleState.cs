using System.Linq;
using UnityEngine;

public class ACM_IdleState : StateAction<ChainAssassination>
{
    public override void OnEnter(ChainAssassination context)
    {
        foreach (var enemy in context.EnemyList.ToList())
            enemy.OnDeselected(context);
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