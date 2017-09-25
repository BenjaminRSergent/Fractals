using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEquationRule : ChaosRule {
	public delegate SimplePointChaosState EquationRule(SimplePointChaosState input);
	private EquationRule _rule;
	public PointEquationRule (EquationRule rule) {
		_rule = rule;
	}
	public override ChaosState Iterate(ChaosState currState) {
		return _rule (currState as SimplePointChaosState);
	}
}
