using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static UnityEngine.UI.Image;

public class SH_PlayerSkill : MonoBehaviourPun
{

    public GameObject player_ui;
    public GameObject enemy_ui;
    public bool isNearPropMachine = false;

    SH_PlayerFSM fsm;
    Transform cam;

    GameObject originalBody;
    public GameObject mimicBody;
    MeshFilter mybMeshFilter;
    MeshRenderer myMeshRenderer;
    MeshCollider myMeshCollider;

    void Start()
    {
        //if(photonView.IsMine)
        //{
        //    enemy_ui = GameObject.Find("EnemyMachineGage");
        //    enemy_ui.SetActive(false);
        //}

        originalBody = GetComponent<Transform>().Find("Body").GetChild(1).gameObject;
        originalBody.SetActive(true);

        fsm = GetComponent<SH_PlayerFSM>();
        cam = Camera.main.transform;

        mybMeshFilter = mimicBody.GetComponent<MeshFilter>();
        myMeshRenderer = mimicBody.GetComponent<MeshRenderer>();
        myMeshCollider = mimicBody.GetComponent<MeshCollider>();
        mimicBody.SetActive(false);

        player_ui = GameObject.Find("PlayerMachineGage");

        player_ui.SetActive(false);
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            if (isNearPropMachine)
            {
                player_ui.SetActive(true);
            }
            else
            {
                player_ui.SetActive(false);
            }
        }
    }

    public void SkillOffMimic()
    {
        photonView.RPC("RpcSkillOffMimic", RpcTarget.All);
    }

    [PunRPC]
    public void RpcSkillOffMimic()
    {
        if (Input.GetMouseButton(1))
        {
            if (photonView.IsMine == false) return;

            originalBody.SetActive(true);
            mimicBody.SetActive(false);
            fsm.RpcOnChangeState(SH_PlayerFSM.State.Normal);
        }
    }

    public void SkillOnMimic(Vector3 origin, Vector3 dir)
    {
        photonView.RPC("RpcSkillOnMimic", RpcTarget.All, origin, dir);
    }

    Ray cameraRay;
    RaycastHit hit;

    [PunRPC]
    public void RpcSkillOnMimic(Vector3 origin, Vector3 dir)
    {
        cameraRay = new Ray(origin, dir);
        Debug.DrawLine(cameraRay.origin, cameraRay.direction * 50, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            if (photonView.IsMine == false) return;

            if (Physics.Raycast(cameraRay, out hit, 50))
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
    }

    float rescueTime = 0;
    const float RESCUESUCCESSTIME = 2;
    public void Rescue()
    {
        if (Physics.Raycast(cameraRay, out hit))
        {
            if (hit.distance < 15)
            {
                SH_PlayerFSM hitFSM = hit.transform.GetComponent<SH_PlayerFSM>();
                SH_PlayerHP hitHP = hit.transform.GetComponent<SH_PlayerHP>();
                if (hitFSM.state == SH_PlayerFSM.State.Seated)
                {
                    //rescueUI.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        rescueTime += Time.deltaTime;
                        // 슬라이더 UI 올라가고
                        if (rescueTime > RESCUESUCCESSTIME)
                        {
                            // 슬라이더 UI 없어지고
                            hitFSM.ChangeState(SH_PlayerFSM.State.Normal);
                            hitHP.OnHealed(20);
                        }
                    }

                }
            }
        }
    }


}
