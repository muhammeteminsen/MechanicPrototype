using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private void Update()
    {
        Debug.Log($"GameObject Name:  {gameObject.name}, Current Animation Clip: {InstanceClip?.name}, Random Index: {RandomIndex}");
    }

    public void SetSpecificClip(AnimationClip targetClip, float animationSpeed = 0f, float startTime = 0f)
    {
        overrideController[animationKey] = targetClip;
        animator.Play(animationKey, 0, startTime);
        animator.speed = animationSpeed;
        InstanceClip = targetClip;
    }
    public void PlayRandomClip(ChainAssassination context, float startTime = 0f)
    {
        RandomIndex = Random.Range(0, context.copyAnimationData.Count);
        AnimationClip selectedClip = context.copyAnimationData[RandomIndex].clip;
        SetSpecificClip(selectedClip,startTime: startTime);
    }

    public void PauseAnimation()
    {
        animator.speed = 0f;
    }
    
    public void ResumeAnimation()
    {
        animator.speed = 1f;
    }
}
