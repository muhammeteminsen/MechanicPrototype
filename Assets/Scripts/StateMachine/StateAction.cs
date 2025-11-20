
public abstract class StateAction<T>
{
    public virtual void ReEnter(T context){}
    public virtual void OnEnter(T telekinesis){}
    public virtual void OnExit(T telekinesis){}
    public virtual void OnUpdate(T telekinesis){}
    public virtual void OnFixedUpdate(T context){}
    public virtual void OnLateUpdate(T context){}
}
