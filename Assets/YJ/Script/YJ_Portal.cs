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
            GameManager.instance.userInfo.is_escape = true;
            escapeCount++;
        }

        // ������� ���ӿ�����Ʈ�� ����
        // DathCam�� �����ϱ�
    }
}
