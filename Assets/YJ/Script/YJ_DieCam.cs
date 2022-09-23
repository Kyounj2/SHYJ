using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ ������ Ȱ��ȭ�ɰž�

// Ȱ��ȭ�Ǹ� ����Ʈ�� ���� ����ִ� �÷��̾ ������ ��
// ����Ʈ 1�����ִ� �÷��̾�� �����Ÿ� �������� �̵�
// ���콺 �¿찪�� �޾ƿͼ� ������ �� �ְ��ϱ�

// ó���� �޾ƿ� ����ִ»���� �ο��� ���� ����ִ� ����� �ο��� �ٸ����
// ����Ʈ�� �ʱ�ȭ�ϰ�
// �ٽ� �÷��̾���� �ְ�ʹ�
public class YJ_DieCam : MonoBehaviour
{
    // �÷��̾���� ���� ����Ʈ
    [SerializeField]
    GameObject[] playerList = null;
    
    // �÷��̾�Լ� ������ �����Ÿ�
    Vector3 des;

    // ���콺 �¿찪
    float mouseX;

    void Start()
    {
        des = new Vector3(0, 0, -5);

        playerList = GameObject.FindGameObjectsWithTag("Player");

        transform.position = playerList[player].transform.position;
        //transform.LookAt(playerList[player].transform);
        transform.parent = playerList[player].transform;
    }

    // �Ѿ�� �� ���� ����
    //int rPlayer = 0;
    void Update()
    {
        // ī�޶� ȸ��
        CamRot();

        // ���콺 ��ư Ŭ�� �� ���� ĳ���ͷ� �̵�
        if (Input.GetMouseButtonDown(0))
        {
            //rPlayer = player;
            NextPlayer();
        }
    }

    void CamRot()
    {
        float x = Input.GetAxis("Mouse X");

        mouseX += x * 205 * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0);
    }


    int player = 0;

    void NextPlayer()
    {
        player++;
        if (player >= playerList.Length) player = 0;

        transform.position = playerList[player].transform.position;
        transform.parent = playerList[player].transform;
    }
}
