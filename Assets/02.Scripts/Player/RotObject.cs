using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotObject : MonoBehaviour
{
    [Range(-180f, 180f)]
    [SerializeField] 
    private float viewRotateZ = 0f;
    public GameObject spotLight;

    public UnityEvent<Vector2> OnPointer;

	private void Update()
	{
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        OnPointer?.Invoke(mousePos);
        LookAt(mousePos);
    }

	public void LookAt(Vector2 targetPos)
    {
        Vector2 dir = (targetPos - (Vector2)transform.position);
        float targetAngle = -(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
        viewRotateZ = Mathf.LerpAngle(viewRotateZ, targetAngle, 0.007f);
        spotLight.transform.localRotation = Quaternion.Euler(0, 0, -viewRotateZ);
    }
}
