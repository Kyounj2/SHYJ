using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_PlayerSkill : MonoBehaviourPun
{
    SH_PlayerFSM fsm;
    Transform cam;

    public GameObject originalBody;
    public GameObject mimicBody;
    MeshFilter mybMeshFilter;
    MeshRenderer myMeshRenderer;
    MeshCollider myMeshCollider;

    void Start()
    {
        fsm = GetComponent<SH_PlayerFSM>();
        cam = Camera.main.transform;

        mimicBody.SetActive(false);
        mybMeshFilter = mimicBody.GetComponent<MeshFilter>();
        myMeshRenderer = mimicBody.GetComponent<MeshRenderer>();
        myMeshCollider = mimicBody.GetComponent<MeshCollider>();
    }

    void Update()
    {
        
    }

    GameObject targetBody;

    public void SkillOnMimic()
    {
        Ray cameraRay = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 10))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                targetBody = hit.collider.gameObject;

                photonView.RPC("RpcSkillOnMimic", RpcTarget.All);

                fsm.ChangeState(SH_PlayerFSM.State.Transform);
            }
        }
    }

    public void SkillOffMimic()
    {
        photonView.RPC("RpcSkillOffMimic", RpcTarget.All);
        fsm.ChangeState(SH_PlayerFSM.State.Normal);
    }

    [PunRPC]
    public void RpcSkillOffMimic()
    {
        originalBody.SetActive(true);
        mimicBody.SetActive(false);
    }

    [PunRPC]
    public void RpcSkillOnMimic()
    {
        originalBody.SetActive(false);
        mimicBody.SetActive(true);

        mybMeshFilter.mesh = targetBody.GetComponent<MeshFilter>().mesh;
        myMeshRenderer.material = targetBody.GetComponent<MeshRenderer>().material;
        myMeshCollider.sharedMesh = targetBody.GetComponent<MeshCollider>().sharedMesh;
        mimicBody.transform.localScale = targetBody.transform.localScale;
    }
}
