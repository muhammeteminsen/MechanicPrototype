using Control;

public class TelekinesisHoldingState : BaseHoldingState
{
    public override void OnEnter(Telekinesis telekinesis)
    {
        base.OnEnter(telekinesis);
        telekinesis.JitterCamera();
        Telekinesisable?.JitterObject(telekinesis, telekinesis.duration, telekinesis.strength,
            telekinesis.vibrato, telekinesis.randomness);
    }

    public override void OnUpdate(Telekinesis telekinesis)
    {
        base.OnUpdate(telekinesis);
        if (IsThrow) return;
        if (telekinesis.Direction().magnitude > telekinesis.holdingThreshold)
            telekinesis.ChangeState(new TelekinesisOutHoldingState());
    }

    public override void OnExit(Telekinesis telekinesis)
    {
        base.OnExit(telekinesis);
        Telekinesisable?.JitterDispose();
    }
}