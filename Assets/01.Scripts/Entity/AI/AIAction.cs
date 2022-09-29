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

	public abstract void EnterAction(); //�׼�, ���°� ���� �Ǿ��� �� ȣ���� �Լ�
	public abstract void TakeAction(); //�׼��� �����ϴ� �Լ� Update �Լ����� ����
	public abstract void ExitAction(); //���°� �ٲ� �� ȣ���� �Լ�
}
