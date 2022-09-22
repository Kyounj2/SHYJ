using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

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

        curPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        //curPlayer = 2;
        print(curPlayer);

        UserSpawn(curPlayer + 1);
    }

    void UserSpawn(int p)
    {
        if (userInfo.role == "Player")
        {
            myPos = spawnPos[p];
            myIcon = userIcon[p];
            int startNum = Random.Range(0, 4);

            preCharacter = myPos.GetChild(startNum).gameObject;
            preThumbnail = myIcon.GetChild(startNum).gameObject;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
            GameStart();
    }

    void GameStart()
    {
        SetUserInfo();
        SetUsersData();

        PhotonNetwork.LoadLevel("GameScene");
    }

    void SetUserInfo()
    {
        userInfo.character = character;
        userInfo.order = curPlayer;
    }

    void SetUsersData()
    {
        usersData.users[curPlayer] = userInfo;
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
}
 