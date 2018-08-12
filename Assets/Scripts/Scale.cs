using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Scale : MonoBehaviour {

	public TMP_Text display;
	private float value;

	// Use this for initialization
	void Start () {
		GameManager.Instance.Scale = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReevaluateWeight() {
		var hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, 1 << LayerMask.NameToLayer("Box"))
					.Select( g => g.GetComponent<Box>());
		value = 0;
		foreach(var h in hits) {
			value += h.Carried || h.IsAirborn ? 0 : h.Weight;
		}
		display.text = $"{value} kg";

		if(value > 15) {
			GameManager.Instance.PlayerLost("You broke the scale!");
		}
	}
}
