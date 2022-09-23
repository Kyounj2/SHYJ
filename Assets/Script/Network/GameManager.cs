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

    public void AddPlayer(PhotonView pv)
    {
        playerList.Add(pv);
    }

    public PhotonView GetPlayerPv(int viewID)
    {
        for(int i = 0; i < playerList.Count; i++)
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
        liveCount = PhotonNetwork.CurrentCluster.Length -1;

        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;

        //GameObject user = GameObject.Find("UserInfo");
        //userInfo = user.GetComponent<UserInfo>();

        //GameObject users = GameObject.Find("UsersData");
        //usersData = users.GetComponent<UsersData>();

        // 플레이어를 생성한다.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Killer", transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Killer", transform.position, Quaternion.identity);
        }

        CreatePlayer();
    }

    void CreatePlayer()
    {
        // 플레이어를 생성하고싶다.
        GameObject player = null;

        // 0. 유저의 역할군이 Player라면
        if (userInfo.role == "Player")
        {
            // 1. Player프리팹을 생성하고 싶다.
            player = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
            // 2. 유저가 Character1을 선택했다면
            if (userInfo.character == "Character1")
            {
                // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
                Transform body = player.transform.Find("Body");
                GameObject character = Instantiate(character1Factory, body);
                character.transform.localPosition = Vector3.zero;
                character.transform.localEulerAngles = Vector3.zero;
                character.transform.localScale = Vector3.one * 0.5f;
                character.SetActive(true);
            }
            else if (userInfo.character == "Character2")
            {
                // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
                Transform body = player.transform.Find("Body");
                GameObject character = Instantiate(character2Factory, body);
                character.transform.localPosition = Vector3.up;
                character.transform.localEulerAngles = Vector3.zero;
                character.transform.localScale = Vector3.one;
                character.SetActive(true);
            }
            else if (userInfo.character == "Character3")
            {
                // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
                Transform body = player.transform.Find("Body");
                GameObject character = Instantiate(character3Factory, body);
                character.transform.localPosition = Vector3.up;
                character.transform.localEulerAngles = Vector3.zero;
                character.transform.localScale = Vector3.one;
                character.SetActive(true);
            }
            else if (userInfo.character == "Character4")
            {
                // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
                Transform body = player.transform.Find("Body");
                GameObject character = Instantiate(character4Factory, body);
                character.transform.localPosition = Vector3.up;
                character.transform.localEulerAngles = Vector3.zero;
                character.transform.localScale = Vector3.one;
                character.SetActive(true);
            }
        }
        else if (userInfo.role == "Killer")
        {
            player = PhotonNetwork.Instantiate("Killer", transform.position, Quaternion.identity);
        }

        SetUserInfo(player);
    }

    void SetUserInfo(GameObject player)
    {
        userInfo.playerOBJ = player;
        userInfo.is_alive = true;
    }

    [PunRPC]
    void RpcliveCount()
    {
        liveCount--;
    }
}
