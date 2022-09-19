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

    public GameObject originalBody;
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

    //Dictionary<string, GameObject> mimicInfo = new Dictionary<string, GameObject>();
    //GameObject targetBody;

    //public void SkillOnMimic()
    //{
    //    Ray cameraRay = new Ray(cam.position, cam.forward);
    //    RaycastHit hit;

    //    if (Physics.Raycast(cameraRay, out hit, 10))
    //    {
    //        if (hit.collider.CompareTag("Transformable"))
    //        {
    //            //targetBody = hit.collider.gameObject;
    //            //mimicInfo["target"] = targetBody;

    //            photonView.RPC("RpcSkillOnMimic", RpcTarget.All);

    //            fsm.ChangeState(SH_PlayerFSM.State.Transform);
    //        }
    //    }
    //}

    //[PunRPC]
    //public void RpcSkillOnMimic()
    //{
    //    originalBody.SetActive(false);
    //    mimicBody.SetActive(true);

    //    mybMeshFilter.mesh = targetBody.GetComponent<MeshFilter>().mesh;
    //    myMeshRenderer.material = targetBody.GetComponent<MeshRenderer>().material;
    //    myMeshCollider.sharedMesh = targetBody.GetComponent<MeshCollider>().sharedMesh;
    //    mimicBody.transform.localScale = targetBody.transform.localScale;
    //}

    public void SkillOffMimic()
    {
        photonView.RPC("RpcSkillOffMimic", RpcTarget.All);
        //fsm.ChangeState(SH_PlayerFSM.State.Normal);
    }

    [PunRPC]
    public void RpcSkillOffMimic()
    {
        originalBody.SetActive(true);
        mimicBody.SetActive(false);

        //fsm.RpcOnChangeState(SH_PlayerFSM.State.Normal);
    }

    public void SkillOnMimic(Vector3 origin, Vector3 dir)
    {
        photonView.RPC("RpcSkillOnMimic", RpcTarget.All, origin, dir);
    }

    [PunRPC]
    public void RpcSkillOnMimic(Vector3 origin, Vector3 dir)
    {
        Ray cameraRay = new Ray(origin, dir);
        Debug.DrawLine(cameraRay.origin, cameraRay.direction * 50, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 50))
        {
            //print(hit.transform.name);
            if (hit.collider.CompareTag("Transformable"))
            {
                originalBody.SetActive(false);
                mimicBody.SetActive(true);
                    
                GameObject targetBody = hit.collider.gameObject;

                mybMeshFilter.mesh = targetBody.GetComponent<MeshFilter>().mesh;
                myMeshRenderer.material = targetBody.GetComponent<MeshRenderer>().material;
                myMeshCollider.sharedMesh = targetBody.GetComponent<MeshCollider>().sharedMesh;
                mimicBody.transform.localScale = targetBody.transform.localScale;

                photonView.RPC("RpcOnChangeState", RpcTarget.All, SH_PlayerFSM.State.Transform);
                //fsm.RpcOnChangeState(SH_PlayerFSM.State.Transform);
            }
        }
    }
}
