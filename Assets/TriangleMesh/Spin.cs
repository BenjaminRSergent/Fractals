using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {
    public float speed = 1.0f;

	// Update is called once per frame
	void FixedUpdate () {
        transform.rotation = transform.rotation * Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up);
	}
}
