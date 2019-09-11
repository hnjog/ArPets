using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 기능 및 vuzix input 확인
// 

public class UInput : MonoBehaviour
{
    public Text text;

    [SerializeField]
    StatusCal cal;

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

    // UI 상태 별 뷰직스 움직임 제어
    protected virtual void _onVInputEvent(VINPUT_EVENT pEvent)
    {
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
                    PlayBall();
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

                    // open a menu! 
                    break;
            }
        }
        else                                                            // 메뉴 중에
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

    // 공놀이 메서드
    void PlayBall()
    {

    }

    // 먹이주기 함수, 오브젝트 매니저와 연계됨. 다만
    // 던진 후에 생성하는 작업이 따로 필요함.
    void PlayFeed()
    {
        //게임 오브젝트 꺼내오고, 위치 수정
        if (AI.success + AI.fail < 10)
        {
            GameObject f_object = ObjectManager.instance.F_Expert();
            f_object.transform.position = Vector3.zero;
        }
        else
        {
            cal.FeedCal();
        }
    }

}
