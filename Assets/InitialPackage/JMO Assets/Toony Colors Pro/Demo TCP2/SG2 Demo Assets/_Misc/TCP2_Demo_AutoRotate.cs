﻿#region

using UnityEngine;

#endregion

public class TCP2_Demo_AutoRotate : MonoBehaviour
{
	public Vector3 axis = Vector3.up;
	public float Speed = -50f;

	void Update()
	{
		this.transform.Rotate(axis, Time.deltaTime * Speed);
	}
}
