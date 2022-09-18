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
        switch (state)
        {
            case State.Normal:
                Normal();
                break;

            case State.Transform:
                Transform();
                break;

            case State.Groggy:
                Catched();
                break;

            case State.Catched:
                Catched();
                break;

            case State.Seated:
                Seated();
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
        if (state == s)
        {
            print("같은 상태 입니다. : " + state);
            return;
        }

        preState = state;
        EndState(preState);

        state = s;

        switch (state)
        {
            case State.Normal:
                pm.cc.enabled = true;
                anim.SetTrigger("Idle");
                break;

            case State.Transform:
                break;

            case State.Groggy:
                anim.SetTrigger("Groggy");
                break;

            case State.Catched:
                body.localEulerAngles = new Vector3(100, 0, 180);
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
                ps.SkillOffMimic();
                break;

            case State.Groggy:
                break;

            case State.Catched:
                pm.cc.enabled = true;
                body.localEulerAngles = new Vector3(0, 0, 0);
                print("end");
                break;

            case State.Seated:
                pm.cc.enabled = true;
                break;

            case State.Die:
                break;
        }
    }

    private void Normal()
    {
        pm.PlayerMovement();
        pr.PlayerRot(SH_PlayerRot.ViewState.FIRST, false);
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (photonView.IsMine)
                ps.SkillOnMimic(pr.targetCamPos.position, pr.targetCamPos.forward);
        }
    }

    private void Transform()
    {
        pm.PlayerMovement();
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD, false);
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (photonView.IsMine)
                ps.SkillOnMimic(pr.targetCamPos.position, pr.targetCamPos.forward);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (photonView.IsMine)
                ChangeState(State.Normal);
        }
    }

    private void Catched()
    {
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD, true);
    }

    private void Seated()
    {
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD, true);
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    [PunRPC]
    public void RpcPlayerPos(Vector3 pos)//, Vector3 rot)
    {
        transform.position = pos;
        //transform.localEulerAngles = transform.localEulerAngles + new Vector3(100, 0, 180);
    }
}
