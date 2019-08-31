using System.Collections;
using UnityEngine;

// 기본 AI - 상태 및 수치만 들어있는 스크립트 생성
// AI를 상속 시켜 이동, 상태, 감정 스크립트 제어를 따로 한다.
// 일반 상태가 아닐 경우, 이동과 감정 스크립트는 notWork 상태로 만들어 제어한다.

public class AI : MonoBehaviour
{
    public Animator veluga_Ani;               // 벨루가 애니메이션

    [Header("bool 변수")]
    protected bool isMoving = false;            // 업데이트로 움직이게 하는 용도의 bool 변수
    protected bool isReturning = false;         // 리턴용 변수
    //bool isLookMe = true;                     // 카메라가 나를 보고 있는가

    [Header("속도 관련")]
	protected float moSpeed = 1.5f;                     // 이동 스피드
	protected float roSpeed = 0.01f;                    // 회전 스피드

    [Header("위치 조정용")]
    [SerializeField]
    protected Transform lookChecker;                    // 카메라와 일정거리 이상 떨어져 있는가
	protected float distance;                           // 거리

    [Header("쿼터니언 값 조정")]
    protected int qx = 0;                               // x 쿼터니언               
	protected int qy = 0;                               // y 쿼터니언

    [Header("행복도와 만복도")]
    protected int happiness = 50;                       // 행복도 (기본수치 50)
	protected int foodPoint = 50;                       // 만복도 (기본수치 50)


	// 여러 가지 상태들
	protected enum Chara_State
    {
        state_Idle = 0,                       // 일반 상태      ( 이동과 대기를 하며 움직인다)
        state_Happy = 1,                      // 기쁨 상태      ( 행복도 상승시마다 실행되며 그 경우 관련 애니메이션을 재생하는 용도)
        state_Sad = 2,                        // 슬픔 상태      ( 행복도 하락시마다 실행되며 그 경우 관련 애니메이션을 재생하는 용도)
        state_Shock = 3,                      // 놀람 상태      ( 처음 들어올 때의 놀람, 애니메이션 재생 후 인사 상태로)
        state_Hi = 4,                         // 인사 상태      ( 애니메이션 재생 후 일반 상태로 간다)
        state_Eat = 5,                        // 먹이주기 상태  ( 식사 UI로 인한 상태, 놀이의 결과에 따라 행복도와 만복도에 영향을 준다.)
        state_Ball = 6                        // 공놀이 상태    ( 공놀이 UI로 인한 상태, 놀이의 결과에 따라 행복도에 영향을 준다.)
    }

	// 감정   (일반 상태에서만 사용될 예정, 표정 변화)
	protected enum Chara_Emotion
    {   
        emotion_Idle = 0,                    // 기본 상태       
        emotion_Happy = 1,                   // 기쁨 상태        ( 행복도 70이상 시)
        emotion_Angry = 2,                   // 화남 상태        ( 행복도 29이하 시)
        emotion_VeryAngry =3,                // 매우 화남 상태   ( 행복도 24 이하 및 만복도 9 이하 시) 
		emotion_NotWork = 4				     // 상태가 일반이 아닐 경우 이 상태가 됨.
    }

	// 이동  (일반 상태에서만 사용됨, 벨루가의 움직임 담당) - AIMoveController 에서 주로 사용
	protected enum Chara_Move
    {
        idle = 0,                       // 정지 상태  (2초마다 60%으로의 확률로 이동 상태로 변함)
        move = 1,                       // 이동 상태  (2초마다 25%으로의 확률로 정지 상태가 됨)
		notWork = 2					    // 상태가 일반이 아닐 경우 이 상태가 됨. - state 스크립트에서 건드림.
    }

	protected static Chara_State veluga_State = Chara_State.state_Idle;
	protected static Chara_Emotion veluga_Emotion = Chara_Emotion.emotion_Idle;
	protected static Chara_Move veluga_MoveState = Chara_Move.idle;
}
