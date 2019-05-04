using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed = 1f;
	public float offset = 0f;
	public bool noNegatives = false;
	public Vector3 direction = Vector3.up;

	private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		originalPosition = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		float sinVal = Mathf.Sin (Time.time * speed + offset * Mathf.PI);
		sinVal = noNegatives ? Mathf.Abs (sinVal) : sinVal;
		transform.localPosition = originalPosition + direction * sinVal;
	}
}
