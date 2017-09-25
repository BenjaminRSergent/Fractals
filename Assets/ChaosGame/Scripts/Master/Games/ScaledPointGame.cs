using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaledPointGame : MonoBehaviour {
	[SerializeField]
	public List<Vector3> _points;
	[SerializeField]
	public List<float> _scales;
	// TODO: Make a timer class to allow building a fractal over time
	[SerializeField]
	public float _baseSphereSize = 1f;
	[SerializeField]
	public int _numRuns = 10000;
	[SerializeField]
	private PrimitiveType _primativeTypeToUse = PrimitiveType.Cube;
	private RandomChaosRuleTable _ruleTable = new RandomChaosRuleTable();
	private ChaosController _controller = null;

	// Use this for initialization
	void Start () {
		// Make equally probable rules for every point scale combo
		for(int pointIndex = 0; pointIndex < _points.Count; pointIndex++) {
			for(int scaleIndex = 0; scaleIndex < _scales.Count; scaleIndex++) {
				_ruleTable.AddRule (MakeRuleFor (transform.position + _points [pointIndex], _scales [scaleIndex]), 1);
			}
		}

		Vector3 startingPoint = _points [Random.Range (0, _points.Count)];
		PrimativeChaosRepresentation sphereRep = new PrimativeChaosRepresentation (_baseSphereSize, _primativeTypeToUse);
		ChaosState initialState = new SimplePointChaosState (startingPoint, sphereRep);
		_controller = new ChaosController (initialState, _ruleTable, transform, true);
		_controller.Run (_numRuns);
	}

	PointEquationRule MakeRuleFor(Vector3 point, float scale) {
		PointEquationRule.EquationRule equation = input => 
		{ 
			Vector3 newPos = input.Position * scale + point * (1 - scale);
			return new SimplePointChaosState(newPos, input.Representation); 
		};
		return new PointEquationRule (equation);
	}
}
