using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosController {
	private ChaosState _currState;
	private ChaosRuleSelector _selector;
	private Transform _parent;
	public ChaosController (ChaosState initialState, ChaosRuleSelector selector, Transform parent, bool spawnInital = true) {
		_currState = initialState;
		_selector = selector;
		_parent = parent;
		if(spawnInital) {
			_currState.SpawnRepresentation (_parent);	
		}
	}

	public void Run(int numIteration) {
		for(int runNum = 0; runNum < numIteration; ++runNum) {
			_currState = _selector.ApplyRule (_currState);
			_currState.SpawnRepresentation (_parent);
		}
	}

}
