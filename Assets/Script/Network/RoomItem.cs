using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomItem : MonoBehaviour
{
    // 내용 (방이름 (0 / 0))
    public Text roomInfo;

    //int map_id

    // 클릭이 되었을 때 호출되는 함수를 가지고있는 변수
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
