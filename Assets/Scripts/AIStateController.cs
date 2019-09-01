using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateController : AI
{


    // Update is called once per frame
    void Update()
    {
        // 기본 상태가 아닐 경우 이동과 감정 상태는 작동하지 않는다.
        if(veluga_State != Chara_State.state_Idle)
        {
            veluga_MoveState = Chara_Move.notWork;
            veluga_Emotion = Chara_Emotion.emotion_NotWork;
        }
    }
}
