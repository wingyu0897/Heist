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

	public void UpdateState() //�׼��� ���� �� ���� ���� ������ üũ�ϴ� �Լ�
	{
		foreach (AIAction ac in actions) //�׼� ����
		{
			ac.TakeAction();
		}

		if (brain.canTransition)
		{
			foreach (AITransition tr in transitions) //��� ���� ������ üũ
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

				if (result == true) //result�� ���� �� transition�� positiveState�� ���¸� ����, positiveState�� ���� ��� �ƹ� ���� ����
				{
					if (tr.postiveState != null)
					{
						brain.ChangeState(tr.postiveState);
						return;
					}
				}
				else //result�� ������ �� transition�� negativeState�� ���¸� ����, negativeState�� ���� ��� �ƹ� ���� ����
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

	public void StartState() //���°� ���۵� �� ȣ���ϴ� �Լ�
	{
		foreach (AIAction ac in actions)
		{
			ac.EnterAction();
		}
	}
	
	public void EndState() //���°� ���� �� ȣ���ϴ� �Լ�
	{
		foreach (AIAction ac in actions)
		{
			ac.ExitAction();
		}
	}
}
