using Cysharp.Threading.Tasks;
using UnityEngine;

public class ACM_AssassinationState : StateAction<ChainAssassination>
{
    public override void ReEnter(ChainAssassination context)
    {
        Assassination(context).Forget();
    }

    public override void OnEnter(ChainAssassination context)
    {
        context.Move.ExitMovement();
        Assassination(context).Forget();
    }

    public override void OnLateUpdate(ChainAssassination context)
    {
        context.Move.HandleCameraLateUpdate();
    }

    private async UniTaskVoid Assassination(ChainAssassination context)
    {
        ACM_Enemy currentEnemyObj = context.EnemyQueue[0];
        if (currentEnemyObj.EffectInstance.TryGetComponent(out ACM_AnimationSwapper animationSwapper))
        {
            AnimationClip clip = InitializeAnimation(animationSwapper, context);
            PlayerTransformation(currentEnemyObj, context);
            currentEnemyObj.OnDeselected(context);
            float duration = clip.length * context.copyAnimationData[animationSwapper.RandomIndex].threshold;
            await UniTask.WaitForSeconds(duration);
            Object.Destroy(currentEnemyObj.gameObject);
            HandleTransition(context);
        }
    }

    private AnimationClip InitializeAnimation(ACM_AnimationSwapper animationSwapper, ChainAssassination context)
    {
        AnimationClip instanceClip = animationSwapper.InstanceClip;
        AnimationClip clip = context.SetAnimationClip(instanceClip);
        return clip;
    }
    private void PlayerTransformation(ACM_Enemy currentEnemyObj, ChainAssassination context)
    {
        Vector3 targetPosition = currentEnemyObj.effectPos.position;
        Quaternion targetRotation = currentEnemyObj.effectPos.rotation;
           
        context.transform.position = targetPosition;
        context.transform.rotation = targetRotation;
    }

    private void HandleTransition(ChainAssassination context)
    {
        if (context.EnemyQueue.Count <= 0)
        {
            context.ChangeState(new ACM_IdleState());
            return;
        }
        context.ChangeState(new ACM_AssassinationState());
    }
}
