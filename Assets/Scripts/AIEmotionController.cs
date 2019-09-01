using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 감정 관련 스크립트

public class AIEmotionController : AI
{
	// 벨루가 감정표현에 사용될 텍스쳐들
	[SerializeField]
	Texture[] emotionMate = new Texture[4];               // 0 : 기분 좋음, 1: 보통, 2: 화남, 3: 매우 화남

    [SerializeField]
    Renderer velugaRenderer;                                    // 벨루가 표정

    private void Start()
    {
        StartCoroutine(EChange());
    }

    private void Update()
    {

        //velugaRenderer.material.mainTexture = emotionMate[0];
    }

    // 행복도와 만복도에 따른 감정 변화
    IEnumerator EChange()
	{
		while (true)
		{
            //일반 상태에서 행복도 70이상 일 시 기분 좋음 상태로
			if(happiness >= 70 && veluga_Emotion == Chara_Emotion.emotion_Idle)
			{
				veluga_Emotion = Chara_Emotion.emotion_Happy;
			}
            //화남 상태에서 행복도 30 이상이거나, 기분 좋은 상태에서 행복도 69 이하일시 일반 상태
			else if((happiness >= 30 && veluga_Emotion == Chara_Emotion.emotion_Angry)
                 || (happiness <= 69 && veluga_Emotion == Chara_Emotion.emotion_Happy))
			{
				veluga_Emotion = Chara_Emotion.emotion_Idle;
			}
            //매우 화남 상태에서 행복도 25이상, 만복도 10 이상이거나, 일반 상태에서 행복도 29 일시 화남 상태
			else if((happiness >= 25 && foodPoint >= 10 && veluga_Emotion == Chara_Emotion.emotion_VeryAngry)
                 || (happiness <= 29 && veluga_Emotion == Chara_Emotion.emotion_Idle))
            {
                veluga_Emotion = Chara_Emotion.emotion_Angry;
            }
            //화남 상태에서 행복도 24이하, 만복도 9이하시 매우 화남 상태
            else if(happiness <= 24 && foodPoint <= 9 && veluga_Emotion == Chara_Emotion.emotion_Angry)
            {
                veluga_Emotion = Chara_Emotion.emotion_VeryAngry;
            }

			yield return null;
		}
	}
}
