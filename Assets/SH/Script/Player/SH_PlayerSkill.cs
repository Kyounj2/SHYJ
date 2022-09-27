using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static UnityEngine.UI.Image;
using static System.Net.WebRequestMethods;
using Unity.Burst.CompilerServices;
using UnityEditor.EventSystems;

public class SH_PlayerSkill : MonoBehaviourPun
{

    public GameObject player_ui;
    public GameObject enemy_ui;
    public bool isNearPropMachine = false;

    SH_PlayerFSM fsm;
    SH_PlayerRot pr;

    public GameObject originalBody;
    public GameObject mimicBody;
    MeshFilter mybMeshFilter;
    MeshRenderer myMeshRenderer;
    MeshCollider myMeshCollider;

    public PhotonView view;
    Outline outline;

    void Start()
    {
        //if(photonView.IsMine)
        //{
        //    enemy_ui = GameObject.Find("EnemyMachineGage");
        //    enemy_ui.SetActive(false);
        //}

        view = GetComponent<PhotonView>();

        originalBody = GetComponent<Transform>().Find("Body").GetChild(0).gameObject;
        originalBody.SetActive(true);

        fsm = GetComponent<SH_PlayerFSM>();

        mybMeshFilter = mimicBody.GetComponent<MeshFilter>();
        myMeshRenderer = mimicBody.GetComponent<MeshRenderer>();
        myMeshCollider = mimicBody.GetComponent<MeshCollider>();
        mimicBody.SetActive(false);

        player_ui = GameObject.Find("PlayerMachineGage");

        //player_ui.SetActive(false);
    }

    void Update()
    {
        //if(photonView.IsMine)
        //{
        //    if (isNearPropMachine)
        //    {
        //        player_ui.SetActive(true);
        //    }
        //    else
        //    {
        //        player_ui.SetActive(false);
        //    }
        //}
    }

    public void SkillOffMimic()
    {
        if (Input.GetMouseButton(1))
        {
            if (photonView.IsMine == false) return;

            photonView.RPC("RpcSkillOffMimic", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RpcSkillOffMimic()
    {
        originalBody.SetActive(true);
        mimicBody.SetActive(false);
        fsm.RpcOnChangeState(SH_PlayerFSM.State.Normal);
    }

    public void SkillOnMimic()
    {
        if (photonView.IsMine == false) return;

        Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        Debug.DrawLine(camRay.origin, camRay.direction * 50, Color.blue);
        
        if (outline != null && outline.enabled)
            outline.enabled = false;

        if (Physics.Raycast(camRay, out hit, 50))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                outline = hit.transform.GetComponent<Outline>();
                outline.enabled = true;

                //outline.OutlineColor = new Color(1f, 0.317f, 0f, 1f);

                if (Input.GetMouseButtonDown(0))
                    photonView.RPC("RpcSkillOnMimic", RpcTarget.All, camRay.origin, camRay.direction);
            }
            else if (outline != null && outline.enabled)
            {
                outline.enabled = false;
            }
        }
    }

    [PunRPC]
    public void RpcSkillOnMimic(Vector3 origin, Vector3 dir)
    {
        Ray camRay = new Ray(origin, dir);
        RaycastHit hit;

        Debug.DrawLine(camRay.origin, camRay.direction * 50, Color.red);

        if (Physics.Raycast(camRay, out hit, 50))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                originalBody.SetActive(false);
                mimicBody.SetActive(true);
                    
                GameObject targetBody = hit.collider.gameObject;

                mybMeshFilter.mesh = targetBody.GetComponent<MeshFilter>().mesh;
                myMeshRenderer.material = targetBody.GetComponent<MeshRenderer>().material;
                myMeshCollider.sharedMesh = targetBody.GetComponent<MeshCollider>().sharedMesh;
                mimicBody.transform.localScale = targetBody.transform.localScale;

                //photonView.RPC("RpcOnChangeState", RpcTarget.All, SH_PlayerFSM.State.Transform);
                fsm.RpcOnChangeState(SH_PlayerFSM.State.Transform);
            }
        }
    }

    float rescueTime = 0;
    const float RESCUESUCCESSTIME = 2;

    public void Rescue()
    {
        if (photonView.IsMine == false) return;

        Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        Debug.DrawLine(camRay.origin, camRay.direction * 50, Color.green);

        if (Physics.Raycast(camRay, out hit, 3))
        {
            print(hit.collider.tag);
            if (hit.collider.CompareTag("Player"))
            {
                SH_PlayerFSM hitFSM = hit.transform.GetComponent<SH_PlayerFSM>();
                SH_PlayerHP hitHP = hit.transform.GetComponent<SH_PlayerHP>();
                if (hitFSM.state == SH_PlayerFSM.State.Seated)
                {
                    print("어디한번 F를 눌러서 동료를 구출해보셔~~^^");
                    if (Input.GetKey(KeyCode.F))
                    {
                        print("눌렀네!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        //photonView.RPC("RpcSkillOnMimic", RpcTarget.All, camRay.origin, camRay.direction);
                    }
                }
            }
        }
    }
}
