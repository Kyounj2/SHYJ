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
        // PhotonNetwork.JoinRoom               : 선택한 방에 들어갈때
        // PhotonNetwork.JoinOrCreateRoom       : 방이름을 설정해서 들어가려고 할때, 해당 이름의 방이
        //                                        없다면 그 이름으로 방을 생성 후 입장
        // PhotonNetwork.JoinRandomOrCreateRoom : 랜덤방을 들어가려고 할때, 조건에 맞는 방이 없다면
        //                                        내가 방을 생성 후 입장
        // PhotonNetwork.JoinRandomRoom         : 랜덤한 방 들어가겠다
    }

    // 방 입장이 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("ReadyScene");
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
        foreach(Transform tr in roomListContent)
    }

    private void UpdateRoomCache(List<RoomInfo> roomList)
    {
        
    }

    private void CreateRoomListUI()
    {
        
    }
}
