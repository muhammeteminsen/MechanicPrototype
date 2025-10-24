using Control;
using UnityEngine;

public class BaseHoldingState : StateAction<Telekinesis>
{
    protected ITelekenisable Telekinesisable;
    public override void OnEnter(Telekinesis telekinesis)
    {
        if (telekinesis.Closest.TryGetComponent(out ITelekenisable telekinesisable))
            Telekinesisable = telekinesisable;
    }
    public override void OnUpdate(Telekinesis telekinesis)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            telekinesis.ChangeState(new TelekinesisThrowState());
            return;
        }
    }
    public override void OnFixedUpdate(Telekinesis telekinesis)
    {
        Telekinesisable?.Holding(telekinesis.holdingPoint, telekinesis.holdingSpeed,
            telekinesis.MainCamera);
    }
}
