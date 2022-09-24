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

    string character;

    public Transform[] spawnPos = new Transform[5];
    Transform myPos;
    public Transform[] userIcon = new Transform[5];
    Transform myIcon;

    GameObject preCharacter;
    GameObject preThumbnail;
    int curPlayer;

    int startNum;

    // Start is called befor the first frame update
    void Start()
    {
        btnCharacter1.onClick.AddListener(OnClickCharacter1);
        btnCharacter2.onClick.AddListener(OnClickCharacter2);
        btnCharacter3.onClick.AddListener(OnClickCharacter3);
        btnCharacter4.onClick.AddListener(OnClickCharacter4);

        GameObject user = GameObject.Find("UserInfo");
        userInfo = user.GetComponent<UserInfo>();

        GameObject users = GameObject.Find("UsersData");
        usersData = users.GetComponent<UsersData>();

        startNum = Random.Range(0, 4);
        character = "Character" + (startNum + 1);   
        curPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

        // 자기 정보를 저장하고 싶다.
        SetUserInfo();
        // 자기 정보를 방장한테 보내주고 싶다.
        PostUserInfo2Master(userInfo.nick_name, userInfo.role, userInfo.character, userInfo.order);

        //========================================================================
        photonView.RPC("RandomNum", RpcTarget.All, startNum);

        //curPlayer = 2;
        //print(curPlayer);

        UserSpawn(curPlayer);
    }

    [PunRPC]
    void RandomNum(int recieveNum)
    {
        if (photonView.IsMine)
            return;
        else
            startNum = recieveNum;
    }

    void UserSpawn(int p)
    {
        photonView.RPC("RpcUserSpawn", RpcTarget.MasterClient, p, startNum);
    }

    [PunRPC]
    void RpcUserSpawn(int p, int startCharacterNum)
    {
        if (userInfo.role == "Player")
        {
            myPos = spawnPos[p];
            myIcon = userIcon[p];

            preCharacter = myPos.GetChild(startCharacterNum).gameObject;
            preThumbnail = myIcon.GetChild(startCharacterNum).gameObject;

            preCharacter.SetActive(true);
            preThumbnail.SetActive(true);
        }
        else if (userInfo.role == "Killer")
        {
            myPos = spawnPos[0];
            myIcon = userIcon[0];
            myPos.GetChild(0).gameObject.SetActive(true);
            myIcon.GetChild(0).gameObject.SetActive(true);
        }

        myIcon.Find("Text (Legacy)").GetComponent<Text>().text = userInfo.nick_name;

        character = "Character" + p;
    }

    private void PostUserInfo2Master(string nickName, string role, string character, int order)
    {
        photonView.RPC("RpcPostUserInfo2Master", RpcTarget.MasterClient, nickName, role, character, order);
    }

    UserInfo tempInfo = new UserInfo();

    [PunRPC]
    void RpcPostUserInfo2Master(string nickName, string role, string character, int order)
    {
        print("userInfo.order : " + userInfo.order + ", order : " + order);
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
            for (int i = 1; i <= order; i++)
            {
                UserInfo sendInfo = new UserInfo();

                sendInfo.nick_name = usersData.users[i].nick_name;
                sendInfo.role = usersData.users[i].role;
                sendInfo.character = usersData.users[i].character;
                sendInfo.order = usersData.users[i].order;
                print(order + " 반복문 " + i);
                SetUsersData(sendInfo);
            }
        }
    }

    // ==========================<방장만 호출하는 함수>===================================
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
        Debug();

        if (Input.GetKeyUp(KeyCode.Alpha0))
            GameStart();
    }

    void GameStart()
    {
        SetUserInfo();
        //PostUserInfo2Master();

        PhotonNetwork.LoadLevel("GameScene");
    }

    void SetUserInfo()
    {
        userInfo.character = character;
        userInfo.order = curPlayer;
    }

    public void OnClickCharacter(int p)
    {
        preCharacter.SetActive(false);
        preThumbnail.SetActive(false);

        preCharacter = myPos.GetChild(p).gameObject;
        preThumbnail = myIcon.GetChild(p).gameObject;

        preCharacter.SetActive(true);
        preThumbnail.SetActive(true);

        character = "Character" + (p + 1);
    }

    public void OnClickCharacter1()
    {
        OnClickCharacter(0);
    }

    public void OnClickCharacter2()
    {
        OnClickCharacter(1);
    }

    public void OnClickCharacter3()
    {
        OnClickCharacter(2);
    }

    public void OnClickCharacter4()
    {
        OnClickCharacter(3);
    }

    void Debug()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print(usersData.users[1].nick_name + "\n" +
                usersData.users[1].role + "\n" +
                usersData.users[1].character + "\n" +
                usersData.users[1].order);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print(usersData.users[2].nick_name + "\n" +
                usersData.users[2].role + "\n" +
                usersData.users[2].character + "\n" +
                usersData.users[2].order);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print(usersData.users[3].nick_name + "\n" +
                usersData.users[3].role + "\n" +
                usersData.users[3].character + "\n" +
                usersData.users[3].order);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            print(usersData.users[4].nick_name + "\n" +
                usersData.users[4].role + "\n" +
                usersData.users[4].character + "\n" +
                usersData.users[4].order);
        }
    }
}
 