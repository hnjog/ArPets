using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 벨루가가 화면 밖에 있을 때, 화살표를 통해 벨루가가 어디있는지 가늠할 수 있게 
// vector2를 이용하여 화살표(2D) - 화면 중앙

public class Pointer : MonoBehaviour
{
	// 벨루가
	[SerializeField]
	GameObject veluga = null;
	//[SerializeField] GameObject cTF = null;

	private void OnEnable()
	{
		StartCoroutine(Point());
	}

	IEnumerator Point()
	{
		while (true)
		{
			Vector3 veluga_pos = veluga.transform.position;
			Vector3 point_Dir = new Vector3(veluga_pos.x - transform.position.x,
											veluga_pos.y - transform.position.y,
											veluga_pos.z - transform.position.z);
			transform.forward = point_Dir;
			yield return null;
		}
	}
}
