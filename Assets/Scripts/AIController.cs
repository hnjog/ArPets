using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 곡선 이동, 이동할 범위의 제한
// slerp와 시점 변환(transform.LookAt(transform.position + forward);)
// 최대 이동거리 지정 및 너무 갑작스러운 각도의 조정

public class AIController : MonoBehaviour
{
    public Animator veluga_Ani;               // 벨루가 애니메이션

    [Header("bool 변수")]
    bool isMoving = false;                    // 업데이트로 움직이게 하는 용도의 bool 변수
    bool isReturning = false;                 // 리턴용 변수

    [Header("속도 관련")]
    float moSpeed = 1.5f;                     // 이동 스피드
    float roSpeed = 0.01f;                    // 회전 스피드

    [Header("위치 조정용")]
    [SerializeField]
    Transform startPos;                       // 시작위치
    float distance;                           // 거리

    [Header("쿼터니언 값 조정")]
    int qx = 0;                               // x 쿼터니언               
    int qy = 0;                               // y 쿼터니언
    
    enum Chara_State
    {
        state_Idle = 0,                       // 정지 상태  (2초마다 60%으로의 확률로 이동 상태로 변함)
        state_Move = 1,                       // 이동 상태  (2초마다 25%으로의 확률로 정지 상태가 됨)
        
    }

    static Chara_State veluga_State = Chara_State.state_Idle;

    void Start()
    {

        // 반복 코루틴을 통해 업데이트문을 사용하지 않음.(최적화)
        StartCoroutine(TestAI());
        StartCoroutine(RandRo());
    }

    //순차적으로 변하는 것만 사용. 조금씩 이동, 회전 하는 등
    void Update()
    {
        //이동 
        if (isMoving)
        {
            Look();
            Move();
        }
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, qy, 0), 0.01f);

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
        if (veluga_State == Chara_State.state_Idle && Random.value >= 0.6)
        {
            isMoving = true;
        }
        // 이동 상태, 30%의 확률
        else if (veluga_State == Chara_State.state_Move && Random.value <= 0.25)
        {
            isMoving = false;
            Wating();
        }

    }

    // 이동 상태
    public void Move()
    {
        veluga_Ani.SetInteger("State", 1);

        transform.Translate(Vector3.forward * Time.deltaTime * moSpeed);                               // 정면 * 거리 * 일정시간 나눠주기

        veluga_State = Chara_State.state_Move;
    }

    // 대기상태
    public void Wating()
    {
        veluga_Ani.SetInteger("State", 0);

        //지속 실행 방지... 인데 이게 효율적인지는 의문
        if (transform.position != transform.position)
        {
            transform.Translate(transform.position);
        }
        
        veluga_State = Chara_State.state_Idle;
    }

    // 움직일때 1초마다 랜덤한 시점처리
    IEnumerator RandRo()
    {
        while(true)
        {
            if(isMoving) RandomRotate();
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
    public void Look()
    {
        // if문을 이용하여 ro 값을 조정하면 회전을 부드럽게 할수 있지 않을까
        roSpeed = Random.Range(0.01f, 0.05f);
        
        // 임의의 쿼터니언 값을 가지게 함.
        Quaternion qa = Quaternion.Euler(qx,qy,0);

        distance = Vector3.Distance(transform.position, Vector3.zero);

        // 거리 체크
        if (distance >= 2) isReturning = true;
        else if (distance <= 1) isReturning = false;
        // 거리가 일정거리 이상이면 시점을 시작지점으로 돌리게 한다.
        if (!isReturning)
            transform.rotation = Quaternion.Slerp(transform.rotation, qa, roSpeed);
        else
        {
            Quaternion qb = Quaternion.LookRotation(startPos.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, qb, roSpeed);
        }
    }


}
