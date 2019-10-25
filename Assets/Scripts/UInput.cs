using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 기능 및 vuzix input 확인
// 먹이 주기 상태일 경우, 메뉴에 먹이는 없게 해야 함.
// 타이밍을 맞게 치는 경우의 수정이 필요함.
// 메뉴 열릴 시 타임 스케일 조정

public class UInput : MonoBehaviour
{
	//public Text text;

	[Header("메뉴")]
	[SerializeField] GameObject idle_Menu = null;              // 기본 메뉴
	[SerializeField] GameObject play_Menu = null;              // 게임 중 메뉴
	[SerializeField] GameObject credit = null;                 // 제작자 크레딧
	[SerializeField] Animator credit_Ani = null;			   // 크레딧 애니메이터(코루틴을 통해 끝나는지 확인)


	[SerializeField] AIStateController aiState = null;         // aistatecontroller

	Animator mAni;                      // 메뉴 애니메이터
	Animator pmAni;                     // 플레이 중 애니메이터

	[SerializeField] public GameObject fakeFish = null;               // 가짜 물고기
	[SerializeField] public GameObject fakeBall = null;               // 가짜 공

	bool isStarting = true;                                         // 시작 중

	[Header("호출용 변수들")]
	//bool call = false;                                              // 호출 사용
	float callTimer = 0f;

	[Header(" 메뉴 상태")]
	byte[] menu = new byte[4] { 0, 1, 2, 3 };
	byte menu_State = 1;                                        // 먹이주기, 돌아가기, 공놀이, 호출
	byte pmenu_State = 1;                                       // 없음 , 돌아가기, 상태 취소, 없음

	public static bool isPlayBall = false;                     // 공놀이 중인가
	bool feedCool = false;

	[Header("공놀이 관련")]
	Ball ballScript;
	[SerializeField] RhythmCheck rhythm = null;                // 리듬 체크 용
															   //public bool once = false;                                // 한번만 체크
	[SerializeField] GameObject timingMenu = null;             // 타이밍 메뉴
	bool oneBall = false;                                      // 공은 하나만

	[SerializeField] GameObject[] filters = new GameObject[6]; // 선택된 것만 보이는 필터들
	[SerializeField] Transform lookCheckerTF = null;              // 보이는 곳 위치

	int fTry = 0;

	// UI 상태
	public enum UIState
	{
		Idle,                                                  // 기본 상태
		Feed,                                                  // 먹이 상태
		Ball,                                                  // 공놀이 상태
		menu,                                                  // 메뉴 상태     (기본 메뉴 -> 먹이, 공놀이)
		play_m,                                                // 플레이 중 메뉴(돌아가기, 기본 상태로)
		credit_m											   // 메뉴 상태에서 아래로 슬라이드 할 경우 나오는 상태, 밑으로 내려가는 애니메이션을 재생,
	}

	public static UIState uIState = UIState.Idle;
	UIState back;                                              // 공놀이, 먹이주기 메뉴 중 취소 시, 돌아가는 형태.

	private void Start()
	{
		lookCheckerTF = GameObject.Find("LookChecker").transform;
		//Debug.Log(lookCheckerTF.name);
		mAni = idle_Menu.GetComponent<Animator>();
		pmAni = play_Menu.GetComponent<Animator>();
		VInput.onVInputEvent += _onVInputEvent;
	}

	private void Update()
	{

		if (isStarting)
		{
			StartCoroutine(UIWait());
		}
		else
		{
			//if(Input.GetMouseButtonDown(0)) rhythm.CheckRhythm();
			VInput.Update(Time.unscaledDeltaTime);
			StateAndUI();
		}

		//if(Input.GetMouseButtonDown(0))
		//{
		//	Debug.Log("a1");
		//	if (uIState != UIState.credit_m)
		//	{
		//		Debug.Log("a2");
		//		Debug.Log(credit.name);
		//		CreditOn();
		//		uIState = UIState.credit_m;
		//		StartCoroutine("CreditChecker");
		//	}
		//	else{
		//		StopCoroutine("CreditChecker");
		//		CreditOff();
		//		//credit_Ani.Rebind();                            // 애니메이터 초기화
		//		uIState = UIState.menu;
		//	}
		//}


	}

	// 메뉴 바꿔주는 함수
	void StateAndUI()
	{
		if (uIState == UIState.menu)
		{
			if (idle_Menu.activeSelf == false)
			{
				idle_Menu.SetActive(true);
				play_Menu.SetActive(false);
			}
		}
		else if (uIState == UIState.play_m)
		{
			if (play_Menu.activeSelf == false)
			{
				play_Menu.SetActive(true);
				idle_Menu.SetActive(false);
			}
		}
		else if (uIState != UIState.Feed)
		{
			fakeFish.SetActive(false);
		}

		if (uIState == UIState.Idle)
		{
			oneBall = false;
		}

		if (menu_State == menu[0])
		{
			filters[0].SetActive(false);
			filters[1].SetActive(true);
			filters[2].SetActive(true);
			filters[5].SetActive(true);
		}
		else if (menu_State == menu[1])
		{
			filters[0].SetActive(true);
			filters[1].SetActive(false);
			filters[2].SetActive(true);
			filters[5].SetActive(true);
		}
		else if (menu_State == menu[2])
		{
			filters[0].SetActive(true);
			filters[1].SetActive(true);
			filters[2].SetActive(false);
			filters[5].SetActive(true);
		}
		else if (menu_State == menu[3])
		{
			filters[0].SetActive(true);
			filters[1].SetActive(true);
			filters[2].SetActive(true);
			filters[5].SetActive(false);
		}

		if (pmenu_State == menu[1])
		{
			filters[3].SetActive(true);
			filters[4].SetActive(false);
		}
		else if (pmenu_State == menu[2])
		{
			filters[3].SetActive(false);
			filters[4].SetActive(true);
		}
	}

	void CreditOn()
	{
		credit.SetActive(true);
	}

	void CreditOff()
	{
		credit.SetActive(false);
	}

	public void TimingOn()
	{
		timingMenu.SetActive(true);
	}

	public void TimingOff()
	{
		timingMenu.SetActive(false);
	}

	// UI 상태 별 뷰직스 움직임 제어
	protected virtual void _onVInputEvent(VINPUT_EVENT pEvent)
	{
		switch (pEvent)
		{
			case VINPUT_EVENT.TAP_1FINGER:                          // 손가락 하나 탭
				if (uIState == UIState.credit_m)                      // 인트로 상태일 경우 터치시 꺼준다.
				{
					StopCoroutine("CreditChecker");
					CreditOff();
					uIState = UIState.menu;
					break;
				}
				if (isPlayBall == false && AIStateController.isAnimating == false)    // 공놀이 중이 아닐때, 애니메이션 재생 중이 아닐 때 메뉴 이용 가능
				{
					// 메뉴를 열 때 타임스케일 = 0
					if (uIState == UIState.Idle)
					{
						Time.timeScale = 0f;
						uIState = UIState.menu;
						mAni.SetTrigger("yu");                          // 메뉴 등장
					}
					else if (uIState == UIState.Feed || uIState == UIState.Ball)
					{
						Time.timeScale = 0f;
						back = uIState;                                   // 저장
						uIState = UIState.play_m;
						pmAni.SetTrigger("yu");                           // 메뉴 등장
					}
					else if (uIState == UIState.menu)                         // 메뉴 중에
					{
						Time.timeScale = 1f;
						if (menu_State == menu[0])
						{
							AI.isStillMove = true;
							aiState.UIFeed();
							fTry = 0;
							mAni.SetTrigger("mu");
							fakeFish.SetActive(true);
						}
						else if (menu_State == menu[1])
						{
							aiState.UIIdle();
							mAni.SetTrigger("mu");
						}
						else if (menu_State == menu[2])
						{
							AI.isStillMove = true;
							aiState.UIBall();
							fakeBall.SetActive(true);
							mAni.SetTrigger("mu");
						}
						else if (menu_State == menu[3])                       // 호출 메뉴
						{
							//lookCheckerTF.localPosition = new Vector3(0, 0, 4.5f);
							lookCheckerTF.SetParent(null);
							AI.isStillMove = true;
							aiState.UIIdle();
							StartCoroutine(CallWait());
							mAni.SetTrigger("mu");
						}
					}
					else if (uIState == UIState.play_m)
					{
						Time.timeScale = 1f;
						if (pmenu_State == menu[1])
						{
							uIState = back;
							pmAni.SetTrigger("mu");
						}
						else if (pmenu_State == menu[2])
						{
							fakeFish.SetActive(false);
							fakeBall.SetActive(false);
							aiState.UICancel();                                // 캔슬 스크립트
							pmAni.SetTrigger("mu");
						}
					}
					SoundManager.s_Instance.Sound_EffectMenuChoose();
				}
				else if (isPlayBall == true)
				{
					rhythm.CheckRhythm();                             // 리듬 체크
				}
				
				break;
			case VINPUT_EVENT.TAP_2FINGER:                          // 손가락 두 개 탭
				break;
			case VINPUT_EVENT.SWIPE_FORWARD_1FINGER:                // 손가락 하나로 앞으로 스와이프
				if (uIState == UIState.Feed)
				{
					aiState.veluga_Ani.SetInteger("FeedState", 1);
					fakeFish.SetActive(false);
					if (fTry < 5 && !feedCool)
					{
						feedCool = true;
						PlayFeed();
						StartCoroutine(FeedCoolCoru());
					}
				}
				else if (uIState == UIState.Ball)
				{
					aiState.veluga_Ani.SetInteger("BallState", 1);
					fakeBall.SetActive(false);
					StartBall();
				}
				else if (uIState == UIState.menu)
				{
					if (menu_State == menu[2])
					{
						mAni.SetInteger("Index", 3);
						menu_State = menu[3];
					}
					// 중앙 메뉴일 때
					else if (menu_State == menu[1])
					{
						mAni.SetInteger("Index", 2);
						menu_State = menu[2];
					}
					else if (menu_State == menu[0])
					{
						mAni.SetInteger("Index", 1);
						menu_State = menu[1];
					}
				}
				else if (uIState == UIState.play_m)
				{
					if (pmenu_State == menu[1])
					{
						pmAni.SetInteger("Index", 2);
						pmenu_State = menu[2];
					}
				}
				SoundManager.s_Instance.Sound_EffectMenuMove();
				break;
			case VINPUT_EVENT.SWIPE_BACKWARD_1FINGER:               // 손가락 하나로 뒤로 스와이프
				if (uIState == UIState.menu)
				{
					// 중앙 메뉴일 때
					if (menu_State == menu[1])
					{
						mAni.SetInteger("Index", 0);
						menu_State = menu[0];
					}
					else if (menu_State == menu[2])
					{
						mAni.SetInteger("Index", 1);
						menu_State = menu[1];
					}
					else if (menu_State == menu[3])
					{
						mAni.SetInteger("Index", 2);
						menu_State = menu[2];
					}
				}
				else if (uIState == UIState.play_m)
				{
					if (pmenu_State == menu[2])
					{
						pmAni.SetInteger("Index", 1);
						pmenu_State = menu[1];
					}
				}
				SoundManager.s_Instance.Sound_EffectMenuMove();
				break;
			case VINPUT_EVENT.SWIPE_UP_1FINGER:                     // 손가락 하나로 위로 스와이프
				break;
			case VINPUT_EVENT.SWIPE_DOWN_1FINGER:                  // 손가락 하나로 밑으로 스와이프
				if(uIState == UIState.menu)
				{
					CreditOn();
					uIState = UIState.credit_m;
					StartCoroutine("CreditChecker");
				}
				break;
			case VINPUT_EVENT.SWIPE_FORWARD_2FINGER:               // 손가락 두 개로 앞으로 스와이프

				break;
			case VINPUT_EVENT.SWIPE_BACKWARD_2FINGER:              // 손가락 두 개로 앞으로 스와이프

				break;
			case VINPUT_EVENT.HOLD_1FINGER:                        // 손가락 하나 대고 있기
				if (uIState == UIState.Idle) Application.Quit();                                // 기본 메뉴일 시 앱 종료
				break;
		}
	}

	//크레딧 재생확인 코루틴
	IEnumerator CreditChecker()
	{
		while(credit_Ani.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
		{
			yield return null;
		}
		CreditOff();
		uIState = UIState.menu;
		yield return null;
	}

	// 공놀이 시작 메서드
	void StartBall()
	{
		// 실패 횟수가 1일때 시작할 수 있게 한다.
		if (!oneBall)
		{
			GameObject b_object = ObjectManager.instance.B_Expert();
			b_object.transform.position = transform.position - (Vector3.up * 0.5f);
			ballScript = b_object.GetComponent<Ball>();
			isPlayBall = true;
			oneBall = true;
		}
	}

	// 먹이주기 함수, 오브젝트 매니저와 연계됨. 
	void PlayFeed()
	{
		fTry++;
		//게임 오브젝트 꺼내오고, 위치 수정
		if (AI.success + AI.fail < 5)
		{
			GameObject f_object = ObjectManager.instance.F_Expert();
			f_object.transform.position = transform.position - (Vector3.up * 1);
		}
	}

	IEnumerator UIWait()
	{
		yield return new WaitForSecondsRealtime(6f);
		isStarting = false;
	}

	IEnumerator FeedCoolCoru()
	{
		yield return new WaitForSecondsRealtime(2.2f);
		feedCool = false;
	}

	// 호출용 15초 대기, 시간을 기다리는 중에 공놀이, 먹이 주기 선택 시 취소 된다.(어차피 그 쪽 스크립트 흐름대로 가도 isStillMove는 false로 변환됨)
	IEnumerator CallWait()
	{
		callTimer = 0;
		while (true)
		{
			callTimer += Time.deltaTime;
			if (callTimer >= 15f)
			{
				AI.isStillMove = false;

				lookCheckerTF.SetParent(transform);
				lookCheckerTF.localPosition = new Vector3(0, 0, 4.5f);
				yield break;
			}
			else if (uIState == UIState.Ball || uIState == UIState.Feed)
			{
				lookCheckerTF.SetParent(transform);
				lookCheckerTF.localPosition = new Vector3(0, 0, 4.5f);
				yield break;
			}
			yield return null;
		}
	}
}
