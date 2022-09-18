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
    MeshFilter tbMeshFilter;
    MeshRenderer tbMeshRenderer;
    MeshCollider tbMeshCollider;


    void Start()
    {
        fsm = GetComponent<SH_PlayerFSM>();
        cam = Camera.main.transform;

        mimicBody.SetActive(false);
        tbMeshFilter = mimicBody.GetComponent<MeshFilter>();
        tbMeshRenderer = mimicBody.GetComponent<MeshRenderer>();
        tbMeshCollider = mimicBody.GetComponent<MeshCollider>();
    }

    void Update()
    {
        
    }

    public void SkillOnMimic()
    {
        Ray cameraRay = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 10))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                GameObject tb = hit.collider.gameObject;

                photonView.RPC("RpcSkillOnMimic", RpcTarget.All, tb);

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
    public void RpcSkillOnMimic(GameObject tb)
    {
        originalBody.SetActive(false);
        mimicBody.SetActive(true);

        tbMeshFilter.mesh = tb.GetComponent<MeshFilter>().mesh;
        tbMeshRenderer.material = tb.GetComponent<MeshRenderer>().material;
        tbMeshCollider.sharedMesh = tb.GetComponent<MeshCollider>().sharedMesh;
        mimicBody.transform.localScale = tb.transform.localScale;
    }
}
