using UnityEngine;

public class ACM_AssassinationState : StateAction<ChainAssassination>
{
    public override void OnEnter(ChainAssassination assassination)
    {
        assassination.Move.ExitMovement();
        Time.timeScale = 1f;
    }
}
