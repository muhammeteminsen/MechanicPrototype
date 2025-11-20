using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainAssassination : MonoBehaviour
{
    private StateBase<ChainAssassination> _stateMachine;
    public LayerMask enemyLayer;
    public Image crosshair;
    public Camera MainCamera => Camera.main;
    public ACM_Move Move => GetComponent<ACM_Move>();
    public List<ACM_Enemy> EnemyQueue { get; set; }= new List<ACM_Enemy>();

    private void Awake()
    {
        _stateMachine = new StateBase<ChainAssassination>(this);
        _stateMachine.ChangeState(new ACM_IdleState());
        crosshair.color = Color.white;
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