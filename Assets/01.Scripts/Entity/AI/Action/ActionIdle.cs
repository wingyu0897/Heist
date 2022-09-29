using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIdle : AIAction
{
	public override void EnterAction()
	{
		
	}

	public override void TakeAction()
	{
		brain.MoveTo(Vector2.zero, Vector2.zero);
	}

	public override void ExitAction()
	{

	}
}
