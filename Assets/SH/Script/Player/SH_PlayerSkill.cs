using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;

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

    GameObject targetBody;
    MeshFilter targetMF;
    MeshRenderer targetMR;
    MeshCollider targetMC;

    public void SkillOnMimic()
    {
        Ray cameraRay = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 10))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                targetBody = hit.collider.gameObject;
                targetMF = targetBody.GetComponent<MeshFilter>();
                targetMR = targetBody.GetComponent<MeshRenderer>();
                targetMC = targetBody.GetComponent<MeshCollider>();
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

        tbMeshFilter.mesh = targetMF.mesh;
        tbMeshRenderer.material = targetMR.material;
        tbMeshCollider.sharedMesh = targetMC.sharedMesh;
        mimicBody.transform.localScale = targetBody.transform.localScale;
    }
}
