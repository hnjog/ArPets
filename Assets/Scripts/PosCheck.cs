using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosCheck : MonoBehaviour
{
    public Text text;


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
