using UnityEngine;
using System.Collections;

public class DoorColorChange : MonoBehaviour {

	public Color col;
	static public DoorColorChange instance;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		ChangeColor ();
	}

	public void ChangeColor()
	{
		GetComponent<Renderer> ().material.SetColor ("_DiffuseColor", col);
		GetComponent<Renderer> ().material.SetTexture ("_ReflectionMap", GameManager.instance.cubeMapMats [GameManager.instance.selectedBG]);
	}
}
