using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 행복도, 만복도 계산
// 각 계산이 끝나면 UI 상태는 기본 상태로

public class StatusCal : AI
{
    private void Awake()
    {
        if(happiness != 50 && foodPoint != 50)
            Load();
    }
    private void Start()
    {
        StartCoroutine(HungerIsComing());
    }

    // 공놀이 및 식사 끝날 때 호출하며 계산한다.
    private void Update()
    {
        if (UInput.uIState == UInput.UIState.Feed)
        {
            if (success + fail >= 10)
            {
                FeedCal();

            }
        }
        else if (UInput.uIState == UInput.UIState.Ball)
        {
            if (fail >= 1)
            {
                BallCal();
            }
        }
    }

    // 먹이주기 계산 완료
    public void FeedCal()
    {
        happiness = happiness + success * 3 - fail * 1;
        foodPoint += foodPoint * 5;
        UInput.uIState = UInput.UIState.Idle;
        Save();
    }

    // 공 행복도 계산
    public void BallCal()
    {
        if(success >= 5)
        {
            happiness += (int)(success / 5);
            foodPoint -= (success + fail);
            UInput.uIState = UInput.UIState.Idle;
            Save();
        }
        else
        {
            happiness--;
            foodPoint -= (success + fail);
            UInput.uIState = UInput.UIState.Idle;
            Save();
        }
    }

    // 안드로이드 앱 끝날 때 저장
    private void OnApplicationQuit()
    {
        Save();
    }

    // 1분에 만복도 10 감소
    IEnumerator HungerIsComing()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(60f);
            foodPoint -= 10;
            Save();
        }
    }

    // 저장 
    public void Save()
    {
        PlayerPrefs.SetInt("Happiness", happiness);
        PlayerPrefs.SetInt("FoodPoint", foodPoint);
        success = fail = 0;
    }

    // 로드
    public void Load()
    {
        happiness = PlayerPrefs.GetInt("Happiness");
        foodPoint = PlayerPrefs.GetInt("FoodPoint");
    }
}
