using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public InputField inputNickname;
    public Button btnConnect;
    public GameObject monster;
    public Animator anim;
    float posY = 837;

    void Start()
    {
        inputNickname.onValueChanged.AddListener(OnValueChanged);
        inputNickname.onSubmit.AddListener(OnSubmit);

        monster.SetActive(false);
    }

    // ���ڻ��� ����
    public Text enter;

    private void Update()
    {
        if(btnConnect.interactable)
        {
            enter.color = Color.white;
        }
        else
        {
            enter.color = Color.gray;
        }

        if (click)
        {
            posY -= 20f;
            posY = Mathf.Clamp(posY, -64, 837);
            monster.gameObject.transform.position = new Vector3(941, posY, -425);
            animTime += Time.deltaTime;
            if (animTime > 1.3f)
            {
                PhotonNetwork.ConnectUsingSettings();
                animTime = 0;
            }
        }
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

    public float animTime = 0;
    bool click = false;
    bool connectionGo = false;
    // ��ư�� ����
    public void OnClickConnection()
    {
        monster.SetActive(true);
        click = true;
        anim.SetTrigger("Hand");
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
