
using UnityEngine;

public class StateBase<T> where T : class
{
    private StateAction<T> _currentState; 
    private readonly T _context;
    public StateBase(T context)
    {
        _context = context;
    }
    
    public StateAction<T> ChangeState(StateAction<T>  newStateAction)
    {
        if (_currentState == newStateAction)
        {
            _currentState?.ReEnter(_context);
            return _currentState;
        }
        _currentState?.OnExit(_context);
        _currentState = newStateAction;
        _currentState.OnEnter(_context);
        return _currentState;
    }
    public void Update()
    {
        _currentState?.OnUpdate(_context);
    }
    public void FixedUpdate()
    {
        _currentState?.OnFixedUpdate(_context);
    }
    public void LateUpdate()
    {
        _currentState?.OnLateUpdate(_context);
    }
}