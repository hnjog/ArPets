using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 상태 스크립트
// 공놀이, 먹이 주기 - 오브젝트 풀로? 
// 해당 조건이 끝나면 다시 기본 상태로 한 번 돌아옴.
// UI 특정 메뉴 선택 시 먹이주기, 공놀이 작동
// 만복도, 행복도 관련 작업 중
// 감정표현 오브젝트는 애니메이션이 있는가?(그렇다면 작동이 되는 시점에서 OnEnable을 사용하던가, 애니메이션 재생 후 다시 꺼지게 해야 함)
// 감정표현 오브젝트는 각기의 위치가 다른가?( 생성될 때 벨루가의 위치 기준으로 바꿔주어야 함)
// 이쪽에서 감정표현 오브젝트를 관리하기 더 용이해 보였음.


public class AIStateController : AI
{
    [SerializeField]
    // 0 : 놀람 1 : 인사 2 : 배고픔 3: 기쁨 
    // 4 : 슬픔 5 : 먹이 대기 6 : 먹이 먹었을 때
    // 7 : 공놀이 대기 8 : 공 받았을 때
    Texture[] veluga_SFace = new Texture[9];               // 각 애니메이션 재생 시 사용할 텍스쳐들

    // 벨루가 표정 렌더러
    [SerializeField]
    Renderer veluga_Renderer = null;                             // 벨루가 표정 렌더러

    [Header("공놀이, 먹이주기관련 변수")]
    bool isDoingBall = false;                              // 공놀이 중
    bool isDoingFeed = false;                              // 먹이 먹는 중


    [Header("행복도 비교용")]
    int checkHappy = 0;                                    // 행복도 비교용(기존의 행복도)
    bool isHappy = false;                                  // 행복함 (기존의 행복도 보다 변화한 행복도가 높음)
    bool isSad = false;                                    // 안 행복함(기존의 행복도 보다 변화한 행복도가 낮음)

    [Header("감정표현용 리스트")]
    // 0 : 화남 1: 크게 놀람 2 : 호기심
    // 3 : 행복 4: 인사 5: 배고픔 6 : 슬픔 7: 놀람
    public List<GameObject> l_Emotion = new List<GameObject>();  // 감정표현 프리팹 리스트(게임 오브젝트 등록용)
    List<GameObject> e_List = new List<GameObject>();            // 감정표현 오브젝트 풀링 리스트(오브젝트 관리용)

    private void Start()
    {
        // 감정표현 생성 후, 리스트 등록
        for (int l = 0; l < 8; l++)
        {
            GameObject e_object = Instantiate(l_Emotion[l], Vector3.zero, Quaternion.identity);
            e_List.Add(e_object);
            e_object.transform.SetParent(transform);
            e_object.SetActive(false);
        }

        //깜짝 놀람 애니메이션 & 인사 하는 것
        checkHappy = happiness;
        StartCoroutine(StartEmotion());
        StartCoroutine(StateChange());
        StartCoroutine(HappyChecker());
    }

    // 공놀이 선택
    public void UIBall()
    {
        // 움직이는 중일 시 대기하라
        if (isStillMove)
        {
            StartCoroutine(Wait());
        }
        veluga_State = Chara_State.state_Ball;
        UInput.uIState = UInput.UIState.Ball;
    }

    // 먹이주기, UI 에서 사용될 예정
    public void UIFeed()
    {
        // 움직이는 중일 시 대기하라
        if (isStillMove)
        {
            StartCoroutine(Wait());
        }
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
            // 움직이는 중일 시 대기하라
            if(isStillMove)
            {
                StartCoroutine(Wait());
            }
            // 공놀이나, 먹이주기 상태, 이동 중이 아닐때!
            if (!isDoingBall || !isDoingFeed || !isStillMove)
            {
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
            else if(isDoingBall && !isStillMove)
            {
                // 공놀이 대기 상태 및 표정
                StartCoroutine(State_Ball());
            }
            else if(isDoingFeed && !isStillMove)
            {
                // 먹이 대기 상태 및 표정
                StartCoroutine(State_Feed());
            }

            yield return null;
        }
    }

    IEnumerator StartEmotion()
    {
        veluga_Renderer.material.mainTexture = veluga_SFace[0];
        yield return new WaitForSeconds(3f);
        veluga_Renderer.material.mainTexture = veluga_SFace[1];
        yield return new WaitForSeconds(3f);
        veluga_State = Chara_State.state_Idle;

    }


    IEnumerator State_Ball()
    {
        veluga_Renderer.material.mainTexture = veluga_SFace[7];
        veluga_Ani.SetInteger("BallState", 1);
        yield return new WaitForSeconds(3f);
        //veluga_State = Chara_State.state_Idle;
    }

    IEnumerator State_Feed()
    {
        veluga_Renderer.material.mainTexture = veluga_SFace[5];
        veluga_Ani.SetInteger("FeedState", 1);
        yield return new WaitForSeconds(3f);
        //veluga_State = Chara_State.state_Idle;
    }

    // 기쁨 상태 일시 사용될 코루틴
    IEnumerator State_Happy()
    {
        veluga_Renderer.material.mainTexture = veluga_SFace[3];
        veluga_Ani.SetTrigger("Happy");
        yield return new WaitForSeconds(3f);
        veluga_State = Chara_State.state_Idle;
        isHappy = false;
    }

    // 슬픔 상태 일시 사용될 코루틴
    IEnumerator State_Sad()
    {
        veluga_Renderer.material.mainTexture = veluga_SFace[4];
        veluga_Ani.SetTrigger("Sad");
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

    // 배고픔 상태일시 사용할 것
    IEnumerator State_Hunger()
    {
        // 30% 확률
        if (Random.value <= 0.3)
        {
            veluga_Renderer.material.mainTexture = veluga_SFace[2];
            veluga_Ani.SetTrigger("Hungry");
            yield return new WaitForSeconds(3f);
            veluga_State = Chara_State.state_Idle;
        }
        // 다시 실행할 때 갑자기 꺼지는 일이 없도록
        else
            yield return null;
    }

    // isStillMove 가 true일 때 false 일 때까지 기다리게 하는 코루틴
    IEnumerator Wait()
    {
        yield return new WaitUntil(() => !isStillMove);
    }

    // 먹이 먹을 때 순간의 애니메이션 및 표정
    public IEnumerator FeedFeed()
    {
        veluga_Renderer.material.mainTexture = veluga_SFace[6];
        veluga_Ani.SetInteger("FeedState", 2);
        yield return new WaitForSeconds(3f);
        veluga_Renderer.material.mainTexture = veluga_SFace[5];
        veluga_Ani.SetInteger("FeedState", 1);
    }


    // 공을 받는 순간의 애니메이션 및 표정 이후 애니메이션 재생 후 대기 상태로
    public IEnumerator BallBall()
    {
        veluga_Renderer.material.mainTexture = veluga_SFace[8];
        veluga_Ani.SetInteger("BallState", 2);
        yield return new WaitForSeconds(3f);
        veluga_Renderer.material.mainTexture = veluga_SFace[7];
        veluga_Ani.SetInteger("BallState", 1);
    }

}
