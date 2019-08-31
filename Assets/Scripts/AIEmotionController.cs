using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 감정 관련 스크립트

public class AIEmotionController : AI
{
	// 벨루가 감정표현에 사용될 머테리얼들
	[SerializeField]
	Material[] emotionMate = new Material[4];

	// 행복도와 만복도에 따른 감정 변화
	// ... 유저가 공놀이 같은 것만 해서 만복도는 낮고, 행복도만 높은 상황은? - 각 상태는 단계적인 영향을 받음
	// 그러므로 화남 - 매우 화남 상태가 아니라면 만복도의 영향을 받지 않고
	// 행복도가 급격히 높아진다고 해서 행복 상태로 바로 가는 것도 아님
	IEnumerator EChange()
	{
		while (true)
		{
			if(happiness >= 70)
			{
				veluga_Emotion = Chara_Emotion.emotion_Happy;
			}
			else if(happiness >= 30)
			{
				veluga_Emotion = Chara_Emotion.emotion_Idle;
			}
			else if(happiness >= 25)
			yield return null;
		}
	}
}
