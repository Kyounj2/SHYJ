using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerFSM : MonoBehaviour
{
    public enum State
    {
        Normal,
        Transform,
        Damage,
        Groggy,
        Seated,
        Die,
    }
    public State state = State.Normal;
    public State preState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                Normal();
                break;

            case State.Transform:
                Transform();
                break;

            case State.Damage:
                Damage();
                break;

            case State.Groggy:
                Groggy();
                break;

            case State.Seated:
                Die();
                break;

            case State.Die:
                Die();
                break;
        }
    }

    public void ChangeState(State s)
    {
        preState = s;
        EndState(preState);

        if (state == s)
        {
            print("같은 상태 입니다. : " + state);
            return;
        }

        state = s;

        switch (state)
        {
            case State.Normal:
                break;

            case State.Transform:
                break;

            case State.Damage:
                break;

            case State.Groggy:
                break;

            case State.Seated:
                break;

            case State.Die:
                break;
        }
    }

    public void EndState(State s)
    {
        switch (s)
        {
            case State.Normal:
                break;

            case State.Transform:
                break;

            case State.Damage:
                break;

            case State.Groggy:
                break;

            case State.Seated:
                break;

            case State.Die:
                break;
        }
    }

    private void Normal()
    {
        throw new NotImplementedException();
    }

    private void Transform()
    {
        throw new NotImplementedException();
    }

    private void Damage()
    {
        throw new NotImplementedException();
    }

    private void Groggy()
    {
        throw new NotImplementedException();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
