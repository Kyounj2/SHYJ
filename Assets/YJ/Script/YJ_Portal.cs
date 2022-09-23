using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YJ_Portal : MonoBehaviourPun
{
    // 탈출을 했는지 뭘로 체크할까?

    // int로 구분할경우 전체인원 == 카운트인원이면
    // 모두 탈출상태로 변경
    // 그치만 죽은사람은 들어올 수 없고
    // 시간안에 모두 못들어올 수도 있지

    // 죽었으면 못나오는 사람 수는 EM에서 하는게 나을것같다

    // 그럼 여기서는 뭘해줘야할까?
    // 누가 들어왔는지 그사람의 고유넘버 확인
    // 몇명 들어왔는지 확인? 해야할까?
    // 오 그런건어때 count로 받고 escape에서 curr인원-1 받은걸로 만약 같다면의 상황추가? 음...

    // 탈출인원 체크
    public int escapeCount;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // other의 포톤뷰가 본인것이면
        // userInfo를 g.m 가져와서
        // 그안에 is_escape를 true로 변경

        if (other.gameObject.layer == 29)
        {
            GameManager.instance.userInfo.is_escape = true;
            escapeCount++;
        }

        // 닿았을때 게임오브젝트를 끄고
        // DathCam을 생성하기
    }
}
