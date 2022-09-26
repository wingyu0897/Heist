using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

	public bool isRunning = false;
	public bool canRun = true;

	public Weapon primaryGun;
	public Weapon secondaryGun;
	public Weapon melee;

	public int backPacks = 0;
	public int interactionSpeed = 1;

	private void Awake()
	{
		Instance = this;
	}
}
