using Control;
using UnityEngine;

public class BaseHoldingState : StateAction<Telekinesis>
{
    protected ITelekinesisable Telekinesisable;
    protected bool IsThrow;
    public override void OnEnter(Telekinesis telekinesis)
    {
        if (telekinesis.Closest.TryGetComponent(out ITelekinesisable telekinesisable))
            Telekinesisable = telekinesisable;
    }
    public override void OnUpdate(Telekinesis telekinesis)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            telekinesis.ChangeState(new TelekinesisThrowState());
            IsThrow = true;
        }
    }
    public override void OnFixedUpdate(Telekinesis telekinesis)
    {
        Telekinesisable?.Holding(telekinesis.holdingPoint, telekinesis.holdingSpeed,
            telekinesis.MainCamera);
    }

    public override void OnExit(Telekinesis telekinesis)
    {
        IsThrow = false;
    }
}
