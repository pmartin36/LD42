using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour {

	public TMP_Text amtStacked;
	public TMP_Text score;
	public TMP_Text directions;

	public Image background;


	// Use this for initialization
	void Start () {
		GameManager.Instance.GameOverScreen = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetScreen() {
		int boxesPlaced = GameManager.Instance.BoxesPlaced;
		amtStacked.text = $"You stacked {boxesPlaced} boxes";
		if( boxesPlaced < 75 ) {
			score.text = "Subpar Stacker";
		}
		else if(boxesPlaced < 150) {
			score.text = "Standard Stacker";
		}
		else if(boxesPlaced < 225) {
			score.text = "Stupendous Stacker";
		}
		else {
			score.text = "Spectacular Stacker";
		}
		GetComponent<Animator>().Play("GameOver");
	}
}
