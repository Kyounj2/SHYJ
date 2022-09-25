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

    public Transform playerSpawnPosition;
    public Transform killerSpawnPosition;

    void Start()
    {
        // 시작할때 서버에 접속한 인원 넣어주기 ( 애너미 제외 )
        liveCount = PhotonNetwork.CurrentCluster.Length -1;

        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;
        
        GameObject user = GameObject.Find("UserInfo");
        userInfo = user.GetComponent<UserInfo>();

        GameObject users = GameObject.Find("UsersData");
        usersData = users.GetComponent<UsersData>();

        CreateAllUser();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            killerSpawnPosition.SetParent(playerSpawnPosition);
        }
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
            userOBJ[1] = PhotonNetwork.Instantiate("Player", playerSpawnPosition.position, Quaternion.identity);
            Transform body = userOBJ[1].transform.Find("Body");
            GameObject model = Instantiate(character1Factory, body);
            model.transform.localPosition = Vector3.zero;
            model.transform.localEulerAngles = Vector3.zero;
            model.transform.localScale = Vector3.one * 0.5f;
            model.SetActive(true);

            //for (int i = 1; i < 2; i++)
            //{
            //    userOBJ[i] = PhotonNetwork.Instantiate("Player", playerSpawnPosition.position, Quaternion.identity);
            //    CreateCharacter(usersData.users[i].character, userOBJ[i]);
            //    SetUserInfo(userOBJ[i], i);
            //}
        }

    }

    void CreateCharacter(string character, GameObject player) // 게임오브젝트 못넘긴다 다른방법 생각하기
    {
        photonView.RPC("RpcCreateCharacter", RpcTarget.All, character, player);
    }

    [PunRPC]
    void RpcCreateCharacter(string character, GameObject player)
    {
        Transform body = player.transform.Find("Body");
        GameObject model;        
        switch (character)
        {
            case "character1":
                model = Instantiate(character1Factory, body);
                model.transform.localPosition = Vector3.zero;
                model.transform.localEulerAngles = Vector3.zero;
                model.transform.localScale = Vector3.one * 0.5f;
                model.SetActive(true);
                break;
            case "character2":
                model = Instantiate(character2Factory, body);
                model.transform.localPosition = Vector3.up;
                model.transform.localEulerAngles = Vector3.zero;
                model.transform.localScale = Vector3.one;
                model.SetActive(true);
                break;
            case "character3":
                model = Instantiate(character3Factory, body);
                model.transform.localPosition = Vector3.up;
                model.transform.localEulerAngles = Vector3.zero;
                model.transform.localScale = Vector3.one;
                model.SetActive(true);
                break;
            case "character4":
                model = Instantiate(character4Factory, body);
                model.transform.localPosition = Vector3.up;
                model.transform.localEulerAngles = Vector3.zero;
                model.transform.localScale = Vector3.one;
                model.SetActive(true);
                break;
        }
    }

    void SetUserInfo(GameObject userOBJ, int order)
    {
        usersData.users[order].playerOBJ = userOBJ;
        usersData.users[order].is_alive = true;
        usersData.users[order].is_escape = false;

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

    //void CreatePlayer()
    //{
    //    // 플레이어를 생성하고싶다.
    //    GameObject player = null;

    //    // 0. 유저의 역할군이 Player라면
    //    if (userInfo.role == "Player")
    //    {
    //        // 1. Player프리팹을 생성하고 싶다.
    //        player = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
    //        // 2. 유저가 Character1을 선택했다면
    //        if (userInfo.character == "Character1")
    //        {
    //            // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
    //            Transform body = player.transform.Find("Body");
    //            GameObject character = Instantiate(character1Factory, body);
    //            character.transform.localPosition = Vector3.zero;
    //            character.transform.localEulerAngles = Vector3.zero;
    //            character.transform.localScale = Vector3.one * 0.5f;
    //            character.SetActive(true);
    //        }
    //        else if (userInfo.character == "Character2")
    //        {
    //            // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
    //            Transform body = player.transform.Find("Body");
    //            GameObject character = Instantiate(character2Factory, body);
    //            character.transform.localPosition = Vector3.up;
    //            character.transform.localEulerAngles = Vector3.zero;
    //            character.transform.localScale = Vector3.one;
    //            character.SetActive(true);
    //        }
    //        else if (userInfo.character == "Character3")
    //        {
    //            // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
    //            Transform body = player.transform.Find("Body");
    //            GameObject character = Instantiate(character3Factory, body);
    //            character.transform.localPosition = Vector3.up;
    //            character.transform.localEulerAngles = Vector3.zero;
    //            character.transform.localScale = Vector3.one;
    //            character.SetActive(true);
    //        }
    //        else if (userInfo.character == "Character4")
    //        {
    //            // 3. Player안에 body의 자식으로 Character1을 생성하고 싶다.
    //            Transform body = player.transform.Find("Body");
    //            GameObject character = Instantiate(character4Factory, body);
    //            character.transform.localPosition = Vector3.up;
    //            character.transform.localEulerAngles = Vector3.zero;
    //            character.transform.localScale = Vector3.one;
    //            character.SetActive(true);
    //        }
    //    }
    //    else if (userInfo.role == "Killer")
    //    {
    //        player = PhotonNetwork.Instantiate("Killer", transform.position, Quaternion.identity);
    //    }

    //    SetUserInfo(player);
    //}
}
