using UnityEngine;
using System.Collections;

// This is a simple way to use vibration on the Vuzix Balde.
// Here's an example of how to use it.
/*
    ...
    private static void _onPlayerHit(){
        // Vibrate headset for 800ms. 
        Vibration.Vibrate(800);
    }
    ...

*/

public static class Vibration {

    #if UNITY_ANDROID && !UNITY_EDITOR
        public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"); 
        public static AndroidJavaClass vibratorClass = new AndroidJavaClass("android.os.Vibrator");
        public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService","vibrator");
    #else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
    #endif

    public static void Vibrate() {
        if (isAndroid()){
            vibrator.Call("vibrate");
        } else{
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long milliseconds) {
        if (isAndroid()){
            vibrator.Call("vibrate", (long) milliseconds);
        } else{
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long[] pattern, int repeat) {
        if (isAndroid()){
            vibrator.Call("vibrate", pattern, repeat);
        }else{
            Handheld.Vibrate();
        }
    }

    public static bool HasVibrator() {
        return isAndroid();
    }

    public static void Cancel() {
        if (isAndroid()){
            vibrator.Call("cancel");
        }
    }

    private static bool isAndroid() {
        #if UNITY_ANDROID && !UNITY_EDITOR
	        return true;
        #else
            return false;
        #endif
    }
}