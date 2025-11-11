using System;
using Control;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TelekinesisLiftState : StateAction<Telekinesis>
{
    private Func<bool> _onComplete;
    public override void OnEnter(Telekinesis telekinesis)
    {
        if (telekinesis.Closest.TryGetComponent(out ITelekinesisable telekinesisable))
        {
            telekinesis.liftFeedback?.PlayFeedbacks();
            telekinesis.TelekinesisAbility.Cancellation();
            telekinesis.TelekinesisAbility.Decrease().Forget();
            telekinesisable.Initialize(telekinesis);
            telekinesisable.Lift(telekinesis.liftPoint, telekinesis.liftOffset, telekinesis.liftSpeed,
                telekinesis.liftDuration, telekinesis /* on finally change state */).Forget();
        }
    }

    public override void OnUpdate(Telekinesis telekinesis)
    {
        if (Input.GetKeyDown(KeyCode.E) && telekinesis.Closest.TryGetComponent(out ITelekinesisable telekinesisable))
        {
            telekinesisable.CancellationToken();
        }
    }

    public override void OnExit(Telekinesis telekinesis)
    {
        telekinesis.holdingFeedback?.PlayFeedbacks();
        telekinesis.EffectController(telekinesis.holdingEffectPrefab);
    }

   
}