using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public InputField inputNickname;
    public Button btnConnect;

    void Start()
    {
        inputNickname.onValueChanged.AddListener(OnValueChanged);
        inputNickname.onSubmit.AddListener(OnSubmit);
    }

    public void OnValueChanged(string s)
    {
        // inputNickname�� �Է��� �Ǹ� ��ư�� Ȱ��ȭ �ϰ� �ʹ�.
        btnConnect.interactable = s.Length > 0;

    }

    public void OnSubmit(string s)
    {
        inputNickname.text = s;
        // inputNickname�� �Է��� �Ǿ��ִٸ� �����ϰ� �ʹ�.

        if (s.Length > 0)
        {
            OnClickConnection();
        }
        else
        {
            print("�г����� �Է����ּ���.");
            // �˾�(�ð�����)
        }
    }

    // ��ư�� ����
    public void OnClickConnection()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ���� ���Ӽ����� �ڵ� ȣ��, �κ� ���� �� ���� �Ұ���
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // OnConnecte()ȣ�� ���Ŀ� �ڵ� ȣ��, �κ� ���� �� ���� ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // �κ� ������ �����ϴϱ� �����ϰ� �ʹ�.
        // �г��� ����
        PhotonNetwork.NickName = inputNickname.text;

        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� ������ �ڵ� ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        PhotonNetwork.LoadLevel("LobbyScene");
    }

}
