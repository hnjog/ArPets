using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 상태 스크립트
// 공놀이, 먹이 주기 - 오브젝트 풀로? 
// 해당 조건이 끝나면 다시 기본 상태로 한 번 돌아옴.
// UI 특정 메뉴 선택 시 먹이주기, 공놀이 작동
// 만복도, 행복도 관련 작업 중


public class AIStateController : AI
{
    [SerializeField]
    // 0 : 먹이주기, 공놀이 상태일 경우 기본 상태로? , 1 : 기쁨 , 2 : 슬픔 , 3 : 놀람 , 4 : 인사 
    Texture[] veluga_SFace = new Texture[4];               // 각 애니메이션 재생 시 사용할 텍스쳐들

    [SerializeField]
    Renderer veluga_SRenderer;                             // 벨루가 표정 렌더러

    [Header("공놀이, 먹이주기관련 변수")]
    bool isDoingBall = false;                              // 공놀이 중
    bool isDoingFeed = false;                              // 먹이 먹는 중


    [Header("행복도 비교용")]
    int checkHappy = 0;                                    // 행복도 비교용(기존의 행복도)
    bool isHappy = false;                                  // 행복함 (기존의 행복도 보다 변화한 행복도가 높음)
    bool isSad = false;                                    // 안 행복함(기존의 행복도 보다 변화한 행복도가 낮음)

    private void Start()
    {
        //깜짝 놀람 애니메이션 & 인사 하는 것
        checkHappy = happiness;
        StartCoroutine(StateChange());
        StartCoroutine(HappyChecker());
    }

    // 공놀이 선택
    public void UIBall()
    {
        veluga_State = Chara_State.state_Ball;
        UInput.uIState = UInput.UIState.Ball;
    }

    // 먹이주기, UI 에서 사용될 예정
    public void UIFeed()
    {
        veluga_State = Chara_State.state_Eat;
        UInput.uIState = UInput.UIState.Feed;
    }

    public void UIIdle()
    {
        veluga_State = Chara_State.state_Idle;
        UInput.uIState = UInput.UIState.Idle;
    }

    public void UICancel()
    {
        success = fail = 0;
        veluga_State = Chara_State.state_Idle;
        UInput.uIState = UInput.UIState.Idle;
    }

    // 상태 변화
    IEnumerator StateChange()
    {
        while(true)
        {
            // 공놀이나, 먹이주기 상태가 아닐때!
            if (!isDoingBall || !isDoingFeed)
            {
                // 공놀이
                //if (veluga_State == Chara_State.state_Ball)
                //{
                //    StartCoroutine(State_Ball());
                //}
                //// 먹이주기
                //else if (veluga_State == Chara_State.state_Eat)
                //{
                //    StartCoroutine(State_Feed());
                //}
                // 행복도 상승
                if (isHappy)
                {
                    StartCoroutine(State_Happy());
                }
                // 행복도 하락
                else if (isSad)
                {
                    StartCoroutine(State_Sad());
                }
                // 만복도 40이하
                else if (foodPoint <= 40)
                {
                    StartCoroutine(State_Hunger());
                }

            }
            yield return null;
        }
    }

    
    IEnumerator State_Ball()
    {
        // 관련 애니메이션
        yield return new WaitForSeconds(3f);
        //veluga_State = Chara_State.state_Idle;
    }

    IEnumerator State_Feed()
    {
        // 관련 애니메이션
        yield return new WaitForSeconds(3f);
        //veluga_State = Chara_State.state_Idle;
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
        isHappy = false;
    }

    // 슬픔 상태 일시 사용될 코루틴
    IEnumerator State_Sad()
    {
        // 관련 애니메이션
        yield return new WaitForSeconds(3f);
        veluga_State = Chara_State.state_Idle;
        isSad = false;
    }

    // 행복도 변화 체크를 위해 사용 
    IEnumerator HappyChecker()
    {
        while(true)
        {
            if (checkHappy < happiness)
            {
                isHappy = true;
                checkHappy = happiness;
            }
            else if (checkHappy > happiness)
            {
                isSad = true;
                checkHappy = happiness;
            }
            yield return null;
        }
    }

    // 배고픔 애니메이션 관련
    IEnumerator State_Hunger()
    {
        // 30% 확률
        if (Random.value <= 0.3)
        {
            //배고픔 애니메이션 실행
            yield return new WaitForSecondsRealtime(3f);
        }
        // 다시 실행할 때 갑자기 꺼지는 일이 없도록
        else
            yield return null;
    }

    
}
