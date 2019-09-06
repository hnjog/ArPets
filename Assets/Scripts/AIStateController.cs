using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 상태 스크립트
// 공놀이, 먹이 주기 - 오브젝트 풀로? 
// 해당 조건이 끝나면 다시 기본 상태로 한 번 돌아옴.
// UI 특정 메뉴 선택 시 먹이주기, 공놀이 작동

public class AIStateController : AI
{
    [SerializeField]
    // 0 : 먹이주기, 공놀이 상태일 경우 기본 상태로? , 1 : 기쁨 , 2 : 슬픔 , 3 : 놀람 , 4 : 인사 
    Texture[] veluga_SFace = new Texture[4];               // 각 애니메이션 재생 시 사용할 텍스쳐들

    [SerializeField]
    Renderer veluga_SRenderer;                             // 벨루가 표정 렌더러

    [Header("공놀이, 먹이주기 프리팹들")]
    [SerializeField]
    GameObject ball;
    [SerializeField]
    GameObject food;

    // 먹이주기, UI 메뉴에서 사용될 예정
    void Feed()
    {
        veluga_State = Chara_State.state_Eat;
    }

    // 공놀이
    void Ball()
    {
        veluga_State = Chara_State.state_Ball;
    }

    // 상태 변화
    IEnumerator StateChange()
    {

        yield return null;
    }

    // 배고픔 상태 일시 사용될 코루틴
    IEnumerator State_Hungry()
    {
        // 관련 애니메이션
        yield return new WaitForSeconds(3f);
        veluga_State = Chara_State.state_Idle;
    }

    // 기쁨 상태 일시 사용될 코루틴
    IEnumerator State_Happy()
    {
        // 관련 애니메이션
        yield return new WaitForSeconds(3f);
        veluga_State = Chara_State.state_Idle;
    }

    // 슬픔 상태 일시 사용될 코루틴
    IEnumerator State_Sad()
    {
        // 관련 애니메이션
        yield return new WaitForSeconds(3f);
        veluga_State = Chara_State.state_Idle;
    }
}
