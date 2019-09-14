using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 곡선 이동, 이동할 범위의 제한
// slerp와 시점 변환(transform.LookAt(transform.position + forward);)
// 최대 이동거리 지정 및 너무 갑작스러운 각도의 조정
// 시작 지점 대신 카메라의 중앙 지점으로 변경함.
// 스크립트 에서 이동부분을 담당하는 것
// 기본 상태일 때만 이동이 되어야 한다

public class AIMoveController : AI
{
    [Header("bool 변수")]
    bool isMoving = false;            // 업데이트로 움직이게 하는 용도의 bool 변수
    bool isReturning = false;         // 리턴용 변수
    //bool isLookMe = true;                     // 카메라가 나를 보고 있는가

    [Header("속도 관련")]
    float moSpeed = 1.5f;                     // 이동 스피드
    float roSpeed = 0.01f;                    // 회전 스피드

    [Header("위치 조정용")]
    [SerializeField]
    Transform lookChecker = null;                    // 카메라와 일정거리 이상 떨어져 있는가
    float distance;                           // 거리

    [Header("쿼터니언 값 조정")]
    int qx = 0;                               // x 쿼터니언               
    int qy = 0;                               // y 쿼터니언

    void Start()
	{
		// 반복 코루틴을 통해 업데이트문을 과하게 사용하지 않음
		StartCoroutine(TestAI());
		StartCoroutine(RandRo());
	}

	//순차적으로 변하는 것만 사용. 조금씩 이동, 회전 하는 등
	void Update()
	{
		if (veluga_State == Chara_State.state_Idle)	// 벨루가가 기본 상태일때만 작동하도록. notwork 상태로 만드는 것은 State 스크립트에서 다룰 것
		{
			//이동 
			if (isMoving)
			{
				VelugaLook();                       // 벨루가 방향
				FreeMove();                         // 벨루가 움직임
			}
			else
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, qy, 0), 0.01f);
		}
	}

	// 테스트용 AI
	IEnumerator TestAI()
	{
		//무한반복
		while (true)
		{
			Timer2Second();
			yield return new WaitForSeconds(2);         // 2초 대기
		}
	}
	// 2초마다 행동을 결정해줄 함수
	public void Timer2Second()
	{

		// 대기 상태, 60%의 확률
		if (veluga_MoveState == Chara_Move.idle && Random.value >= 0.6)
		{
			isMoving = true;
		}
		// 이동 상태, 25%의 확률, 귀환 중에는 사용하지 않음.
		else if (veluga_MoveState == Chara_Move.move && Random.value <= 0.25 && !isReturning)
		{
			isMoving = false;
			Wating();
		}

	}

	// 자유 이동 상태
	public void FreeMove()
	{
		veluga_Ani.SetInteger("MoveState", 1);

		transform.Translate(Vector3.forward * Time.deltaTime * moSpeed);                               // 정면 * 거리 * 일정시간 나눠주기

		veluga_MoveState = Chara_Move.move;
	}

	// 대기상태
	public void Wating()
	{
		veluga_Ani.SetInteger("MoveState", 0);

		//지속 실행 방지... 인데 이게 효율적인지는 의문
		if (transform.position != transform.position)
		{
			transform.Translate(transform.position);
		}

		veluga_MoveState = Chara_Move.idle;
	}

	// 움직일때 1초마다 랜덤한 시점처리
	IEnumerator RandRo()
	{
		while (true)
		{
			if (isMoving) RandomRotate();
			yield return new WaitForSeconds(1);         // 1초 대기
		}
	}

	// 랜덤 방향 지정
	public void RandomRotate()
	{
		qx = Random.Range(-31, 31);
		qy = Random.Range(-121, 121);
	}

	// 시점 변환 메소드
	public void VelugaLook()
	{
		// if문을 이용하여 ro 값을 조정하면 회전을 부드럽게 할수 있지 않을까
		roSpeed = Random.Range(0.01f, 0.05f);

		// 임의의 쿼터니언 값을 가지게 함.
		Quaternion qa = Quaternion.Euler(qx, qy, 0);

		distance = Vector3.Distance(transform.position, lookChecker.position);

		// 거리 체크
		if (distance >= 3) isReturning = true;
		else if (distance <= 1) isReturning = false;
		// 거리가 일정거리 이상이면 시점을 카메라 중앙 지점으로 돌리게 한다.
		if (!isReturning)
			transform.rotation = Quaternion.Slerp(transform.rotation, qa, roSpeed);
		else
		{
			Quaternion qb = Quaternion.LookRotation(lookChecker.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, qb, roSpeed);
		}
	}
}
