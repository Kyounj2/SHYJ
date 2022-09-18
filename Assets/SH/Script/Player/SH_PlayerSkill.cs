using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;

public struct MimicInfo
{
    public GameObject go;
    public MeshFilter mf;
    public MeshRenderer mr;
    public MeshCollider mc;
}

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

    //Dictionary<string, MimicInfo> target = new Dictionary<string, MimicInfo>();
    MimicInfo target;

    public void SkillOnMimic()
    {
        Ray cameraRay = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 10))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                GameObject targetBody = hit.collider.gameObject;

                target.go = targetBody;
                target.mf = targetBody.GetComponent<MeshFilter>();
                target.mr = targetBody.GetComponent<MeshRenderer>();
                target.mc = targetBody.GetComponent<MeshCollider>();

                //targetMF = targetBody.GetComponent<MeshFilter>();
                //targetMR = targetBody.GetComponent<MeshRenderer>();
                //targetMC = targetBody.GetComponent<MeshCollider>();
                photonView.RPC("RpcSkillOnMimic", RpcTarget.All, target);

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
    public void RpcSkillOnMimic(MimicInfo target)
    {
        originalBody.SetActive(false);
        mimicBody.SetActive(true);

        tbMeshFilter.mesh = target.mf.mesh;
        tbMeshRenderer.material = target.mr.material;
        tbMeshCollider.sharedMesh = target.mc.sharedMesh;
        mimicBody.transform.localScale = target.go.transform.localScale;
    }
}
