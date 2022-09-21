using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviourPun
{
    UserInfo userInfo;

    public Button btnCharacter1;
    public Button btnCharacter2;
    public Button btnCharacter3;
    public Button btnCharacter4;

    string character;

    public Transform[] spawnPos = new Transform[5];

    // Start is called befor the first frame update
    void Start()
    {
        btnCharacter1.onClick.AddListener(OnClickCharacter1);
        btnCharacter2.onClick.AddListener(OnClickCharacter2);
        btnCharacter3.onClick.AddListener(OnClickCharacter3);
        btnCharacter4.onClick.AddListener(OnClickCharacter4);

        GameObject user = GameObject.Find("UserInfo");
        userInfo = user.GetComponent<UserInfo>();

        if (userInfo.role == "Player")
        {
            PhotonNetwork.Instantiate("Player", spawnPos[1].position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Killer", spawnPos[0].position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
            GameStart();
    }

    void GameStart()
    {
        GameObject user = GameObject.Find("UserInfo");
        UserInfo userInfo = user.GetComponent<UserInfo>();

        if (userInfo != null)
        {
            userInfo.character = character;
        }

        PhotonNetwork.LoadLevel("GameScene");
    }

    public void OnClickCharacter1()
    {
        character = "Charater1";
    }
    public void OnClickCharacter2()
    {
        character = "Charater2";
    }
    public void OnClickCharacter3()
    {
        character = "Charater3";
    }
    public void OnClickCharacter4()
    {
        character = "Charater4";
    }
}
 