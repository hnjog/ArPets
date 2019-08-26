using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기초 스크립트
// 이동 지점을 받아와서 해당 이동 지점을 바라보며 이동.
// 다만, 새로운 스크립트는 이동 지점이 존재하지 않음.

public class BasicAI : MonoBehaviour
{
    [SerializeField]
    GameObject moveSpot = null;          // 이동할 좌표들이 있는 게임오브젝트
    [SerializeField]
    Transform[] movePoints = null;       // 이동 좌표들

    public Animator veluga_Ani;          // 벨루가 애니메이션

    public GameObject cam;

    int targetIndex;                 // 이동지점의 인덱스

    bool isMoving = false;           // 업데이트로 움직이게 하는 용도의 bool 변수

    float speed = 1.5f;              // 스피드

    // 캐릭터의 상태를 서술
    enum Chara_State
    {
        state_Idle = 0,                       //정지 상태  (2초마다 60%으로의 확률로 이동 상태로 변함)
        state_Move = 1,                       //이동 상태  (2초마다 30%으로의 확률로 정지 상태가 됨)
    }

    //벨루가 상태
    static Chara_State veluga_State;

    void Start()
    {

        //각 자식들의 위치를 movePoints에 집어넣음, childCount로 자식의 개수를 구함, 자식들의 각 위치를 movePoints에 집어넣는다.

        for (int x = 0; x < moveSpot.transform.childCount; x++)
            movePoints[x] = moveSpot.transform.GetChild(x);

        // 반복 코루틴을 통해 업데이트문을 사용하지 않음.(최적화)
        StartCoroutine(TestAI());
    }

    //순차적으로 변하는 것만 사용. 조금씩 이동, 회전 하는 등
    void Update()
    {
        //이동 
        if (isMoving)
        {
            TargetMove();
        }
        else
        {

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

    // 이동지점 지정 
    public void TargetSearch()
    {
        int randomIndex = Random.Range(0, movePoints.Length);                           // 랜덤 인덱스 지정
        targetIndex = randomIndex;                                                      // 이동은 TargetMove에서

        veluga_State = Chara_State.state_Move;
    }
    // 이동지점으로 이동
    public void TargetMove()
    {
        veluga_Ani.SetInteger("State", 1);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);                               // 정면 * 거리 * 일정시간 나눠주기
        transform.LookAt(movePoints[targetIndex].position);                                          // 타겟 바라보기
        float dis = Vector3.Distance(transform.position, movePoints[targetIndex].position);          // 타겟과 자신의 현재거리 측정
        Debug.Log(dis);
        //이동 완료시(0.5f 이내일시) 새로운 이동지점 지정
        if (dis <= 0.5f)
        {
            TargetSearch();
        }
    }
    

    // 대기상태
    public void Wating()
    {
        veluga_Ani.SetInteger("State", 0);
        transform.LookAt(cam.transform.position);

        //지속 실행 방지... 인데 이게 효율적인지는 의문
        if (transform.position != transform.position)
        {
            transform.Translate(transform.position);
        }
        veluga_State = Chara_State.state_Idle;
    }

    // 2초마다 행동을 결정해줄 함수
    public void Timer2Second()
    {
        // 대기 상태, 60%의 확률
        if(veluga_State== Chara_State.state_Idle && Random.value >= 0.6)
        {
            isMoving = true;
            TargetSearch();
        }
        // 이동 상태, 30%의 확률
        else if(veluga_State == Chara_State.state_Move && Random.value <= 0.3)
        {
            isMoving = false;
            Wating();
        }
    }


}
