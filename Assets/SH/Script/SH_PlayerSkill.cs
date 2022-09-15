using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerSkill : MonoBehaviour
{
    Transform cam;

    public GameObject originalBody;
    public GameObject mimicBody;
    MeshFilter tbMeshFilter;
    MeshRenderer tbMeshRenderer;
    MeshCollider tbMeshCollider;


    void Start()
    {
        cam = Camera.main.transform;

        mimicBody.SetActive(false);
        tbMeshFilter = mimicBody.GetComponent<MeshFilter>();
        tbMeshRenderer = mimicBody.GetComponent<MeshRenderer>();
        tbMeshCollider = mimicBody.GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Mimic();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            originalBody.SetActive(true);
            mimicBody.SetActive(false);
        }
    }

    void Mimic()
    {
        Ray cameraRay = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, 10))
        {
            if (hit.collider.CompareTag("Transformable"))
            {
                GameObject tb = hit.collider.gameObject;

                originalBody.SetActive(false);
                mimicBody.SetActive(true);

                tbMeshFilter.mesh = tb.GetComponent<MeshFilter>().mesh;
                tbMeshRenderer.material = tb.GetComponent<MeshRenderer>().material;
                tbMeshCollider.sharedMesh = tb.GetComponent<MeshCollider>().sharedMesh;
            }
        }
    }
}
