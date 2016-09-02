using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFrom3D : MonoBehaviour {

	public string thisTargetName;
	public string Icon;
	public string description;
	public GameObject thisTarget;
	public string targetName;
	public IButtonInfo buttonInfo;

	// Use this for initialization
	void Start () {
		if (thisTargetName != "") {
			thisTarget = GameObject.Find (thisTargetName);
		}
	}

	void OnEnable()
	{
		if (thisTargetName != "") {
//			List<IButtonInfo> btnInfo = AppData.GetCarPartsByName(Scene1_UI.CarSeleted,buttonType);
//			foreach (IButtonInfo item in btnInfo) {
//				if (item.Name == buttonName) {
//					SetThisTarget (GameObject.Find (buttonName + "Float"), item);
//				}
//			}
			thisTarget = GameObject.Find (thisTargetName);
		}
	}

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
		}else if (thisTargetName != "") {
			thisTarget = GameObject.Find (thisTargetName);
		}
		if(thisTargetName == "" && !thisTarget) {
			SetThisTarget (GameObject.Find(AppData.GetSamples (buttonInfo.Description).Asset),buttonInfo);
		}
	}

	public void ClickThisButton()
	{
		if (GameManager.instance.inAnimation == false) {
			if (buttonInfo.Name == "后盖开启" && Scene1_UI.CarSeleted == "Tiguan") {
				
				//Debug.Log ("CarStudio.Exists (buttonInfo.Name) " + CarStudio.Exists (buttonInfo.Name));


				Texture2D img;
				if (CarStudio.Exists (buttonInfo.Name)) {
					img = Resources.Load (buttonInfo.Icon) as Texture2D;
					Debug.Log ("CarStudio.Exist");
					AnimationPlay ("_playback");
				} else {
					img = Resources.Load (buttonInfo.Icon + "b") as Texture2D;
					Debug.Log ("CarStudio. not Exist");
					CarStudio.AddPart (buttonInfo.Name);
					AnimationPlay ("_play");
				}
				GameManager.instance.nowCustomButton.thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));


				//CarStudio.RemovePart (buttonInfo.Name);
			} else {
				Texture2D img = Resources.Load (AppData.GetSamples (buttonInfo.Description).Icon) as Texture2D;
				UIManager.instance.ShowFloatWindow (img, AppData.GetSamples (buttonInfo.Description).Description,true);
			}
		}
	}

	public void ClickButtonSpecial()
	{
		Texture2D img = Resources.Load (Icon) as Texture2D;
		UIManager.instance.ShowFloatWindow (img, description,false);
	}

	public void AnimationPlay(string str)
	{
		GameObject[] parts = GameObject.FindGameObjectsWithTag("AnimPart");
		foreach (GameObject obj in parts) {
			obj.GetComponent<PartAnimation> ().SettingAnimation (buttonInfo.Name + str);
		}
	}
}
