using UnityEngine;

public class ACM_AnimationSwapper : MonoBehaviour
{
    private Animator animator;
    private AnimatorOverrideController overrideController;
    public string animationKey = "Attack";
    public AnimationClip InstanceClip { get; private set; }
    public int RandomIndex { get; private set; }
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }
    public void SetAnimationClip(ChainAssassination context)
    {
        RandomIndex = Random.Range(0, context.copyAnimationData.Count);
        AnimationClip selectedClip = context.copyAnimationData[RandomIndex].clip;
        overrideController[animationKey] = selectedClip;
        animator.Play(animationKey, 0, 0f);
        animator.speed = 0f;
        InstanceClip = selectedClip;
    }
}
