using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    // ���� (���̸� (0 / 0))
    public Text roomInfo;

    //int map_id

    // Ŭ���� �Ǿ��� �� ȣ��Ǵ� �Լ��� �������ִ� ����
    //public Action<string, int> onClickAction;

    public void SetInfo(string roomName, int curPlayer, byte maxPlayer)
    {
        name = roomName;
        roomInfo.text = roomName + "\t(" + curPlayer + " / " + maxPlayer + ")";
    }

    //public void OnClick()
    //{
    //    if (onClickAction != null)
    //    {
    //        onClickAction(name)
    //    }
    //}
}
