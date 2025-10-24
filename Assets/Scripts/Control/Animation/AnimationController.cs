using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private static readonly int IsIdle = Animator.StringToHash("Idle");
    private static readonly int IsHolding = Animator.StringToHash("Holding");
    private static readonly int IsThrow = Animator.StringToHash("Throw");

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Idle()
    {
        AllParametersFalse();
        _animator.SetBool(IsIdle,true);
    }
    public void Holding()
    {
        AllParametersFalse();
        _animator.SetBool(IsHolding,true);
    }

    public void Throw()
    {
        AllParametersFalse();
        _animator.SetBool(IsThrow,true);
    }

  
    private void AllParametersFalse()
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                _animator.SetBool(parameter.name,false);
            }
        } 
    }
}
