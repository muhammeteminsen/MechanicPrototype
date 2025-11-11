using Control;
using UnityEngine;

public class TelekinesisThrowState : StateAction<Telekinesis>
{
    public override void OnEnter(Telekinesis telekinesis)
    {
        telekinesis.EffectController(effect: telekinesis.holdingEffectPrefab, play: false);
        telekinesis.throwFeedback?.PlayFeedbacks();
        telekinesis.JitterDisposeCamera();
        telekinesis.animationController?.Throw();
        if (telekinesis.Closest.TryGetComponent(out ITelekinesisable telekinesisable))
        {
            telekinesisable.Throw(telekinesis.throwForce, telekinesis.MainCamera,
                telekinesis.maxDistance);
            telekinesis.ChangeState(new TelekinesisIdleState());
        }
    }
}