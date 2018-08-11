using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxPlacedCounter : MonoBehaviour {

	TMP_Text text;

	// Use this for initialization
	void Start () {
		GameManager.Instance.BoxPlacedCounter = this;
		text = GetComponent<TMP_Text>();
	}

	public void UpdateCount(int count) {
		text.text = $"Boxes placed: {count}";
	}
}
