using Control;
using UnityEngine;

public class TelekinesisLiftState : StateAction<Telekinesis>
{
    public override void OnEnter(Telekinesis telekinesis)
    {
        if (telekinesis.Closest.TryGetComponent(out ITelekenisable telekinesisable))
        {
            telekinesis.TelekinesisAbility.Cancellation();
            telekinesis.TelekinesisAbility.Decrease().Forget();
            telekinesisable.Initialize(telekinesis);
            telekinesisable.Lift(telekinesis.liftPoint, telekinesis.liftOffset, telekinesis.liftSpeed,
                telekinesis.liftDuration, telekinesis /* on finally change state */ ).Forget();
        }
            
    }

    public override void OnUpdate(Telekinesis telekinesis)
    {
        if (Input.GetKeyDown(KeyCode.E) && telekinesis.Closest.TryGetComponent(out ITelekenisable telekinesisable))
        {
            telekinesisable.CancellationToken();
        }
    }
}