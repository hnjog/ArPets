using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 기능 및 vuzix input 확인
// 이벤트 시스템, 트리거 x
// 일단 그래픽 레이캐스트를 이용해보자.


public class UInput : MonoBehaviour
{
    //public Text text;

    [Header("")]
    public Image iFeed;



    // UI 상태
    public enum UIState
    {
        Idle,
        Feed,
        Ball,
        menu
    }

    public static UIState uIState = UIState.Idle;

    private void Start()
    {
        VInput.onVInputEvent += _onVInputEvent;
    }

    private void Update()
    {
        VInput.Update(Time.unscaledDeltaTime);
    }

    public void feedChanger()
    {

    }

    // UI 상태 별 뷰직스 움직임 제어
    protected virtual void _onVInputEvent(VINPUT_EVENT pEvent)
    {
        // 먹이줄 때
        if (uIState == UIState.Feed)
        {
            switch (pEvent)
            {
                case VINPUT_EVENT.TAP_1FINGER:                          // 손가락 하나 탭
                    //메뉴로
                    break;
                case VINPUT_EVENT.TAP_2FINGER:                          // 손가락 두 개 탭
                    
                    break;
                case VINPUT_EVENT.SWIPE_FORWARD_1FINGER:                // 손가락 하나로 앞으로 스와이프
                    PlayFeed();
                    break;
                case VINPUT_EVENT.SWIPE_BACKWARD_1FINGER:               // 손가락 하나로 뒤로 스와이프

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

                    // open a menu! 
                    break;
            }
        }
        // 공놀이 때
        else if (uIState == UIState.Ball)
        {
            switch (pEvent)
            {
                case VINPUT_EVENT.TAP_1FINGER:                          // 손가락 하나 탭
                    //메뉴로
                    //if(공놀이 시작이 이미 된 경우)
                    // 받아치기로
                    break;
                case VINPUT_EVENT.TAP_2FINGER:                          // 손가락 두 개 탭

                    break;
                case VINPUT_EVENT.SWIPE_FORWARD_1FINGER:                // 손가락 하나로 앞으로 스와이프
                    StartBall();
                    break;
                case VINPUT_EVENT.SWIPE_BACKWARD_1FINGER:               // 손가락 하나로 뒤로 스와이프

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

                    // open a menu! 
                    break;
            }
        }
        // 평상시에
        else if (uIState == UIState.Idle)
        {
            switch (pEvent)
            {
                case VINPUT_EVENT.TAP_1FINGER:                          // 손가락 하나 탭
                    //메뉴로
                    break;
                case VINPUT_EVENT.TAP_2FINGER:                          // 손가락 두 개 탭

                    break;
                case VINPUT_EVENT.SWIPE_FORWARD_1FINGER:                // 손가락 하나로 앞으로 스와이프
                    break;
                case VINPUT_EVENT.SWIPE_BACKWARD_1FINGER:               // 손가락 하나로 뒤로 스와이프

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
                    Application.Quit();                                // 앱 종료
                    // open a menu! 
                    break;
            }
        }
        // 메뉴 중에
        else
        {
            switch (pEvent)
            {
                case VINPUT_EVENT.TAP_1FINGER:                          // 손가락 하나 탭
                    // 메뉴 선택
                    break;
                case VINPUT_EVENT.TAP_2FINGER:                          // 손가락 두 개 탭

                    break;
                case VINPUT_EVENT.SWIPE_FORWARD_1FINGER:                // 손가락 하나로 앞으로 스와이프
                    // 메뉴 오른쪽
                    break;
                case VINPUT_EVENT.SWIPE_BACKWARD_1FINGER:               // 손가락 하나로 뒤로 스와이프
                    // 메뉴 왼쪽
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

                    // open a menu! 
                    break;
            }
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

}
