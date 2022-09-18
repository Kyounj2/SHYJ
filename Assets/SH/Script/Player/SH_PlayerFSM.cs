using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_PlayerFSM : MonoBehaviourPun
{
    public enum State
    {
        Normal,
        Transform,
        Groggy,
        Catched,
        Seated,
        Die,
    }
    public State state = State.Normal;
    public State preState;

    public Transform body;
    Animator anim;

    SH_PlayerMove pm;
    SH_PlayerRot pr;
    SH_PlayerSkill ps;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        pm = GetComponent<SH_PlayerMove>();
        pr = GetComponent<SH_PlayerRot>();
        ps = GetComponent<SH_PlayerSkill>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeState(State.Groggy);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeState(State.Catched);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeState(State.Seated);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeState(State.Normal);

        switch (state)
        {
            case State.Normal:
                Normal();
                break;

            case State.Transform:
                Transform();
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
        photonView.RPC("RpcOnChangeState", RpcTarget.All, s);
    }

    [PunRPC]
    public void RpcOnChangeState(State s)
    {
        preState = state;
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
                anim.SetTrigger("Idle");
                break;

            case State.Transform:
                break;

            case State.Groggy:
                anim.SetTrigger("Groggy");
                break;

            case State.Catched:
                pm.cc.enabled = false;
                anim.SetTrigger("Catched");
                break;

            case State.Seated:
                pm.cc.enabled = false;
                anim.SetTrigger("Seated");
                break;

            case State.Die:
                break;
        }
    }

    public void EndState(State s)
    {
        //photonView.RPC("RpcEndState", RpcTarget.All, s);
        switch (s)
        {
            case State.Normal:
                break;

            case State.Transform:
                break;

            case State.Groggy:
                break;

            case State.Catched:
                pm.cc.enabled = true;
                body.localEulerAngles = new Vector3(0, 0, 0);
                break;

            case State.Seated:
                pm.cc.enabled = true;
                break;

            case State.Die:
                break;
        }
    }

    //public void RpcEndState(State s)
    //{
    //    switch (s)
    //    {
    //        case State.Normal:
    //            break;

    //        case State.Transform:
    //            break;

    //        case State.Groggy:
    //            break;

    //        case State.Catched:
    //            pm.cc.enabled = true;
    //            body.localEulerAngles = new Vector3(0, 0, 0);
    //            break;

    //        case State.Seated:
    //            pm.cc.enabled = true;
    //            break;

    //        case State.Die:
    //            break;
    //    }
    //}

    private void Normal()
    {
        pm.PlayerMovement();
        pr.PlayerRot(SH_PlayerRot.ViewState.FIRST);

        if (Input.GetButtonDown("Fire1"))
        {
            ps.SkillOnMimic();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ps.SkillOffMimic();
        }
    }

    private void Transform()
    {
        pm.PlayerMovement();
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD);

        if (Input.GetButtonDown("Fire1"))
        {
            ps.SkillOnMimic();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ps.SkillOffMimic();
        }
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
