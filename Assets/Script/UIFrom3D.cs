using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFrom3D : MonoBehaviour {

	public GameObject thisTarget;
	public string targetName;
	IButtonInfo buttonInfo;

	// Use this for initialization
//	void Start () {
//	
//	}

	public void SetThisTarget(GameObject target,IButtonInfo btn)
	{
		thisTarget = target;
		buttonInfo = btn;
	}

	Vector3 WorldToUI(Camera camera,Vector3 pos){  
		CanvasScaler scaler = GameObject.Find("Canvas").GetComponent<CanvasScaler>();  

		float resolutionX = scaler.referenceResolution.x;  
		float resolutionY = scaler.referenceResolution.y;  

		Vector3 viewportPos = camera.WorldToViewportPoint(pos);  

		Vector3 uiPos = new Vector3(viewportPos.x * resolutionX - resolutionX * 0.5f,  
			viewportPos.y * resolutionY - resolutionY * 0.5f,0);  

		return uiPos;
	}
	
	// Update is called once per frame
	void Update () {
		if (thisTarget) {
			transform.localPosition = WorldToUI (Camera.main, thisTarget.transform.position);
		}

	}

	public void ClickThisButton()
	{
		Texture2D img = Resources.Load (AppData.GetSamples (buttonInfo.Description).Icon) as Texture2D;
		UIManager.instance.ShowFloatWindow (img, AppData.GetSamples (buttonInfo.Description).Description);
	}
}
