using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 감정 관련 스크립트
// 기본 상태에서만 작동하도록 한다.
// 표정만 관리(텍스쳐 교체)

public class AIEmotionController : AI
{
	// 벨루가 감정표현에 사용될 텍스쳐들
	[SerializeField]
	Texture[] veluga_Face = new Texture[4];               // 0 : 기분 좋음, 1: 보통, 2: 화남, 3: 매우 화남

	[SerializeField]
	Renderer veluga_Renderer = null;                             // 벨루가 표정 렌더러

	//bool isStarting = true;                                         // 시작 중

	private void Start()
	{
		StartCoroutine(UIWait());
		StartCoroutine(EChange());
		StartCoroutine(FChange());
	}

	IEnumerator UIWait()
	{
		yield return new WaitForSecondsRealtime(6f);
		//isStarting = false;
	}

	// 행복도와 만복도에 따른 감정 변화
	IEnumerator EChange()
	{
		while (true)
		{
			if (veluga_State == Chara_State.state_Idle)
			{
				//일반 상태에서 행복도 70이상 일 시 기분 좋음 상태로
				if (happiness >= 70 && veluga_Emotion == Chara_Emotion.emotion_Idle)
				{
					veluga_Emotion = Chara_Emotion.emotion_Happy;
				}
				//화남 상태에서 행복도 30 이상이거나, 기분 좋은 상태에서 행복도 69 이하일시 일반 상태
				else if ((happiness >= 30 && veluga_Emotion == Chara_Emotion.emotion_Angry)
					 || (happiness <= 69 && veluga_Emotion == Chara_Emotion.emotion_Happy))
				{
					veluga_Emotion = Chara_Emotion.emotion_Idle;
				}
				//매우 화남 상태에서 행복도 25이상, 만복도 10 이상이거나, 일반 상태에서 행복도 29 일시 화남 상태
				else if ((happiness >= 25 && foodPoint >= 10 && veluga_Emotion == Chara_Emotion.emotion_VeryAngry)
					 || (happiness <= 29 && veluga_Emotion == Chara_Emotion.emotion_Idle))
				{
					veluga_Emotion = Chara_Emotion.emotion_Angry;
				}
				//화남 상태에서 행복도 24이하, 만복도 9이하시 매우 화남 상태
				else if (happiness <= 24 && foodPoint <= 9 && veluga_Emotion == Chara_Emotion.emotion_Angry)
				{
					veluga_Emotion = Chara_Emotion.emotion_VeryAngry;
				}
			}

			yield return null;
		}
	}

	// 감정에 따른 표정 변화
	IEnumerator FChange()
	{
		while (true)
		{
			// 감정이 작동할 때(즉 기본 상태일때)
			if (veluga_State == Chara_State.state_Idle)
			{
				if (veluga_Emotion == Chara_Emotion.emotion_Happy)
				{
					veluga_Renderer.material.mainTexture = veluga_Face[0];
				}
				else if (veluga_Emotion == Chara_Emotion.emotion_Idle)
				{
					veluga_Renderer.material.mainTexture = veluga_Face[1];
				}
				else if (veluga_Emotion == Chara_Emotion.emotion_Angry)
				{
					veluga_Renderer.material.mainTexture = veluga_Face[2];
				}
				else if (veluga_Emotion == Chara_Emotion.emotion_VeryAngry)
				{
					veluga_Renderer.material.mainTexture = veluga_Face[3];
				}
			}

			yield return null;
		}
	}
}
