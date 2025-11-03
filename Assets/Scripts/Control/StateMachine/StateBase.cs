using Control;
using UnityEngine;

public class StateBase<T> where T : class
{
    private StateAction<T> _currentState; 
    private readonly T _context;
    public StateBase(T context)
    {
        _context = context;
    }
    
    public void ChangeState(StateAction<T>  newStateAction)
    {
        if (_currentState == newStateAction)
        {
            _currentState?.ReEnter(_context);
            return;
        }
        _currentState?.OnExit(_context);
        _currentState = newStateAction;
        _currentState.OnEnter(_context);
    }
    public void Update()
    {
        _currentState?.OnUpdate(_context);
    }
    public void FixedUpdate()
    {
        _currentState?.OnFixedUpdate(_context);
    }
}