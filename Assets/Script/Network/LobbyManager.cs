using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // ��ư ���
    public Button btnRoleRandom;
    public Button btnRolePlayer;
    public Button btnRoleEnemy;
    public Button btnCreateRoom;
    public Button btnCreate;
    public Button btnRoomItemFactory;
    
    public InputField inputRoomName;

    public GameObject popUp;

    int role; // player : 1, enemy : 2

    // ���� ������
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    // ����Ʈ Content
    public Transform roomListContent;

    public void OnClickBtnRoleRandom()
    {
        role = Random.Range(1, 3);
        // �ٸ� ��ư �� �������
        // ��ư �� �ٲٱ�
    }

    public void OnClickBtnRolePlayer()
    {
        role = 1;
        // �ٸ� ��ư �� �������
        // ��ư �� �ٲٱ�
    }

    public void OnClickBtnRoleEnemy()
    {
        role = 2;
        // �ٸ� ��ư �� �������
        // ��ư �� �ٲٱ�
    }

    public void OnClickBtnCreateRoom()
    {
        popUp.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        inputRoomName.onValueChanged.AddListener(OnValueChanged);
        inputRoomName.onSubmit.AddListener(OnSubmit);
    }

    public void OnValueChanged(string s)
    {
        // inputRoomName�� �Է��� �Ǹ� ��ư�� Ȱ��ȭ �ϰ� �ʹ�.
        btnCreate.interactable = s.Length > 0;

    }

    public void OnSubmit(string s)
    {
        inputRoomName.text = s;
        // inputRoomName�� �Է��� �Ǹ� ������� �ϰ� �ʹ�.

        if (s.Length > 0)
        {
            OnClickCreate();
        }
        else
        {
            print("�� �̸��� �Է����ּ���.");
            // �˾�(�ð�����)
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickCreate()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 5;
        roomOption.IsVisible = true;

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["role"] = role;
        //hash["mapID"] = 
        roomOption.CustomRoomProperties = hash;
        roomOption.CustomRoomPropertiesForLobby = new string[] { "role" };

        PhotonNetwork.CreateRoom(inputRoomName.text, roomOption, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }

    // �� ������ �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //PhotonNetwork.LoadLevel("ReadyScene");
        PhotonNetwork.LoadLevel("GameScene");
        print("OnJoinedRoom");
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed" + returnCode + ", " + message);
    }

    // �濡 ���� ������ ����Ǹ� ȣ�� �Ǵ� �Լ� (�߰�/����/����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        // �븮��Ʈ UI ��ü ����
        DeleteRoomListUI();
        // �븮��Ʈ ������ ������Ʈ
        UpdateRoomCache(roomList);
        // �븮��Ʈ UI ��ü ����
        CreateRoomListUI();
    }

    private void DeleteRoomListUI()
    {
        foreach (Transform tr in roomListContent)
        {
            Destroy(tr.gameObject);
        }

    }

    private void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // ����, ����
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                // ���࿡ �ش� ���� �����Ȱ��̶��
                if (roomList[i].RemovedFromList)
                {
                    // roomCache���� �ش� ������ ����
                    roomCache.Remove(roomList[i].Name);
                }
                // �׷��� �ʴٸ�
                else
                {
                    // ���� ����
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            // �߰�
            else
            {
                roomCache[roomList[i].Name] = roomList[i];
            }
        }
    }

    public GameObject roomItemfactory;

    private void CreateRoomListUI()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            // ������� �����.
            GameObject go = Instantiate(roomItemfactory, roomListContent);
            // ������� ������ ���� (������ (0/0))
            RoomItem item = go.GetComponent<RoomItem>();
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item.SetInfo(info);

            // roomItem ��ư�� Ŭ���Ǹ� ȣ��Ǵ� �Լ� ���
            item.onClickAction = SetRoomName;
            //item.onClickAction = (room) =>
            //{
            //    inputRoomName.text = room;
            //};

            string desc = (string)info.CustomProperties["desc"];
        }
    }

    void SetRoomName(string room)
    {
        // ���̸� ����
        inputRoomName.text = room;
        JoinRoom();
    }
}
