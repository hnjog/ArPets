using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 행복도, 만복도 계산

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

    // 먹이주기 계산 완료
    public void FeedCal()
    {
        happiness = happiness + success * 3 - fail * 1;
        foodPoint += foodPoint * 5;
        Save();
    }

    // 공 행복도 계산
    public void BallCal()
    {
        if(success >= 5)
        {
            happiness += (int)(success / 5);
            foodPoint -= (success + fail);
            Save();
        }
        else
        {
            happiness--;
            foodPoint -= (success + fail);
            Save();
        }
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
    }

    // 로드
    public void Load()
    {
        happiness = PlayerPrefs.GetInt("Happiness");
        foodPoint = PlayerPrefs.GetInt("FoodPoint");
    }
}
