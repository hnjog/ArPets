using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 기능 및 vuzix input 확인
// 먹이 주기 상태일 경우, 메뉴에 먹이는 없게 해야 함.
// 타이밍을 맞게 치는 경우의 수정이 필요함.
// 메뉴 열릴 시 타임 스케일 조정

public class UInput : MonoBehaviour
{
    public Text text;

    [Header("메뉴")]
    [SerializeField] GameObject idle_Menu = null;              // 기본 메뉴
    [SerializeField] GameObject play_Menu = null;              // 게임 중 메뉴

    [SerializeField] AIStateController aiState = null;         // aistatecontroller

    Animator mAni;                      // 메뉴 애니메이터
    Animator pmAni;                     // 플레이 중 애니메이터

    [Header(" 메뉴 상태")]
    byte[] menu = new byte[3] { 0, 1, 2 };
    byte menu_State = 1;
    byte pmenu_State = 1;

    //[Header("시간 조절용")]
    //float lt;
    //float dt;
    //float aniTime;

    public static bool isPlayBall = false;     // 공놀이 중인가

    Ball ballScript;

    // UI 상태
    public enum UIState
    {
        Idle,                           // 기본 상태
        Feed,                           // 먹이 상태
        Ball,                           // 공놀이 상태
        menu,                           // 메뉴 상태     (기본 메뉴 -> 먹이, 공놀이)
        play_m                          // 플레이 중 메뉴(돌아가기, 기본 상태로)
    }

    public static UIState uIState = UIState.Idle;
    UIState back;

    private void Start()
    {
        mAni = idle_Menu.GetComponent<Animator>();
        pmAni = play_Menu.GetComponent<Animator>();
        VInput.onVInputEvent += _onVInputEvent;
    }

    private void Update()
    {
        VInput.Update(Time.unscaledDeltaTime);
        text.text = "happy : " + aiState.happiness + " foodPo : " + aiState.foodPoint;
    }

    // 메뉴 바꿔주는 함수
    void menuChange()
    {
        if(uIState == UIState.menu)
        {
            if(idle_Menu.activeSelf == false)
            {
                idle_Menu.SetActive(true);
                play_Menu.SetActive(false);
            }
        }
        else if(uIState == UIState.play_m)
        {
            if(play_Menu.activeSelf == false)
            {
                play_Menu.SetActive(true);
                idle_Menu.SetActive(false);
            }
        }
    }



    // UI 상태 별 뷰직스 움직임 제어
    protected virtual void _onVInputEvent(VINPUT_EVENT pEvent)
    {
        
        switch (pEvent)
        {
            case VINPUT_EVENT.TAP_1FINGER:                          // 손가락 하나 탭
                if (isPlayBall == false)
                {
                    if (uIState == UIState.Idle)
                    {
                        uIState = UIState.menu;

                        mAni.SetTrigger("yu");                          // 메뉴 등장
                        mAni.SetInteger("Index", 1);                        // 초기 값
                    }
                    else if(uIState == UIState.Feed || uIState == UIState.Ball)
                    {
                        back = uIState;                                   // 저장
                        uIState = UIState.play_m;
                        pmAni.SetTrigger("yu");                           // 메뉴 등장
                        pmAni.SetInteger("Index", 1);                        // 초기 값
                    }
                    else if(uIState == UIState.menu)                         // 메뉴 중에
                    {
                        if(menu_State == menu[0])
                        {
                            aiState.UIFeed();
                            mAni.SetTrigger("mu");
                        }
                        else if (menu_State == menu[1])
                        {
                            aiState.UIIdle();
                            mAni.SetTrigger("mu");
                        }
                        else if (menu_State == menu[2])
                        {
                            aiState.UIBall();
                            mAni.SetTrigger("mu");
                        }
                    }
                    else if(uIState == UIState.play_m)
                    {
                        if (pmenu_State == menu[1])
                        {
                            uIState = back;
                            pmAni.SetTrigger("mu");
                        }
                        else if (pmenu_State == menu[2])
                        {

                            aiState.UICancel();                                // 캔슬 스크립트
                            pmAni.SetTrigger("mu");
                        }
                    }

                }
                else
                {
                    ballScript.Rebound();                           // 받아치기
                }
                break;
            case VINPUT_EVENT.TAP_2FINGER:                          // 손가락 두 개 탭
                
                break;
            case VINPUT_EVENT.SWIPE_FORWARD_1FINGER:                // 손가락 하나로 앞으로 스와이프
                if (uIState == UIState.Feed) PlayFeed();
                else if (uIState == UIState.Ball) StartBall();
                else if(uIState == UIState.menu)
                {
                    // 중앙 메뉴일 때
                    if(menu_State == menu[1])
                    {
                        mAni.SetInteger("Index", 2);                        
                        menu_State = menu[2];
                    }
                    else if(menu_State == menu[0])
                    {
                        mAni.SetInteger("Index", 1);
                        menu_State = menu[1];
                    }
                }
                else if(uIState == UIState.play_m)
                {
                    if (pmenu_State == menu[1])
                    {
                        pmAni.SetInteger("Index", 2);
                        pmenu_State = menu[2];
                    }
                }
                break;
            case VINPUT_EVENT.SWIPE_BACKWARD_1FINGER:               // 손가락 하나로 뒤로 스와이프
                if(uIState == UIState.menu)
                {
                    // 중앙 메뉴일 때
                    if (menu_State == menu[1])
                    {
                        mAni.SetInteger("Index", 0);
                        menu_State = menu[0];
                    }
                    else if (menu_State == menu[2])
                    {
                        mAni.SetInteger("Index", 1);
                        menu_State = menu[1];
                    }
                }
                else if(uIState == UIState.play_m)
                {
                    if (pmenu_State == menu[2])
                    {
                        mAni.SetInteger("Index", 1);
                        menu_State = menu[1];
                    }
                }
                break;
            case VINPUT_EVENT.SWIPE_UP_1FINGER:                     // 손가락 하나로 위로 스와이프

                break;
            case VINPUT_EVENT.SWIPE_DOWN_1FINGER:                  // 손가락 하나로 밑으로 스와이프

                break;
            case VINPUT_EVENT.SWIPE_FORWARD_2FINGER:               // 손가락 두 개로 앞으로 스와이프

                break;
            case VINPUT_EVENT.SWIPE_BACKWARD_2FINGER:              // 손가락 두 개로 앞으로 스와이프

                break;
            case VINPUT_EVENT.HOLD_1FINGER:                        // 손가락 하나 대고 있기
                if (uIState == UIState.Idle) Application.Quit();                                // 앱 종료
                break;
        }
    }

    // 공놀이 시작 메서드
    void StartBall()
    {
        // 실패 횟수가 1일때 시작할 수 있게 한다.
        if (AI.fail < 1)
        {
            GameObject b_object = ObjectManager.instance.B_Expert();
            b_object.transform.position = transform.position;
            ballScript = b_object.GetComponent<Ball>();
            isPlayBall = true;
        }
    }

    // 먹이주기 함수, 오브젝트 매니저와 연계됨. 
    void PlayFeed()
    {
        //게임 오브젝트 꺼내오고, 위치 수정
        if (AI.success + AI.fail < 10)
        {
            GameObject f_object = ObjectManager.instance.F_Expert();
            f_object.transform.position = transform.position;
        }
    }

    IEnumerator UIWait()
    {
        yield return new WaitForSecondsRealtime(1f);
    }
}
