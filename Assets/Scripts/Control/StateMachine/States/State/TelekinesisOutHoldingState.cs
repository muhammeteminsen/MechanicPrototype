using Control;

public class TelekinesisOutHoldingState : BaseHoldingState
{
    public override void OnEnter(Telekinesis telekinesis)
    {
        base.OnEnter(telekinesis);
        telekinesis.animationController?.Holding();
    }

    public override void OnUpdate(Telekinesis telekinesis)
    {
        base.OnUpdate(telekinesis);
        if (IsThrow) return;
        if (telekinesis.Direction() < telekinesis.holdingThreshold * telekinesis.holdingThreshold)
            telekinesis.ChangeState(new TelekinesisHoldingState());
    }
}