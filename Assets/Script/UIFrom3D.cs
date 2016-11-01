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
	public float contentHeight;

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
        // || buttonInfo.Name == "电动踏板"
        if (GameManager.instance.inAnimation == false) {
            /*
			if (buttonInfo.Name == "后盖开启" && Scene1_UI.CarSeleted == "Tiguan") {
				
				//Debug.Log ("CarStudio.Exists (buttonInfo.Name) " + CarStudio.Exists (buttonInfo.Name));


				Texture2D img;
				if (CarStudio.Exists (buttonInfo.Name)) {
					img = Resources.Load (buttonInfo.Icon) as Texture2D;
					Debug.Log ("CarStudio.Exist");
					AnimationPlay (buttonInfo.Name + "_playback");
				} else {
					img = Resources.Load (buttonInfo.Icon + "b") as Texture2D;
					Debug.Log ("CarStudio. not Exist");
					CarStudio.AddPart (buttonInfo.Name);
					AnimationPlay (buttonInfo.Name + "_play");
				}
				GameManager.instance.nowCustomButton.thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));


				//CarStudio.RemovePart (buttonInfo.Name);
			} else {
				Texture2D img = Resources.Load (AppData.GetSamples (buttonInfo.Description).Icon) as Texture2D;
				UIManager.instance.ShowFloatWindow (img, buttonInfo.Name, AppData.GetSamples (buttonInfo.Description).Description,true);
			}
            */
            Texture2D img = Resources.Load(AppData.GetSamples(buttonInfo.Description).Icon) as Texture2D;
            UIManager.instance.ShowFloatWindow(img, buttonInfo.Name, AppData.GetSamples(buttonInfo.Description).Description, true);
			UIManager.instance.ResetFloatwindowContentsize (contentHeight);
        }
	}

	public void ClickButtonSpecial()
	{
        if (GameManager.instance.inAnimation == false)
        {
            if (gameObject.name == "Coach3DButtonBrake")
            {
                CarControl.instance.camTarget = thisTarget.transform;
                GameManager.instance.inCameraPosition = true;
                GameManager.instance.nowCamPositionID = 17;
                CarControl.instance.camNowPosX = 0;
                CarControl.instance.camNowRotateY = Camera.main.transform.rotation.eulerAngles.y;
                GameManager.instance.CameraGoto(GameManager.instance.allCamPosition[17]);
            }

            if (gameObject.name == "Pedal3DButton")
            {
                if (GameManager.instance.pedalOpen == false)
                {
                    GameManager.instance.pedalOpen = true;
                    GameManager.instance.isDoorOpen = false;
                    AnimationPlay("电动踏板_play");
                }
                else
                {
                    GameManager.instance.pedalOpen = false;
                    GameManager.instance.isDoorOpen = true;
                    AnimationPlay("电动踏板_playback");
                    UIManager.instance.float3DButton.gameObject.SetActive(true);
                }
            }
            else if (gameObject.name == "TiguanBack3DButton")
            {
                //Debug.Log ("CarStudio.Exists (buttonInfo.Name) " + CarStudio.Exists (buttonInfo.Name));


                Texture2D img;
                if (CarStudio.Exists("后盖开启"))
                {
                    img = Resources.Load(AppData.GetCarPartData(Scene1_UI.CarSeleted, "后盖开启").Icon) as Texture2D;
                    Debug.Log("CarStudio.Exist");
                    AnimationPlay("后盖开启_playback");
                }
                else
                {
                    img = Resources.Load(AppData.GetCarPartData(Scene1_UI.CarSeleted, "后盖开启").Icon + "b") as Texture2D;
                    Debug.Log("CarStudio. not Exist");
                    CarStudio.AddPart("后盖开启");
                    AnimationPlay("后盖开启_play");
                }
                GameManager.instance.nowCustomButton.thisImage.sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0, 0));
            }
            else
            {
                Texture2D img = Resources.Load(Icon) as Texture2D;
                UIManager.instance.ShowFloatWindow(img, targetName, description, false);
            }
			UIManager.instance.ResetFloatwindowContentsize (contentHeight);
        }
    }

	public void AnimationPlay(string str)
	{
		GameObject[] parts = GameObject.FindGameObjectsWithTag("AnimPart");
		foreach (GameObject obj in parts) {
			obj.GetComponent<PartAnimation> ().SettingAnimation (str);
		}
	}
} 
