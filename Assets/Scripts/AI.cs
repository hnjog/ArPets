using System.Collections;
using UnityEngine;

// 곡선 이동, 이동할 범위의 제한
// slerp와 시점 변환(transform.LookAt(transform.position + forward);)
// 최대 이동거리 지정 및 너무 갑작스러운 각도의 조정
// 시작 지점 대신 카메라의 중앙 지점으로 변경함.
// 벨루가 상호작용과 감정 애니메이션을 분리하는게 나을수도?
// 정리 필요
// 기본 AI - 상태 및 수치만 들어있는 스크립트 생성
// AI를 상속 받아 지금 이 스크립트는 AIMoveController 스크립트로 변환
// AI 감정을 변환 관련 스크립트
// AI 상태 관련 스크립트

public class AI : MonoBehaviour
{
    public Animator veluga_Ani;               // 벨루가 애니메이션

    [Header("bool 변수")]
    protected bool isMoving = false;            // 업데이트로 움직이게 하는 용도의 bool 변수
    protected bool isReturning = false;         // 리턴용 변수
    //bool isLookMe = true;                     // 카메라가 나를 보고 있는가

    [Header("속도 관련")]
    float moSpeed = 1.5f;                     // 이동 스피드
    float roSpeed = 0.01f;                    // 회전 스피드

    [Header("위치 조정용")]
    [SerializeField]
    Transform lookChecker;                    // 카메라와 일정거리 이상 떨어져 있는가
    float distance;                           // 거리

    [Header("쿼터니언 값 조정")]
    int qx = 0;                               // x 쿼터니언               
    int qy = 0;                               // y 쿼터니언

    [Header("행복도와 만복도")]
    int happiness = 50;                       // 행복도 (기본수치 50)
    int foodPoint = 50;                       // 만복도 (기본수치 50)


    // 여러 가지 상태들
    enum Chara_State
    {
        state_Normal = 0,                     // 일반 상태      ( 이동과 대기를 하며 움직인다)
        state_Happy = 1,                      // 기쁨 상태      ( 행복도 상승시마다 실행되며 그 경우 관련 애니메이션을 재생하는 용도)
        state_Sad = 2,                        // 슬픔 상태      ( 행복도 하락시마다 실행되며 그 경우 관련 애니메이션을 재생하는 용도)
        state_Shock = 3,                      // 놀람 상태      ( 처음 들어올 때의 놀람, 애니메이션 재생 후 인사 상태로)
        state_Hi = 4,                         // 인사 상태      ( 애니메이션 재생 후 일반 상태로 간다)
        state_Eat = 5,                        // 먹이주기 상태  ( 식사 UI로 인한 상태, 놀이의 결과에 따라 행복도와 만복도에 영향을 준다.)
        state_Ball = 6                        // 공놀이 상태    ( 공놀이 UI로 인한 상태, 놀이의 결과에 따라 행복도에 영향을 준다.)
    }

    // 감정   (일반 상태에서만 사용될 예정, 표정 변화)
    enum Chara_Emotion
    {   
        emotion_Idle = 0,                    // 기본 상태       
        emotion_Happy = 1,                   // 기쁨 상태        ( 행복도 70이상 시)
        emotion_Angry = 2,                   // 화남 상태        ( 행복도 29이하 시)
        emotion_VeryAngry =3                 // 매우 화남 상태   ( 행복도 24 이하 및 만복도 9 이하 시)  
    }
    
    // 일반상태일 시 사용하는 상태들(이동과 대기)
    enum Chara_Move
    {
        idle = 0,                       // 정지 상태  (2초마다 60%으로의 확률로 이동 상태로 변함)
        move = 1,                       // 이동 상태  (2초마다 25%으로의 확률로 정지 상태가 됨)
    }

    static Chara_Move veluga_MoveState = Chara_Move.idle;

    // 여기까지를 AI
    // 밑의 내용을 AIMoveController로 만듬.

    void Start()
    {
        // 반복 코루틴을 통해 업데이트문을 과하게 사용하지 않음
        StartCoroutine(TestAI());
        StartCoroutine(RandRo());

    }

    //순차적으로 변하는 것만 사용. 조금씩 이동, 회전 하는 등
    void Update()
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
        veluga_Ani.SetInteger("State", 1);

        transform.Translate(Vector3.forward * Time.deltaTime * moSpeed);                               // 정면 * 거리 * 일정시간 나눠주기

        veluga_MoveState = Chara_Move.move;
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
        
        veluga_MoveState = Chara_Move.idle;
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
    public void VelugaLook()
    {
        // if문을 이용하여 ro 값을 조정하면 회전을 부드럽게 할수 있지 않을까
        roSpeed = Random.Range(0.01f, 0.05f);
        
        // 임의의 쿼터니언 값을 가지게 함.
        Quaternion qa = Quaternion.Euler(qx,qy,0);

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
