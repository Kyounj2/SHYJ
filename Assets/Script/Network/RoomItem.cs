using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    // ���� (���̸� (0 / 0))
    public Text roomInfo;

    // Ŭ���� �Ǿ��� �� ȣ��Ǵ� �Լ��� �������ִ� ����
    public Action<string> onClickAction;

    // ���� �ֱ�
    AudioSource buttonSound;

    private void Start()
    {
        buttonSound = GetComponent<AudioSource>();
    }

    public void SetInfo(string roomName, int curPlayer, byte maxPlayer)
    {
        name = roomName;
        roomInfo.text = roomName + "\t(" + curPlayer + " / " + maxPlayer + ")";
    }

    public void OnClick()
    {
        buttonSound.Play();
        if (onClickAction != null)
        {
            onClickAction(name);
        }
    }
}
