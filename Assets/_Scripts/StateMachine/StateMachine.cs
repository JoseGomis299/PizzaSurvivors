using System;
using System.Collections.Generic;

public class StateMachine
{
    private StateNode _currentState;
    private readonly Dictionary<Type, StateNode> _nodes = new();
    private readonly HashSet<ITransition> _anyTransition = new();

    public void Update()
    {
        ITransition transition = GetTransition();
        if (transition != null)
            ChangeState(transition.To);
        
        _currentState.State?.Update();
    }

    private ITransition GetTransition()
    {
        foreach (var transition in _anyTransition)
            if (transition.Condition.Evaluate())
                return transition;

        foreach (var transition in _currentState.Transitions)
            if (transition.Condition.Evaluate())
                return transition;

        return null;
    }

    public void FixedUpdate()
    {
        _currentState.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        _currentState = _nodes[state.GetType()];
        _currentState.State?.Enter();
    }

    private void ChangeState(IState state)
    {
        if(state == _currentState) return;
        
        _currentState.State?.Exit();
        _currentState = _nodes[state.GetType()];
        _currentState.State?.Enter();
    }
    
    public void At(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }
    
    public void Any(IState to, IPredicate condition)
    {
        _anyTransition.Add(new Transition(to, condition));
    }

    private StateNode GetOrAddNode(IState state)
    {
        var node = _nodes[state.GetType()];
        if (node == null)
        {
            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
        }

        return node;
    }


    class StateNode
    {
        public IState  State { get;}
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }
        
        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }

}