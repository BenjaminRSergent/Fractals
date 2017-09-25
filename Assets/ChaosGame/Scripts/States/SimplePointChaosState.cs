using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePointChaosState : ChaosState {
	private ChaosStateRepresentation _representation;
	private Vector3 _position;

	public Vector3 Position {
		get {
			return _position;
		}
	}

	public ChaosStateRepresentation Representation {
		get {
			return _representation;
		}
	}

	public SimplePointChaosState(Vector3 position, ChaosStateRepresentation representation) {
		_position = position;
		_representation = representation;
	}

	public override void SpawnRepresentation  (Transform parent) {
		var details = ChaosStateRepresentation.SpawnDetails.FromPosition (_position);
		_representation.Spawn (details, parent);
	}
}
