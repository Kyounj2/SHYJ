using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 버튼 목록
    public Button btnRoleRandom;
    public Button btnRolePlayer;
    public Button btnRoleEnemy;
    public Button btnCreateRoom;
    public Button btnCreate;
    public Button btnRoomItemFactory;
    
    public InputField inputRoomName;

    public GameObject popUp;

    int role; // player : 1, enemy : 2

    // 방의 정보들
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    // 리스트 Content
    public Transform roomListContent;

    public void OnClickBtnRoleRandom()
    {
        role = Random.Range(1, 3);
        // 다른 버튼 색 원래대로
        // 버튼 색 바꾸기
    }

    public void OnClickBtnRolePlayer()
    {
        role = 1;
        // 다른 버튼 색 원래대로
        // 버튼 색 바꾸기
    }

    public void OnClickBtnRoleEnemy()
    {
        role = 2;
        // 다른 버튼 색 원래대로
        // 버튼 색 바꾸기
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
        // inputRoomName이 입력이 되면 버튼을 활성화 하고 싶다.
        btnCreate.interactable = s.Length > 0;

    }

    public void OnSubmit(string s)
    {
        inputRoomName.text = s;
        // inputRoomName이 입력이 되면 방생성을 하고 싶다.

        if (s.Length > 0)
        {
            OnClickCreate();
        }
        else
        {
            print("방 이름을 입력해주세요.");
            // 팝업(시간나면)
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

    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text);
    }

    // 방 입장이 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //PhotonNetwork.LoadLevel("ReadyScene");
        PhotonNetwork.LoadLevel("GameScene");
        print("OnJoinedRoom");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed" + returnCode + ", " + message);
    }

    // 방에 대한 정보가 변경되면 호출 되는 함수 (추가/삭제/수정)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        // 룸리스트 UI 전체 삭제
        DeleteRoomListUI();
        // 룸리스트 정보를 업데이트
        UpdateRoomCache(roomList);
        // 룸리스트 UI 전체 생성
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
            // 수정, 삭제
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                // 만약에 해당 룸이 삭제된것이라면
                if (roomList[i].RemovedFromList)
                {
                    // roomCache에서 해당 정보를 삭제
                    roomCache.Remove(roomList[i].Name);
                }
                // 그렇지 않다면
                else
                {
                    // 정보 수정
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            // 추가
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
            // 룸아이템 만든다.
            GameObject go = Instantiate(roomItemfactory, roomListContent);
            // 룸아이템 정보를 셋팅 (방제목 (0/0))
            RoomItem item = go.GetComponent<RoomItem>();
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item.SetInfo(info);

            // roomItem 버튼이 클릭되면 호출되는 함수 등록
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
        // 룸이름 설정
        inputRoomName.text = room;
        JoinRoom();
    }
}
