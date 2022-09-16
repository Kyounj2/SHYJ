using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public InputField inputNickname;
    public Button btnConnect;

    void Start()
    {
        inputNickname.onValueChanged.AddListener(OnValueChanged);
        inputNickname.onSubmit.AddListener(OnSubmit);
    }

    public void OnValueChanged(string s)
    {
        // inputNickname이 입력이 되면 버튼을 활성화 하고 싶다.
        btnConnect.interactable = s.Length > 0;

    }

    public void OnSubmit(string s)
    {
        inputNickname.text = s;
        // inputNickname이 입력이 되어있다면 접속하고 싶다.

        if (s.Length > 0)
        {
            OnClickConnection();
        }
        else
        {
            print("닉네임을 입력해주세요.");
            // 팝업(시간나면)
        }
    }

    // 버튼에 연결
    public void OnClickConnection()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버 접속성공시 자동 호출, 로비 생성 및 진입 불가능
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // OnConnecte()호출 이후에 자동 호출, 로비 생성 및 진입 가능
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // 로비에 진입이 가능하니까 진입하고 싶다.
        // 닉네임 설정
        PhotonNetwork.NickName = inputNickname.text;

        PhotonNetwork.JoinLobby();
    }

    // 로비 진입 성공시 자동 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        PhotonNetwork.LoadLevel("LobbyScene");
    }

}
