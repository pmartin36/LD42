using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedDisplay : MonoBehaviour {

	TMP_Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<TMP_Text>();
		GameManager.Instance.SpeedDisplay = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateSpeed(float speed) {
		text.text = speed.ToString("N0");
	}
}
