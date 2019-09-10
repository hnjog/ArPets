using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 기능 및 vuzix input 확인
// 

public class UInput : MonoBehaviour
{
    public Text text;

    // UI 상태
    public enum UIState
    {
        Idle,
        Feed,
        Ball
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

    protected virtual void _onVInputEvent(VINPUT_EVENT pEvent)
    {
        switch (pEvent)
        {
            case VINPUT_EVENT.TAP_1FINGER:
                text.text = "postiton : " + transform.position + "rotation : " + transform.rotation;
                break;
            case VINPUT_EVENT.TAP_2FINGER:
                // back or cancel. 
                break;
            case VINPUT_EVENT.SWIPE_FORWARD_1FINGER:
                GameObject f_object = ObjectManager.instance.F_Expert();
                f_object.transform.position = Vector3.zero;
                f_object.transform.rotation = transform.rotation;
                // scroll right through menu. 
                break;
            case VINPUT_EVENT.SWIPE_BACKWARD_1FINGER:
                // scroll left through menu. 
                break;
            case VINPUT_EVENT.SWIPE_UP_1FINGER:
                break;
            case VINPUT_EVENT.SWIPE_DOWN_1FINGER:
                break;
            case VINPUT_EVENT.SWIPE_FORWARD_2FINGER:
                break;
            case VINPUT_EVENT.SWIPE_BACKWARD_2FINGER:
                break;
            case VINPUT_EVENT.HOLD_1FINGER:
                // open a menu! 
                break;
        }
    }
}
