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
    Transform cam;

    public GameObject originalBody;
    public GameObject mimicBody;
    MeshFilter mybMeshFilter;
    MeshRenderer myMeshRenderer;
    MeshCollider myMeshCollider;

    public PhotonView view;

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
        cam = Camera.main.transform;

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

        Ray camRay = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        Debug.DrawLine(camRay.origin, camRay.direction * 50, Color.blue);

        if (Physics.Raycast(camRay, out hit, 50))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                if (Input.GetMouseButtonDown(0))
                    photonView.RPC("RpcSkillOnMimic", RpcTarget.All, camRay.origin, camRay.direction);
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

        Ray camRay = new Ray(cam.position, cam.forward);
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

        //// 1. 주변에 콜라이더 중에 Player태그를 가진 애중에 상태가 Seated인놈이 있을때
        //colls = Physics.OverlapSphere(transform.position, 2);
        //for (int i = 0; i < colls.Length; i++)
        //{
        //    if (colls[i].CompareTag("Player"))
        //    {
        //        SH_PlayerFSM otherFSM = colls[i].transform.GetComponent<SH_PlayerFSM>();
        //        SH_PlayerHP otherHP = colls[i].transform.GetComponent<SH_PlayerHP>();
        //        if (otherFSM.state == SH_PlayerFSM.State.Seated)
        //        {
        //            Vector3 dir = otherFSM.transform.position - transform.position;
        //            dir.y = 0;
        //            if (Vector3.Dot(dir, transform.forward) > 0.5)
        //            {
        //                print("어디한번 F를 눌러서 동료를 구출해보셔~~^^");
        //                if (Input.GetKey(KeyCode.F))
        //                {
        //                    rescueTime += Time.deltaTime;
        //                    print(rescueTime);
        //                    // 슬라이더 UI 올라가고
        //                    if (rescueTime > RESCUESUCCESSTIME)
        //                    {
        //                        // 슬라이더 UI 없어지고
        //                        otherFSM.ChangeState(SH_PlayerFSM.State.Normal);
        //                        otherHP.OnHealed(20);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        //if (Physics.Raycast(cameraRay, out hit))
        //{
        //    if (hit.distance < 15)
        //    {
        //        SH_PlayerFSM hitFSM = hit.transform.GetComponent<SH_PlayerFSM>();
        //        SH_PlayerHP hitHP = hit.transform.GetComponent<SH_PlayerHP>();
        //        if (hitFSM.state == SH_PlayerFSM.State.Seated)
        //        {
        //            //rescueUI.SetActive(true);
        //            if (Input.GetKeyDown(KeyCode.F))
        //            {
        //                rescueTime += Time.deltaTime;
        //                print(rescueTime);
        //                // 슬라이더 UI 올라가고
        //                if (rescueTime > RESCUESUCCESSTIME)
        //                {
        //                    // 슬라이더 UI 없어지고
        //                    hitFSM.ChangeState(SH_PlayerFSM.State.Normal);
        //                    hitHP.OnHealed(20);
        //                }
        //            }

        //        }
        //    }
        //}
    }
}
