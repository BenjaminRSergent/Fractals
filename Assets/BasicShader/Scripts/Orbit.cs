using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
    public Transform target;
    public float speed = 1.0f;

    [SerializeField]
    private float _dist;
    [SerializeField]
    private float _height;

    void Start () {
        var posDiff = target.position - transform.position;
        _height = -posDiff.y;
        posDiff.y = 0;
        _dist = posDiff.magnitude;
    }
	
	void FixedUpdate () {
        float angle = Time.time * speed;

        var newPos = target.transform.position;
        newPos.x += _dist * Mathf.Cos(angle);
        newPos.y += _height;
        newPos.z += _dist * Mathf.Sin(angle);

        transform.position = newPos;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position); ;
    }
}
