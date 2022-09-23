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
                escapeCount++;
                other.GetComponent<YJ_KillerMove>().Campos.parent = null;
                //GameManager.instance.userInfo.is_escape = true;
                //GameObject cam = Instantiate(dathCam);
                //cam.transform.position = Vector3.zero;
                //cam.tag = "MainCamera";
                other.GetComponent<YJ_KillerMove>().Campos.gameObject.AddComponent<YJ_DieCam>();
                
            }
            other.gameObject.SetActive(false);

            //photonView.RPC("RpcActiveFlase", RpcTarget.All, other.GetComponent<PhotonView>().ViewID);
        }

        // DathCam�� �����ϱ�? �ѱ�?

        
    }

    [PunRPC]
    void RpcActiveFlase(int viewID)
    {
        PhotonView pv = GameManager.instance.GetPlayerPv(viewID);
        pv.gameObject.SetActive(false);
    }
}
