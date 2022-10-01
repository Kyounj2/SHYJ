using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviourPun
{
    UserInfo userInfo;
    UsersData usersData;

    public Button btnCharacter1;
    public Button btnCharacter2;
    public Button btnCharacter3;
    public Button btnCharacter4;
    public Button btnReady;

    string character;

    public Transform[] spawnPos = new Transform[5];
    Transform myPos;
    public Transform[] userIcon = new Transform[5];
    Transform myIcon;

    GameObject preCharacter;
    GameObject preThumbnail;
    int curPlayer;

    int startNum;

    public GameObject playerView;
    public GameObject killerView;

    // Start is called befor the first frame update
    void Start()
    {
        btnCharacter1.onClick.AddListener(OnClickCharacter1);
        btnCharacter2.onClick.AddListener(OnClickCharacter2);
        btnCharacter3.onClick.AddListener(OnClickCharacter3);
        btnCharacter4.onClick.AddListener(OnClickCharacter4);
        btnReady.onClick.AddListener(OnClickReady);

        GameObject user = GameObject.Find("UserInfo");
        userInfo = user.GetComponent<MyUser>().userInfo;

        GameObject users = GameObject.Find("UsersData");
        usersData = users.GetComponent<UsersData>();

        startNum = Random.Range(0, 4);
        character = "Character" + (startNum + 1);   
        curPlayer = PhotonNetwork.CurrentRoom.PlayerCount - 1;

        if (curPlayer == 0)
        {
            startNum = -1;
            killerView.SetActive(true);
        }
        else
        {
            playerView.SetActive(true);
        }

        switch (startNum)
        {
            case 0:
                preButton = btnCharacter1.gameObject;
                EventChooseCharacter(btnCharacter1);
                break;
            case 1:
                preButton = btnCharacter2.gameObject;
                EventChooseCharacter(btnCharacter2);
                break;
            case 2:
                preButton = btnCharacter3.gameObject;
                EventChooseCharacter(btnCharacter3);
                break;
            case 3:
                preButton = btnCharacter4.gameObject;
                EventChooseCharacter(btnCharacter4);
                break;
        }            

        // 자기 정보를 저장하고 싶다.
        SetUserInfo();
        // 자기 정보를 방장한테 보내주고 싶다.
        PostUserInfo2Master(userInfo.nick_name, userInfo.role, userInfo.character, userInfo.order);

        //========================================================================
    }

    void UserSpawn(int spawnOrder)
    {
        photonView.RPC("RpcUserSpawn", RpcTarget.All, spawnOrder);
    }

    [PunRPC]
    void RpcUserSpawn(int spawnOrder)
    {
        if (spawnOrder != 0)
        {
            myPos = spawnPos[spawnOrder];
            myIcon = userIcon[spawnOrder];

            //print(usersData.users[spawnOrder].character);
            int spawnCharacterNum = int.Parse(usersData.users[spawnOrder].character.Substring(9)) - 1;

            preCharacter = myPos.GetChild(spawnCharacterNum).gameObject;
            preThumbnail = myIcon.GetChild(spawnCharacterNum).gameObject;

            preCharacter.SetActive(true);
            preThumbnail.SetActive(true);
        }
        else
        {
            myPos = spawnPos[0];
            myIcon = userIcon[0];
            myPos.GetChild(0).gameObject.SetActive(true);
            myIcon.GetChild(0).gameObject.SetActive(true);
        }

        myIcon.Find("Text (Legacy)").GetComponent<Text>().text = usersData.users[spawnOrder].nick_name;
    }

    private void PostUserInfo2Master(string nickName, string role, string character, int order)
    {
        photonView.RPC("RpcPostUserInfo2Master", RpcTarget.MasterClient, nickName, role, character, order);
    }

    UserInfo tempInfo = new UserInfo();
    [PunRPC]
    void RpcPostUserInfo2Master(string nickName, string role, string character, int order)
    {
        // MasterClient는 본인의 정보를 등록한다.
        if (userInfo.order == order)
        {
            usersData.users[userInfo.order] = userInfo;
        }
        // 들어온 정보가 내 것이 아니면 받은 정보를 담아서 알맞은 자리에 넣고 싶다.
        else
        {
            tempInfo.nick_name = nickName;
            tempInfo.role = role;
            tempInfo.character = character;
            tempInfo.order = order;

            usersData.users[tempInfo.order] = tempInfo;
        }

        // 1. 내가 방장이라면
        if (photonView.IsMine)
        {
            // 2. 취합한 정보를 다시 뿌려주고 싶다.
            for (int i = 0; i <= order; i++)
            {
                UserInfo sendInfo = new UserInfo();

                sendInfo.nick_name = usersData.users[i].nick_name;
                sendInfo.role = usersData.users[i].role;
                sendInfo.character = usersData.users[i].character;
                sendInfo.order = usersData.users[i].order;
                //print(order + " 반복문 " + i);
                SetUsersData(sendInfo);
                UserSpawn(i);
            }
        }
    }

    // ====================<MasterClient의 정보를 받아 동기화 하는 부분>===================

    void SetUsersData(UserInfo receiveInfo)
    {
        photonView.RPC("RpcSetUsersData", RpcTarget.All, receiveInfo.nick_name,
            receiveInfo.role, receiveInfo.character, receiveInfo.order); 
    }

    [PunRPC]
    void RpcSetUsersData(string nickName, string role, string character, int order)
    {
        UserInfo receiveInfo = new UserInfo();
        //print(order + "들어왔는지 좀 보자");
        receiveInfo.nick_name = nickName;
        receiveInfo.role = role;
        receiveInfo.character = character;
        receiveInfo.order = order;

        print(receiveInfo.nick_name + "\n" +
                receiveInfo.role + "\n" +
                receiveInfo.character + "\n" +
                receiveInfo.order);

        usersData.users[receiveInfo.order] = receiveInfo;
    }
    //==================================================================================

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyUp(KeyCode.Alpha0))
                GameStart();
        }
    }

    void GameStart()
    {
        photonView.RPC("RpcGameStart", RpcTarget.All);
    }

    [PunRPC]
    public void RpcGameStart()
    {
        usersData.dontDestroyUserData = usersData;
        PhotonNetwork.LoadLevel("GameScene");
    }

    void SetUserInfo()
    {
        userInfo.character = character;
        userInfo.order = curPlayer;
    }

    public void CharacterChange(string changeCharacter)
    {
        //CharacterChange(changeCharacter, userInfo.order);
        userInfo.character = changeCharacter;
        photonView.RPC("RpcCharacterChange", RpcTarget.All, changeCharacter, userInfo.order);
        ScreenUpdate(userInfo.order);
    }

    GameObject preButton;
    Vector2 orginSize = new Vector2(350, 350);
    Vector2 clickSize = new Vector2(400, 400);
    Color orginColor = new Color(1f, 1f, 1f, 1f);
    Color clickColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public GameObject outline;

    public void OnClickCharacter1()
    {
        EventChooseCharacter(btnCharacter1);
        CharacterChange("Character1");
    }

    public void OnClickCharacter2()
    {
        EventChooseCharacter(btnCharacter2);
        CharacterChange("Character2");
    }

    public void OnClickCharacter3()
    {
        EventChooseCharacter(btnCharacter3);
        CharacterChange("Character3");
    }

    public void OnClickCharacter4()
    {
        EventChooseCharacter(btnCharacter4);
        CharacterChange("Character4");
    }

    void EventChooseCharacter(Button btn)
    {
        preButton.GetComponent<RectTransform>().sizeDelta = orginSize;
        preButton = btn.gameObject;
        preButton.GetComponent<RectTransform>().sizeDelta = clickSize;
        outline.transform.position = preButton.transform.position;
    }

    //public void CharacterChange(string character, int order)
    //{
    //    userInfo.character = character;
    //    photonView.RPC("RpcCharacterChange", RpcTarget.All, character, order);
    //}

    [PunRPC]
    public void RpcCharacterChange(string character, int order)
    {
        usersData.users[order].character = character;
    }

    public void ScreenUpdate(int changerOrder)
    {
        photonView.RPC("RpcScreenUpdate", RpcTarget.All, changerOrder);
    }

    [PunRPC]
    public void RpcScreenUpdate(int changerOrder)
    {
        // 1. 정보가 바뀐 사람이 플레이어 라면
        if (changerOrder != 0)
        {
            // 정보가 바뀐 플레이어의 정보를 참고 하여 스크린을 업데이트 하고 싶다.
            // 1. 바뀐 정보를 참고하고 싶다.
            myPos = spawnPos[changerOrder];
            myIcon = userIcon[changerOrder];

            // 2. myPos, myIcon의 자식을 모두 끄고 싶다.
            for (int i = 0; i < 4; i++)
            {
                myPos.GetChild(i).gameObject.SetActive(false);
                myIcon.GetChild(i).gameObject.SetActive(false);
            }

            int changeCharacterNum = int.Parse(usersData.users[changerOrder].character.Substring(9)) - 1;

            myPos.GetChild(changeCharacterNum).gameObject.SetActive(true);
            myIcon.GetChild(changeCharacterNum).gameObject.SetActive(true);
        }
        // 2. 정보가 바뀐 사람이 킬러(방장) 이라면
        else
        {
            return;
        }
    }

    public void OnClickReady()
    {
        photonView.RPC("RpcOnClickReady", RpcTarget.All, userInfo.order);
    }

    int readyCount = 0;
    [PunRPC]
    public void RpcOnClickReady(int order)
    {
        curPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        usersData.users[order].is_ready = true;

        for (int i = 0; i < curPlayer; i++)
        {
            if (usersData.users[i].is_ready)
            {
                readyCount++;
            }
        }

        if (readyCount == curPlayer)
            RpcGameStart();
        else
            readyCount = 0;
    }
}
 