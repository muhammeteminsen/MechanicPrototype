using UnityEngine;

public class ACM_SelectionState : StateAction<ChainAssassination>
{
    private ACM_ISelectableEnemy _hover;

    public override void OnEnter(ChainAssassination telekinesis)
    {
        Time.timeScale = 0f;
    }

    public override void OnUpdate(ChainAssassination assassination)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            assassination.ChangeState(new ACM_IdleState());
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            assassination.ChangeState(new ACM_AssassinationState());
            return;
        }

        RaycastController(assassination);
        assassination.Move.HandleMovementUpdate();
    }

    public override void OnLateUpdate(ChainAssassination assassination)
    {
        assassination.Move.HandleCameraLateUpdate();
    }

    public override void OnExit(ChainAssassination assassination)
    {
        if (assassination.EnemyQueue.Count > 0)
        {
            foreach (var enemy in assassination.EnemyQueue)
            {
                enemy.OnUnSelect(assassination);
            }

            assassination.EnemyQueue.Clear();
        }
        else
        {
            _hover?.OnUnSelect(assassination);
        }

        _hover = null;
    }

    private void RaycastController(ChainAssassination assassination)
    {
        Ray ray = assassination.MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 25f, assassination.enemyLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            if (hit.collider.TryGetComponent(out ACM_ISelectableEnemy component))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!assassination.EnemyQueue.Contains(component as ACM_Enemy))
                        component.OnSelected(assassination);
                    else
                        assassination.EnemyQueue.Remove(component as ACM_Enemy);
                    _hover = null;
                }

                if (!assassination.EnemyQueue.Contains(component as ACM_Enemy))
                {
                    _hover?.OnSelectable(assassination);
                    if (_hover != component)
                        _hover?.OnUnSelect(assassination);
                    _hover = component;
                }
                else
                {
                    _hover?.OnUnSelect(assassination);
                }
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
            _hover?.OnUnSelect(assassination);
        }
    }
}