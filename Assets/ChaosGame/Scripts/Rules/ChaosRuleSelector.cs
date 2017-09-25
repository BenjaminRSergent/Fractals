using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChaosRuleSelector {
	public abstract ChaosState ApplyRule (ChaosState state);
}
