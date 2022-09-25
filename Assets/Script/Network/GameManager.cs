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

    // ���� ����ִ� �ο��� ������� �Ǵ��ϱ� (�ֳʹ� ����)
    public int liveCount = 0;

    // �÷��̾� ���� ����Ʈ
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
        // ���Ӿ����� ���������� �Ѿ�� ����ȭ���ֱ� ( ���Ӿ� ��� �ѹ� )
        PhotonNetwork.AutomaticallySyncScene = true;

        // �����Ҷ� ������ ������ �ο� �־��ֱ� ( �ֳʹ� ���� )
        //liveCount = PhotonNetwork.CurrentCluster.Length - 1;
        liveCount = PhotonNetwork.CurrentRoom.Players.Count - 1;
        print(liveCount);

        // OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        // RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;

        GameObject user = GameObject.Find("UserInfo");
        userInfo = user.GetComponent<MyUser>().userInfo;

        GameObject users = GameObject.Find("UsersData");
        usersData = users.GetComponent<UsersData>();

        //// �÷��̾ �����Ѵ�.
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

        // 1. �����̸� Killer�� �����ϰ� �ʹ�.
        if (PhotonNetwork.IsMasterClient)
        {
            userOBJ[0] = PhotonNetwork.Instantiate("Killer", killerSpawnPosition.position, Quaternion.identity);
            SetUserInfo(userOBJ[0], 0);
        }
        // 2. ������ �ƴϸ� Player�� �����ϰ� �ʹ�.
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
