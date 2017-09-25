using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChaosRule {
	public abstract ChaosState Iterate(ChaosState currState);
}
