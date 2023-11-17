using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kaicita
{
    public abstract class State
    {
        public GameObject obj;

        public State(GameObject obj) { this.obj = obj; }
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();
    }

    public class StateMachine
    {
        private Stack<State> m_states = new();

        public StateMachine(State startingState)
        {
            SetState(startingState);
        }

        public void SetState(State state)
        {
            if (m_states.TryPeek(out var prev))
                prev.OnExit();
            m_states = new(new[] {state});
            m_states.Peek().OnEnter();
        }

        public void PushState(State state)
        {
            if (m_states.TryPeek(out var prev))
                prev.OnExit();
            m_states.Push(state);
            m_states.Peek().OnEnter();
        }

        public void PopState()
        {
            if (m_states.TryPop(out var prev))
                prev.OnExit();
            if (m_states.TryPeek(out var current))
                current.OnEnter();
        }

        public void Update()
        {
            if (m_states.TryPeek(out var current))
                current.OnUpdate();
        }

        #nullable enable
        public State? GetState()
        {
            if (m_states.TryPeek(out var current))
                return current;
            else
                return null;
        }
    }
}