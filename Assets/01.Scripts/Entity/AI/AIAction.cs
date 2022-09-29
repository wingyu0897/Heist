using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
	protected AIBrain brain;

	protected virtual void Awake()
	{
		brain = transform.parent.parent.GetComponent<AIBrain>();
	}

	public abstract void EnterAction(); //액션, 상태가 시작 되었을 때 호춤할 함수
	public abstract void TakeAction(); //액션을 실행하는 함수 Update 함수에서 실행
	public abstract void ExitAction(); //상태가 바뀔 때 호출할 함수
}
