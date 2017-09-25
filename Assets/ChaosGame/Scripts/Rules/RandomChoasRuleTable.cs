using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChaosRuleTable : ChaosRuleSelector {
	private struct ChaosRuleOption {
		public ChaosRuleOption(ChaosRule ruleIn, int probabilityIn) {
			rule = ruleIn;
			probability = probabilityIn;
		}

		public ChaosRule rule;
		public int probability;
	}
	private List<ChaosRuleOption> _options = new List<ChaosRuleOption>();
	private int _maxWeight = 0;

	public void AddRule(ChaosRule rule, int weight) {
		_options.Add (new ChaosRuleOption(rule, weight));
		_maxWeight += weight;
	}

	public override ChaosState ApplyRule(ChaosState state) {
		ChaosRule toUse = GetRandomRule ();
		return toUse.Iterate (state);
	}

	public ChaosRule GetRandomRule() {
		// Decrement the random number by the probabilities in order until it is 0 or less.
		// The index where this occurs correspond to the rule
		int randomNum = Random.Range (0, _maxWeight + 1);
		int index;
		// One less than count to ensure the index is always in range when exiting the loop.
		for(index = 0; randomNum > 0 && index < _options.Count - 1; index++) {
			randomNum -= _options [index].probability;
		}
		return _options [index].rule;
	}
}
