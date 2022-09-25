using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YJ_Portal : MonoBehaviourPun
{
    // Ż���� �ߴ��� ���� üũ�ұ�?

    // int�� �����Ұ�� ��ü�ο� == ī��Ʈ�ο��̸�
    // ��� Ż����·� ����
    // ��ġ�� ��������� ���� �� ����
    // �ð��ȿ� ��� ������ ���� ����

    // �׾����� �������� ��� ���� EM���� �ϴ°� �����Ͱ���

    // �׷� ���⼭�� ��������ұ�?
    // ���� ���Դ��� �׻���� �����ѹ� Ȯ��
    // ��� ���Դ��� Ȯ��? �ؾ��ұ�?
    // �� �׷��Ǿ count�� �ް� escape���� curr�ο�-1 �����ɷ� ���� ���ٸ��� ��Ȳ�߰�? ��...

    // Ż���ο� üũ
    public int escapeCount;

    // �׾����� �� ī�޶�
    public GameObject dathCam;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // other�� ����䰡 ���ΰ��̸�
        // userInfo�� g.m �����ͼ�
        // �׾ȿ� is_escape�� true�� ����

        if (other.gameObject.layer == 29)
        {
            if (other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                other.GetComponent<SH_PlayerRot>().camPivot.parent = null;
                //GameManager.instance.userInfo.is_escape = true;
                //GameObject cam = Instantiate(dathCam);
                //cam.transform.position = Vector3.zero;
                //cam.tag = "MainCamera";
                other.GetComponent<SH_PlayerRot>().camPivot.gameObject.AddComponent<YJ_DieCam>();
                //photonView.RPC("RpcActiveFalse", RpcTarget.All, other.GetComponent<PhotonView>().ViewID);
                photonView.RPC("RpcEscapeCount", RpcTarget.All);
                
            }
            other.gameObject.SetActive(false);

        }

        // DathCam�� �����ϱ�? �ѱ�?

        
    }

    [PunRPC]
    void RpcEscapeCount()
    {
        escapeCount++;
    }
}
