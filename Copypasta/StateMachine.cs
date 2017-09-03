using System;
using System.Collections.Generic;
using System.Linq;

namespace Copypasta
{
    public class StateMachine
    {
        private HashSet<string> _registeredStates = new HashSet<string>();
        private Dictionary<Tuple<string, string>, List<Tuple<Action, DateTime>>> _fromStateToState = new Dictionary<Tuple<string, string>, List<Tuple<Action, DateTime>>>();
        private Dictionary<string, List<Tuple<Action, DateTime>>> _fromAnyToState = new Dictionary<string, List<Tuple<Action, DateTime>>>();
        private Dictionary<string, List<Tuple<Action, DateTime>>> _fromStateToAny = new Dictionary<string, List<Tuple<Action, DateTime>>>();
        private string _currentState;

        public void TransitionTo(string state)
        {
            if (String.IsNullOrEmpty(state)) { throw new InvalidStateException("State cannot be null or empty."); }
            if (state.ToLower() == "any") { throw new InvalidStateException($"Invalid state: {state}.  Any is a reserved keyword."); }
            if (!_registeredStates.Contains(state)) { throw new InvalidStateException($"Invalid state: {state}.  States must be registered before being set."); }

            var actionList = new List<Tuple<Action, DateTime>>();

            if (_fromStateToState.TryGetValue(new Tuple<string, string>(_currentState, state), out var action))
            {
                action.ForEach(x => actionList.Add(x));
            }

            if (_fromAnyToState.TryGetValue(state, out action))
            {
                action.ForEach(x => actionList.Add(x));
            }

            if (_fromStateToAny.TryGetValue(_currentState, out action))
            {
                action.ForEach(x => actionList.Add(x));
            }

            actionList
                .OrderBy(x => x.Item2)
                .ToList()
                .ForEach(x => x.Item1.Invoke());

            _currentState = state;
        }

        public void RegisterStates(List<string> states)
        {
            foreach (var state in states)
            {
                RegisterState(state);
            }
        }

        public void RegisterState(string state)
        {
            if (String.IsNullOrEmpty(state)) { throw new InvalidStateException("State cannot be null or empty."); }
            if (state.ToLower() == "any") { throw new InvalidStateException($"Invalid state: {state}.  Any is a reserved keyword."); }
            if (_registeredStates.Contains(state)) { throw new InvalidStateException($"{state} has already been registered."); }

            _registeredStates.Add(state);
        }

        public void RegisterAction(string initialState, string endState, Action action)
        {
            try
            {
                RegisterState(initialState);
            }
            catch (InvalidStateException)
            {
                if (initialState.ToLower() == "any")
                {
                    RegisterState(endState);
                    if (_fromAnyToState.TryGetValue(endState, out var actionList))
                    {
                        actionList.Add(new Tuple<Action, DateTime>(action, DateTime.Now));
                    }
                    else
                    {
                        _fromAnyToState.Add(endState, new List<Tuple<Action, DateTime>>()
                        {
                            new Tuple<Action, DateTime>(action, DateTime.Now)
                        });
                    }
                    return;
                }
                throw;
            }

            try
            {
                RegisterState(endState);
                if (_fromStateToState.TryGetValue(new Tuple<string, string>(initialState, endState), out var actionList))
                {
                    actionList.Add(new Tuple<Action, DateTime>(action, DateTime.Now));
                }
                else
                {
                    _fromStateToState.Add(new Tuple<string, string>(initialState, endState), new List<Tuple<Action, DateTime>>()
                    {
                        new Tuple<Action, DateTime>(action, DateTime.Now)
                    });
                }
            }
            catch (InvalidStateException)
            {
                if (endState.ToLower() == "any")
                {
                    if (_fromStateToAny.TryGetValue(initialState, out var actionList))
                    {
                        actionList.Add(new Tuple<Action, DateTime>(action, DateTime.Now));
                    }
                    else
                    {
                        _fromStateToAny.Add(initialState, new List<Tuple<Action, DateTime>>()
                        {
                            new Tuple<Action, DateTime>(action, DateTime.Now)
                        });
                    }
                    return;
                }
                throw;
            }
        }
    }
}
