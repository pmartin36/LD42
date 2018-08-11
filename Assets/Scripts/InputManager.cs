using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseInput {
	LEFT,
	RIGHT,
	NONE
}

public class InputPackage {
	public float Horizontal { get; set; }
	public float Vertical { get; set; }

	public MouseInput MouseClick { get; set; }
	public Vector2 MouseLocation { get; set; }
}

public class InputManager : MonoBehaviour {

	InputPackage package;

	// Use this for initialization
	void Start () {
		package = new InputPackage();
	}
	
	// Update is called once per frame
	void Update () {
		package.Horizontal = Input.GetAxis("Horizontal");
		package.Vertical = Input.GetAxis("Vertical");

		if (Input.GetMouseButtonDown(0)) {
			package.MouseClick = MouseInput.LEFT;			
		}
		else if(Input.GetMouseButtonDown(1)) {
			package.MouseClick = MouseInput.RIGHT;
		}
		else {
			package.MouseClick = MouseInput.NONE;
		}
		package.MouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		GameManager.Instance.ProcessInputs(package);
	}
}
