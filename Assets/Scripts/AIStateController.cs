using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 상태 스크립트
// 공놀이, 먹이 주기 - 오브젝트 풀로? 
// 해당 조건이 끝나면 다시 기본 상태로 한 번 돌아옴.
// UI 특정 메뉴 선택 시 먹이주기, 공놀이 작동
// 만복도, 행복도 관련 작업 중
// 감정오브젝트를 특정 감정 표현시 켜주고 애니메이션을 주기로 함


public class AIStateController : AI
{
	[SerializeField]
	// 0 : 놀람 1 : 인사 2 : 배고픔 3: 기쁨 
	// 4 : 슬픔 5 : 먹이 대기 6 : 먹이 먹었을 때
	// 7 : 공놀이 대기 8 : 공 받았을 때 9 : 먹이 먹었을 때 2
	Texture[] veluga_SFace = new Texture[10];                    // 각 애니메이션 재생 시 사용할 텍스쳐들

	// 벨루가 표정 렌더러
	[SerializeField]
	Renderer veluga_Renderer = null;                             // 벨루가 표정 렌더러

	//[Header("공놀이, 먹이주기관련 변수")]
	bool isDoingBall = false;                                    // 공놀이 중
	bool isDoingFeed = false;                                    // 먹이 먹는 중
	public static bool isAnimating = false;                      // 다른 애니메이션 재생 중인 경우

	[Header("감정표현용 리스트")]
	// 0 : 화남 1: 크게 놀람 2 : 호기심
	// 3 : 행복 4: 인사 5: 배고픔 6 : 슬픔 7: 놀람
	public List<GameObject> l_Emotion = new List<GameObject>();  // 감정표현 프리팹 리스트

	[SerializeField] GameObject swimming = null;                 // 파티클

	[SerializeField] Animator crown_ani;						 // 왕관 움직이게 하는 것

	private void Start()
	{
		//깜짝 놀람 애니메이션 & 인사 하는 것
		
		StartCoroutine(StartEmotion());
		StartCoroutine(StateChange());
	}

	//private void Update()
	//{
	//	if(Input.GetMouseButtonDown(0))
	//	{
	//		StartCoroutine(BallBall());
	//	}
	//}

	// 공놀이 선택
	public void UIBall()
	{
		success = 0;
		fail = 0;
		// 움직이는 중일 시 대기하라
		if (isStillMove)
		{
			StartCoroutine(Wait());
		}
		StartCoroutine(State_Ball());
		veluga_State = Chara_State.state_Ball;
		UInput.uIState = UInput.UIState.Ball;
	}

	// 먹이주기, UI 에서 사용될 예정
	public void UIFeed()
	{
		success = 0;
		fail = 0;
		// 움직이는 중일 시 대기하라
		if (isStillMove)
		{
			StartCoroutine(Wait());
		}
		StartCoroutine(State_Feed());
		veluga_State = Chara_State.state_Eat;
		UInput.uIState = UInput.UIState.Feed;
	}

	public void UIIdle()
	{
		success = 0;
		fail = 0;
		veluga_State = Chara_State.state_Idle;
		UInput.uIState = UInput.UIState.Idle;
	}

	public void UICancel()
	{
		success = 0;
		fail = 0;
		veluga_State = Chara_State.state_Idle;
		UInput.uIState = UInput.UIState.Idle;
		veluga_Ani.SetInteger("BallState", 0);
		veluga_Ani.SetInteger("FeedState", 0);
	}

	IEnumerator StateChange()
	{
		while (true)
		{
			// 움직이는 중일 시 대기하라
			if (isStillMove)
			{
				StartCoroutine(Wait());
				swimming.SetActive(false);
			}
			if (!isAnimating)
			{
				// 공놀이나, 먹이주기 상태, 이동 중이 아닐때!
				if (veluga_State == Chara_State.state_Idle)
				{
					swimming.SetActive(true);
					// 만복도 40이하
					if (foodPoint <= 40)//&& !hungry
					{
						StartCoroutine(State_Hunger());
					}
					else{
						SoundManager.s_Instance.Sound_BelugaNormal();
					}
				}
			}
			yield return new WaitForSeconds(3f);

		}
	}

	IEnumerator StartEmotion()
	{
		l_Emotion[7].SetActive(true);
		veluga_Renderer.material.mainTexture = veluga_SFace[0];
		yield return new WaitForSeconds(3f);
		l_Emotion[7].SetActive(false);
		l_Emotion[4].SetActive(true);
		veluga_Renderer.material.mainTexture = veluga_SFace[1];
		yield return new WaitForSeconds(3f);
		transform.SetParent(null);
		l_Emotion[4].SetActive(false);
		veluga_State = Chara_State.state_Idle;
	}

	// 공놀이 대기 코루틴
	public IEnumerator State_Ball()
	{
		veluga_Renderer.material.mainTexture = veluga_SFace[7];
		veluga_Ani.SetInteger("BallState", 1);
		yield return null;
	}

	// 먹이 대기 코루틴
	public IEnumerator State_Feed()
	{
		veluga_Renderer.material.mainTexture = veluga_SFace[5];
		veluga_Ani.SetInteger("FeedState", 1);
		yield return null;
	}

	// 기쁨 상태 일시 사용될 코루틴
	public IEnumerator State_Happy()
	{
		veluga_State = Chara_State.state_Happy;
		l_Emotion[3].SetActive(true);
		veluga_Renderer.material.mainTexture = veluga_SFace[3];
		veluga_Ani.SetTrigger("Happy");
		yield return new WaitForSeconds(3f);
		isAnimating = false;
		l_Emotion[3].SetActive(false);
		veluga_State = Chara_State.state_Idle;
		SoundManager.s_Instance.Sound_BelugaHappy();
	}

	// 슬픔 상태 일시 사용될 코루틴
	public IEnumerator State_Sad()
	{
		veluga_State = Chara_State.state_Sad;
		l_Emotion[6].SetActive(true);
		veluga_Renderer.material.mainTexture = veluga_SFace[4];
		veluga_Ani.SetTrigger("Sad");
		yield return new WaitForSeconds(3f);
		isAnimating = false;
		l_Emotion[6].SetActive(false);
		veluga_State = Chara_State.state_Idle;
		SoundManager.s_Instance.Sound_BelugaSad();
	}

	// 배고픔 상태일시 사용할 것
	IEnumerator State_Hunger()
	{
		// 30% 확률
		if (Random.value < 0.3)
		{
			swimming.SetActive(false);
			isAnimating = true;
			veluga_State = Chara_State.state_Hungry;
			l_Emotion[5].SetActive(true);
			veluga_Renderer.material.mainTexture = veluga_SFace[2];
			veluga_Ani.SetTrigger("Hungry");
			yield return new WaitForSeconds(3f);
			isAnimating = false;
			l_Emotion[5].SetActive(false);
			veluga_State = Chara_State.state_Idle;
			SoundManager.s_Instance.Sound_EffectHungry();
		}
	}

	// isStillMove 가 true일 때 false 일 때까지 기다리게 하는 코루틴
	IEnumerator Wait()
	{
		yield return new WaitUntil(() => !isStillMove);
	}

	// 먹이 먹을 때 순간의 애니메이션 및 표정
	public IEnumerator FeedFeed()
	{
		if (!isDoingFeed)
		{
			isDoingFeed = true;
			veluga_Renderer.material.mainTexture = veluga_SFace[5];
			veluga_Ani.SetTrigger("FeedFeed");
			SoundManager.s_Instance.Sound_EffectFeed();
			yield return new WaitForSeconds(1f);
			veluga_Renderer.material.mainTexture = veluga_SFace[9];
			yield return new WaitForSeconds(0.3f);
			veluga_Renderer.material.mainTexture = veluga_SFace[6];
			yield return new WaitForSeconds(0.3f);
			veluga_Renderer.material.mainTexture = veluga_SFace[9];
			yield return new WaitForSeconds(0.4f);
			veluga_Renderer.material.mainTexture = veluga_SFace[5];
			isDoingFeed = false;
		}
	}


	// 공을 받는 순간의 애니메이션 및 표정 이후 애니메이션 재생 후 대기 상태로
	public IEnumerator BallBall()
	{
		if (!isDoingBall)
		{
			isDoingBall = true;
			veluga_Renderer.material.mainTexture = veluga_SFace[8];
			veluga_Ani.SetTrigger("BallBall");
			crown_ani.SetTrigger("Crown");
			SoundManager.s_Instance.Sound_EffectBall();
			yield return new WaitForSecondsRealtime(2f);
			veluga_Renderer.material.mainTexture = veluga_SFace[7];
			isDoingBall = false;
		}

	}

}
