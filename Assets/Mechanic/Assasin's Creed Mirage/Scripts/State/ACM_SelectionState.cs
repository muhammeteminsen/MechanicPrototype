using System.Linq;
using UnityEngine;

public class ACM_SelectionState : StateAction<ChainAssassination>
{
    private ACM_ISelectableEnemy _hover;

    public override void OnEnter(ChainAssassination context)
    {
        Time.timeScale = 0f;
    }

    public override void OnUpdate(ChainAssassination context)
    {
        if (StateTransition(context)) return;
        Raycast(context);
        context.Move.HandleMovementUpdate();
    }

    public override void OnLateUpdate(ChainAssassination context)
    {
        context.Move.HandleCameraLateUpdate();
    }

    public override void OnExit(ChainAssassination context)
    {
        Time.timeScale = 1f;
        ClearHover(context);
    }

    private void Raycast(ChainAssassination context)
    {
        Ray ray = context.MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 25f, context.enemyLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            if (hit.collider.TryGetComponent(out ACM_ISelectableEnemy component))
            {
                PlayerInteraction(context, component);
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            ClearHover(context);
        }
    }

    private bool StateTransition(ChainAssassination context)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            context.ChangeState(new ACM_IdleState());
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            context.ChangeState(new ACM_AssassinationState());
            return true;
        }

        return false;
    }


    private void SelectedEnemy(ChainAssassination context, ACM_ISelectableEnemy enemy)
    {
        ACM_Enemy enemyClass = enemy as ACM_Enemy;
        if (enemyClass == null) return;
        if (!context.EnemyQueue.Contains(enemyClass))
            enemy.OnSelected(context);
        else
            enemy.OnDeselected(context);

        _hover = null;
    }

    private void HoveredEnemy(ChainAssassination context, ACM_ISelectableEnemy enemy)
    {
        ACM_Enemy enemyClass = enemy as ACM_Enemy;
        if (context.EnemyQueue.Contains(enemyClass))
        {
            ClearHover(context);
        }
        else if (_hover != enemy)
        {
            _hover?.OnUnSelect(context);
            _hover = enemy;
            _hover.OnSelectable(context);
        }
    }

    private void ClearHover(ChainAssassination context)
    {
        if (_hover == null) return;
        _hover?.OnUnSelect(context);
        _hover = null;
    }

    private void PlayerInteraction(ChainAssassination context, ACM_ISelectableEnemy enemy)
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectedEnemy(context, enemy);
        }

        HoveredEnemy(context, enemy);
    }
}