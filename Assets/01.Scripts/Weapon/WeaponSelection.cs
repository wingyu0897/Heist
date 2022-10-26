using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public Weapon meleeWeapon;

	/// <summary>
	/// 무기를 지정하는 함수
	/// </summary>
	/// <param name="primary">주무기 삽입</param>
	/// <param name="secondary">보조무기 삽입</param>
	/// <param name="melee">근접무기 삽입</param>
	public void SetWeapons(Weapon primary, Weapon secondary = null, Weapon melee = null)
	{

	}
}
