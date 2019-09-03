using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateController : AI
{
    [SerializeField]
    // 0 : 먹이주기, 공놀이 상태일 경우 기본 상태로? , 1 : 기쁨 , 2 : 슬픔 , 3 : 놀람 , 4 : 인사 
    Texture[] veluga_SFace = new Texture[4];               // 각 애니메이션 재생 시 사용할 텍스쳐들

    [SerializeField]
    Renderer veluga_SRenderer;                             // 벨루가 표정 렌더러

    void Update()
    {
        //// 기본 상태가 아닐 경우 이동과 감정 상태는 작동하지 않는다.
        //if(veluga_State != Chara_State.state_Idle)
        //{
        //    veluga_MoveState = Chara_Move.notWork;
        //    veluga_Emotion = Chara_Emotion.emotion_NotWork;
        //}
    }


}
