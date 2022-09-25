using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.TextCore.Text;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    [HideInInspector]
    public UserInfo userInfo;
    UsersData usersData;

    public GameObject character1Factory;
    public GameObject character2Factory;
    public GameObject character3Factory;
    public GameObject character4Factory;

    // 현재 살아있는 인원이 몇명인지 판단하기 (애너미 제외)
    public int liveCount = 0;

    // 플레이어 접속 리스트
    List<PhotonView> playerList = new List<PhotonView>();

    public Transform playerSpawnPosition;
    public Transform killerSpawnPosition;

    public void AddPlayer(PhotonView pv)
    {
        playerList.Add(pv);
    }

    public PhotonView GetPlayerPv(int viewID)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].ViewID == viewID)
                return playerList[i];
        }
        return null;
    }

    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = true;

        // 시작할때 서버에 접속한 인원 넣어주기 ( 애너미 제외 )
        //liveCount = PhotonNetwork.CurrentCluster.Length - 1;
        liveCount = PhotonNetwork.CurrentRoom.Players.Count - 1;
        print(liveCount);

        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;

        GameObject user = GameObject.Find("UserInfo");
        userInfo = user.GetComponent<MyUser>().userInfo;

        GameObject users = GameObject.Find("UsersData");
        usersData = users.GetComponent<UsersData>();

        //// 플레이어를 생성한다.
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.Instantiate("Killer", transform.position, Quaternion.identity);
        //}
        //else
        //{
        //    PhotonNetwork.Instantiate("Killer", transform.position, Quaternion.identity);
        //}

        CreateAllUser();
    }

    private void Update()
    {
        print(liveCount);
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            killerSpawnPosition.SetParent(playerSpawnPosition);
        }

        Debug();
    }

    void CreateAllUser()
    {
        GameObject[] userOBJ = new GameObject[5];

        // 1. 방장이면 Killer를 생성하고 싶다.
        if (PhotonNetwork.IsMasterClient)
        {
            userOBJ[0] = PhotonNetwork.Instantiate("Killer", killerSpawnPosition.position, Quaternion.identity);
            SetUserInfo(userOBJ[0], 0);
        }
        // 2. 방장이 아니면 Player를 생성하고 싶다.
        else
        {
            string prefabName = "Player" + userInfo.character.Substring(9);
            userOBJ[userInfo.order] = PhotonNetwork.Instantiate(prefabName, playerSpawnPosition.position, Quaternion.identity);
            //userOBJ[1] = PhotonNetwork.Instantiate("Player", playerSpawnPosition.position, Quaternion.identity);
            SetUserInfo(userOBJ[userInfo.order], userInfo.order);

            //for (int i = 1; i < 2; i++)
            //{
            //    //print(usersData.users[i].character.Substring(9));
            //    //string prefabName = "Player" + usersData.users[i].character.Substring(9);
            //    userOBJ[i] = PhotonNetwork.Instantiate("Player1", playerSpawnPosition.position, Quaternion.identity);
            //    SetUserInfo(userOBJ[i], i);
            //}
        }
    }

    void SetUserInfo(GameObject userOBJ, int order)
    {
        //usersData.users[order].playerOBJ = userOBJ;
        //usersData.users[order].is_alive = true;
        //usersData.users[order].is_escape = false;

        if (order == userInfo.order)
        {
            userInfo.playerOBJ = userOBJ;
            userInfo.is_alive = true;
            userInfo.is_escape = false;
        }
    }

    [PunRPC]
    void RpcliveCount()
    {
        liveCount--;
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
