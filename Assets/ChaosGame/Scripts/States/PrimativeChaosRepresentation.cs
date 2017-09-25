using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimativeChaosRepresentation : ChaosStateRepresentation {
	private float _baseSize;
	private PrimitiveType _type;
	public PrimativeChaosRepresentation(float baseSize, PrimitiveType type) {
		_baseSize = baseSize;
		_type = type;
	}
	// TODO: Color rules
	public override void Spawn(SpawnDetails details, Transform parent) {
		GameObject sphere = GameObject.CreatePrimitive (_type);
		sphere.transform.position = details.position;
		sphere.transform.rotation = details.rotation;
		sphere.transform.localScale = _baseSize * details.scale;
		sphere.transform.parent = parent;
		// TODO: Cache color materials. This can get expensive.
		//sphere.GetComponent<Renderer>().material.color = details.color;
	}
}
