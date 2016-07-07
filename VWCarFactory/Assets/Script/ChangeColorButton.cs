using UnityEngine;
using System.Collections;

public class ChangeColorButton : MonoBehaviour {

	public int thisID;

	// Use this for initialization
	void Start () {
	
	}
	
	public void ChangeColor()
	{
		CarControl.instance.ChangeColor (thisID);
	}
}
