using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

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
    SH_PlayerHP ph;
    SH_PlayerSkill ps;

    Transform chair;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        pm = GetComponent<SH_PlayerMove>();
        pr = GetComponent<SH_PlayerRot>();
        ph = GetComponent<SH_PlayerHP>();  
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
                Groggy();
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
                Transform player = body.GetComponentInParent<Transform>();
                player.localEulerAngles = new Vector3(0, 270, 0);
                pm.cc.enabled = true;
                anim.SetTrigger("Seated");
                break;

            case State.Die:
                player = body.GetComponentInParent<Transform>();
                player.localEulerAngles = new Vector3(0, 270, 0);
                //GameManager.instance.userInfo.is_alive = false;
                //GameManager.instance.userInfo.is_escape = false;
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
                ps.originalBody.SetActive(true);
                ps.mimicBody.SetActive(false);
                break;

            case State.Groggy:
                break;

            case State.Catched:
                pm.cc.enabled = true;
                body.localEulerAngles = new Vector3(0, 0, 0);
                //print("end");
                break;

            case State.Seated:
                Transform player = body.GetComponentInParent<Transform>();
                player.localEulerAngles = new Vector3(0, 0, 0);
                pm.cc.enabled = true;
                ph.seatedTime = 0;
                break;

            case State.Die:

                break;
        }
    }

    private void Normal()
    {
        pm.PlayerMovement();
        pr.PlayerRot(SH_PlayerRot.ViewState.FIRST, false);
        ps.SkillOnMimic();
        ps.Rescue();
    }

    private void Transform()
    {
        pm.PlayerMovement();
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD, false);
        ps.SkillOnMimic();
        ps.SkillOffMimic();
    }

    private void Groggy()
    {
        pm.PlayerMovement();
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD, false);
    }

    private void Catched()
    {
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD, true);
    }

    private void Seated()
    {
        pr.PlayerRot(SH_PlayerRot.ViewState.THIRD, true);
        ph.Seated();
    }

    Collider[] colls;
    bool liveCountFlag = true;
    private void Die()
    {
        colls = Physics.OverlapSphere(transform.position, 2);
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].CompareTag("Chair"))
            {
                colls[i].transform.SetParent(transform, true);
                break;
            }
        }

        transform.position += Vector3.down * 0.2f * Time.deltaTime;

        if (transform.position.y < -5)
        {
            transform.position = new Vector3(0, -10, 0);
            transform.GetComponent<SH_PlayerRot>().camPivot.parent = null;
            transform.GetComponent<SH_PlayerRot>().camPivot.gameObject.AddComponent<YJ_DieCam>();
            if (liveCountFlag)
            {
                GameManager.instance.photonView.RPC("RpcliveCount", RpcTarget.All);
                liveCountFlag = false;
            }
        }
    }

    [PunRPC]
    public void RpcPlayerPos(Vector3 pos)//, Vector3 rot)
    {
        transform.position = pos;
        //transform.localEulerAngles = transform.localEulerAngles + new Vector3(100, 0, 180);
    }
}
