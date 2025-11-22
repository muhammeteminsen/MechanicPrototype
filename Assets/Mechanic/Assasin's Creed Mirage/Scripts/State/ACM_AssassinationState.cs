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
        ACM_Enemy currentEnemyObj = context.EnemyList[0];
        if (currentEnemyObj.EffectInstance.TryGetComponent(out ACM_AnimationSwapper instanceAnimationSwapper) &&
            currentEnemyObj.TryGetComponent(out ACM_AnimationSwapper enemyAnimationSwapper))
        {
            AnimationClip clip = InitializeAnimation(instanceAnimationSwapper, context);
            PlayerTransformation(currentEnemyObj, context);
            currentEnemyObj.OnDeselected(context);
            float duration = clip.length * context.copyAnimationData[instanceAnimationSwapper.RandomIndex].threshold;
            await UniTask.WaitForSeconds(duration);
            InitializeAnimation(enemyAnimationSwapper, context, instanceAnimationSwapper);
            enemyAnimationSwapper.PauseAnimation();
            context.PauseAnimation();
            await UniTask.WaitForSeconds(1f);
            context.ResumeAnimation();
            HandleTransition(context);
        }
    }

    private AnimationClip InitializeAnimation(ACM_AnimationSwapper instanceAnimationSwapper, ChainAssassination context)
    {
        AnimationClip instanceClip = instanceAnimationSwapper.InstanceClip;
        AnimationClip clip = context.PlayAnimation(instanceClip);
        return clip;
    }

    private void InitializeAnimation(ACM_AnimationSwapper enemyAnimationSwapper, ChainAssassination context,
        ACM_AnimationSwapper instanceAnimationSwapper)
    {
        enemyAnimationSwapper.SetSpecificClip(
            context.copyAnimationData[instanceAnimationSwapper.RandomIndex].targetClip,
            1f
        , context.copyAnimationData[instanceAnimationSwapper.RandomIndex].startTime);
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
        if (context.EnemyList.Count <= 0)
        {
            foreach (var enemy in context.EnemyTempList)
            {
                ACM_AnimationSwapper animationSwapper = enemy.GetComponent<ACM_AnimationSwapper>();
                animationSwapper.ResumeAnimation();
            }
            context.EnemyTempList.Clear();
            context.ChangeState(new ACM_IdleState());
            return;
        }

        context.ChangeState(new ACM_AssassinationState());
    }
}