using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 행복도, 만복도 계산
// 각 계산이 끝나면 UI 상태는 기본 상태로
// 행복도 관련 애니메이션을 이쪽에서 관리하는 것이 좋아보임.

public class StatusCal : AI
{

	[Header("행복도 비교용")]
	int checkHappy;                                    // 행복도 비교용(기존의 행복도)

	[SerializeField] AIStateController aiState = null;
	[SerializeField] UInput uu = null;

	[Header("만복도 UI 표시용")]
	[SerializeField] Image foodUI = null;

	[Header("공놀이 성공,실패 시 넣을 텍스트")]
	[SerializeField] GameObject ballSuccess = null;
	[SerializeField] GameObject ballFail = null;

	[Header("타이밍 메뉴")]
	[SerializeField] GameObject timing_menu = null;

	[Header("왕관")]
	[SerializeField] GameObject crown = null;				// 5회 이상 10회 미만
	[SerializeField] GameObject shine_Crown = null;			// 10회 이상일 시 

	private void Awake()
	{
		Load();
		// 한가지 가능성 - 유저가 막장으로 행복도와 만복도를 0 으로 만들시 껏다 키면 50,50이 됨.
		if (happiness == 0 && foodPoint == 0)
		{
			happiness = 50;
			foodPoint = 50;
		}

	}
	private void Start()
	{
		checkHappy = happiness;
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
			if(success >= 10)
			{
				ShineCrown();
			}
			else if(success >=10)
			{
				CrownOn();
			}

			if (fail >= 1)
			{
				timing_menu.SetActive(false);
				BallCal();
				CrownOff();
			}
		}

		if (foodPoint > 100 || foodPoint < 0 || happiness > 100 || happiness < 0) MMCheck();

		FoodAni();

	}

	// 최소값, 최대값 넘어섰는지를 체크 후, 넘어선 경우 최소값, 최대값으로 맞추어줌.
	void MMCheck()
	{
		if (foodPoint > 100) foodPoint = 100;
		else if (foodPoint < 0) foodPoint = 0;

		if (happiness > 100) happiness = 100;
		else if (happiness < 0) happiness = 0;

	}

	// 먹이주기 계산 완료
	public void FeedCal()
	{
		AIStateController.isAnimating = true;
		uu.fakeFish.SetActive(false);
		aiState.StopCoroutine("FeedFeed");
		checkHappy = happiness;
		happiness = happiness + (success * 3) - (fail * 1);
		foodPoint += success * 5;
		veluga_Ani.SetInteger("FeedState", 0);
		UInput.uIState = UInput.UIState.Idle;

		Save();

		veluga_State = Chara_State.state_Idle;
		// 행복도 상승
		if (checkHappy <= happiness)
		{
			aiState.StartCoroutine(aiState.State_Happy());
		}
		// 행복도 하락
		else if (checkHappy > happiness)
		{
			aiState.StartCoroutine(aiState.State_Sad());
		}

	}


	// 공 행복도 계산
	public void BallCal()
	{
		AIStateController.isAnimating = true;
		uu.fakeBall.SetActive(false);
		aiState.StopCoroutine("BallBall");
		checkHappy = happiness;

		UInput.uIState = UInput.UIState.Idle;
		veluga_Ani.SetInteger("BallState", 0);
		UInput.isPlayBall = false;

		if (success >= 5)
		{
			happiness += (int)(success / 5);
			foodPoint -= (success + fail);
			ballSuccess.SetActive(true);
			SoundManager.s_Instance.Sound_EffectBallSuccess();
			
		}
		else
		{
			happiness--;
			foodPoint -= (success + fail);
			ballFail.SetActive(true);
			SoundManager.s_Instance.Sound_EffectBallFail();
		}

		Save();

		veluga_State = Chara_State.state_Idle;

		// 행복도 상승
		if (checkHappy <= happiness)
		{
			aiState.StartCoroutine(aiState.State_Happy());
		}
		// 행복도 하락
		else if (checkHappy > happiness)
		{
			aiState.StartCoroutine(aiState.State_Sad());
		}

		StartCoroutine(WaitText());

	}


	// 왕관 씌우기, 빛나는 왕관 x
	void CrownOn()
	{
		crown.SetActive(true);
		shine_Crown.SetActive(false);
	}
	
	// 왕관 다 끄기. 실패 시 사용
	void CrownOff()
	{
		crown.SetActive(false);
		shine_Crown.SetActive(false);
	}
	// 빛나는 왕관만 키기
	void ShineCrown()
	{
		crown.SetActive(false);
		shine_Crown.SetActive(true);
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
		success = 0;
		fail = 0;
		PlayerPrefs.SetInt("Happiness", happiness);
		PlayerPrefs.SetInt("FoodPoint", foodPoint);

	}

	// 로드
	public void Load()
	{
		happiness = PlayerPrefs.GetInt("Happiness");
		foodPoint = PlayerPrefs.GetInt("FoodPoint");
	}

	// 만복도 UI가 서서히 차오르고, 내리는 걸 위해 사용
	// 조건문이 매끄럽지 않음. lerp 함수는 speed 만큼
	void FoodAni()
	{
		//if (speed < 0.9f)
		//{
		//    speed += Time.deltaTime;
		//    foodUI.fillAmount = Mathf.Lerp(beforeF * 0.01f, foodPoint * 0.01f, speed);
		//}
		//else
		//{
		foodUI.fillAmount = foodPoint * 0.01f;
		//    beforeF = foodPoint;
		//    speed = 0f;
		//}

	}

	IEnumerator WaitText()
	{
		yield return new WaitForSecondsRealtime(3f);
		ballSuccess.SetActive(false);
		ballFail.SetActive(false);
	}
}
