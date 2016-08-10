using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFrom3D : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public Vector3 WorldToUI(Camera camera,Vector3 pos){  
		CanvasScaler scaler = GameObject.Find("UIRoot").GetComponent<CanvasScaler>();  

		float resolutionX = scaler.referenceResolution.x;  
		float resolutionY = scaler.referenceResolution.y;  

		Vector3 viewportPos = camera.WorldToViewportPoint(pos);  

		Vector3 uiPos = new Vector3(viewportPos.x * resolutionX - resolutionX * 0.5f,  
			viewportPos.y * resolutionY - resolutionY * 0.5f,0);  

		return uiPos;
	} 
	
	// Update is called once per frame
	void Update () {
	
	}
}
