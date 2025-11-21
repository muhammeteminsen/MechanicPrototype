using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CopyAnimationData
{
    public AnimationClip clip;
    [Range(0f,1f)]public float threshold;
}
public class ChainAssassination : MonoBehaviour
{
    private StateBase<ChainAssassination> _stateMachine;
    public List<CopyAnimationData> copyAnimationData;
    public GameObject effectPrefab;
    [SerializeField] private string animationClipKey = "Attack";
    
    public LayerMask enemyLayer;
    public Image crosshair;
    public Camera MainCamera => Camera.main;
    public ACM_Move Move => GetComponent<ACM_Move>();
    private Animator Animator => GetComponent<Animator>();
    public List<ACM_Enemy> EnemyQueue { get; set; }= new List<ACM_Enemy>();
    private AnimatorOverrideController _overrideController;

    private void Awake()
    {
        _stateMachine = new StateBase<ChainAssassination>(this);
        _stateMachine.ChangeState(new ACM_IdleState());
        crosshair.color = Color.white;
        _overrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        Animator.runtimeAnimatorController = _overrideController;
    }
    
    private void Update()
    {
        _stateMachine.Update();
    }

    private void LateUpdate()
    {
        _stateMachine.LateUpdate();
    }
    private StateAction<ChainAssassination> currentState;
    public void ChangeState(StateAction<ChainAssassination> newState)
    {
       currentState = _stateMachine.ChangeState(newState);
    }

    public AnimationClip SetAnimationClip(AnimationClip clip)
    {
        _overrideController[animationClipKey] = clip;
        Animator.Play(animationClipKey, 0, 0f);
        Animator.speed = 1f;
        return clip;
    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            fontSize = 40,
            normal =
            {
                textColor = Color.black
            }
        };
        GUI.Label(new Rect(10, 10, 1000, 30), "Current State: " + currentState?.GetType().Name, style);
    }
}