using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChaosStateRepresentation {
	public struct SpawnDetails {
		public static SpawnDetails FromPosition(Vector3 pos) {
			SpawnDetails details = new SpawnDetails();
			details.position = pos;
			details.rotation = Quaternion.identity;
			details.scale = Vector3.one;
			details.color = Color.white;
			details.extra = null;

			return details;
		}
		public Vector3 position; 
		public Quaternion rotation; 
		public Vector3 scale;
		public Color color;
		public object extra;
	}
	public abstract void Spawn(SpawnDetails details, Transform parent);
}
