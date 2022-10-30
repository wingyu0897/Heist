using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : MonoBehaviour
{
	protected AIBrain brain;

	[SerializeField] private List<AIAction> actions = new List<AIAction>();
	[SerializeField] private List<AITransition> transitions = new List<AITransition>();

	public virtual void Awake()
	{
		brain = GetComponentInParent<AIBrain>();
	}

	public void UpdateState() //액션을 수행 및 상태 변경 조건을 체크하는 함수
	{
		foreach (AIAction ac in actions) //액션 수행
		{
			ac.TakeAction();
		}

		if (brain.canTransition)
		{
			foreach (AITransition tr in transitions) //모든 변경 조건을 체크
			{
				bool result = false;
				foreach (AIDecision de in tr.decisions)
				{
					result = de.Result();
					if (!result)
					{
						break;
					}
				}

				if (result == true) //result가 참일 때 transition의 positiveState로 상태를 변경, positiveState가 없을 경우 아무 수행 없음
				{
					if (tr.postiveState != null)
					{
						brain.ChangeState(tr.postiveState);
						return;
					}
				}
				else //result가 거짓일 때 transition의 negativeState로 상태를 변경, negativeState가 없을 경우 아무 수행 없음
				{
					if (tr.negativeState != null)
					{
						brain.ChangeState(tr.negativeState);
						return;
					}
				}
			}
		}
	}

	public void StartState() //상태가 시작될 때 호출하는 함수
	{
		foreach (AIAction ac in actions)
		{
			ac.EnterAction();
		}
	}
	
	public void EndState() //상태가 끝날 때 호출하는 함수
	{
		foreach (AIAction ac in actions)
		{
			ac.ExitAction();
		}
	}
}
