using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public enum CarType
{
	Passat,
	Tiguan
}

public class GameManager : MonoBehaviour {

	static public GameManager instance;
	public CarType carType;
	public Transform camOutside;
	public Transform camInside;
	public GameObject car;
	public CarPrefab carPrefab;
	public int selectedCarID;
	public int selectedBG;
	public int nowSelectedCustomType;
	bool cameraIsInside;
	public bool inGoto;
	CameraControl camControl;

	public List<string> customTexturesBtn;
	public List<string> customTextures;

	public List<GameObject> allButtonIcon;
	public GameObject floorObj;
	public Material[] skyboxMats;
	public Texture[] floors;
	public Texture[] cubeMapMats;
	//public Camera c;

	public Transform[] allCamPosition;
    public float[] allCamPositionScaleVal;
	public int nowCamPositionID;
    public int nowCamScalePos;
	public bool inCameraPosition;
	public Vector3 cameraInitPosition;

	public float cameraMoveTime;
	public Ease cameraMoveEase;
	public bool isDoorOpen;

	public bool isPartButtonClick;
	public float cameraRotationY;
	public bool epDown;
	public Material[] carBodyMats;
	public Material[] glassMats;
	public bool videoIsPlaying;
	public bool videoIsSoundOff;
	public bool inAnimation;
	public bool inCNG;
	public bool inCoach;

	public GameObject[] iphones;
	public CustomButton nowCustomButton;
	public GameObject backParticle;
    public bool pedalOpen;

    public bool isInChangeColor;


    void Awake()
	{
		instance = this;
		InitData ();
		InitialCar ();
		InitCarPart ();
		CarStudio.SaveCustumUserCar(Scene1_UI.CarSeleted + "save");
	}

	// Use this for initialization
	void Start () {
		UIManager.instance.videoContent.Stop ();
		UIManager.instance.videoContent.gameObject.SetActive (false);
		camControl = Camera.main.GetComponent<CameraControl> ();
		cameraInitPosition = transform.position;
		UIManager.instance.ChangeScrollBar (false);
		if (Scene1_UI.CarSeleted == "Tiguan") {
			ChangeBGFunc (1);
			selectedCarID = 0;
		} else {
			ChangeBGFunc (2);
			selectedCarID = 1;
		}
	}

	void OnEnable()
	{
		Debug.Log ("video false");
		UIManager.instance.videoContent.Stop();
		UIManager.instance.videoContent.gameObject.SetActive (false);
	}

	void InitData()
	{
//        for (int i = 0; i < AppData.GetCarPaintingByName(AppData.CarList[GameManager.instance.selectedCarID]).Count; i++)
//        {
//            customTexturesBtn.Add(AppData.GetCarPaintingByName(AppData.CarList[GameManager.instance.selectedCarID])[i].Icon);
//            customTextures.Add(AppData.GetCarPaintingByName(AppData.CarList[GameManager.instance.selectedCarID])[i].ModelPath);//"CarBodyTexture/Passart/tex_" + (i+1));
//        }

    }

	public void InitialCar()
	{
		//CarStudio.IsInitObject = false;
		Debug.Log("Scene1_UI.CarSeleted " + Scene1_UI.CarSeleted);
		CarStudio.OpenStudio(Scene1_UI.CarSeleted);
		//car = CarStudio.objects[CarStudio.Car.CarBaseModle];
		//carPrefab = car.GetComponent<CarPrefab> ();
		//car.transform.SetParent (CarControl.instance.transform);
		//CarPartsSetting ();

		//CarStudio.LoadCustum(Scene1_UI.CarSeleted + "save");

	}

//	public void CameraChange(bool inside)
//	{
//		if (cameraIsInside != inside) {
//			
//			if (inside) {
//				//camControl.enabled = false;
//				CameraGoto (camInside);
//				cameraIsInside = true;
//			} else {
//				
//				CameraGoto (camOutside);
//				cameraIsInside = false;
//			}
//		}
//	}

	public void CameraGoto(Transform pos)
	{
        nowCamScalePos = 0;

        transform.DOMove (pos.position, 1.0f).SetEase (Ease.OutExpo).OnComplete(CameraGoFinish);
		transform.DORotate (pos.rotation.eulerAngles, 1.0f).SetEase (Ease.OutExpo);
		inGoto = true;
//		if (cameraIsInside) {
//			//camControl.enabled = true;
//			car.transform.DORotate (new Vector3 (0, -90, 0), 1.0f).SetEase (Ease.OutExpo);
//		} else {
//			car.transform.DORotate (new Vector3 (0, 0, 0), 1.0f).SetEase (Ease.OutExpo);
//		}
	}

	public void CameraGoBack()
	{
		if (GameManager.instance.inCameraPosition) {
			GameManager.instance.inCameraPosition = false;
            CarControl.instance.camTarget = CarControl.instance.transform;
            UIManager.instance.HideOtherFloatButton();
            Camera.main.transform.DOMove (GameManager.instance.cameraInitPosition, GameManager.instance.cameraMoveTime).SetEase(GameManager.instance.cameraMoveEase);
			CloseDoor ();
			CloseBackDoor ();
			ResetAnimationObj ();
			backParticle.SetActive (false);
			UIManager.instance.float3DButton.gameObject.SetActive (false);
		}
	}

	public void CloseDoor()
	{
		if (GameManager.instance.isDoorOpen == true) {
			GameObject[] parts = GameObject.FindGameObjectsWithTag("AnimPart");
			foreach (GameObject obj in parts) {
				obj.GetComponent<PartAnimation> ().DoorClose();
			}
			//GameManager.instance.isDoorOpen = false;
		}
	}

	public void CameraGoFinish()
	{
		inGoto = false;
		if (inCameraPosition) {
			cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
			CarControl.instance.camInPos = Camera.main.transform.localPosition;
		}
		//camControl.enabled = true;
	}

	public void ChangeCustomTexture(int id)
	{
		//carPrefab.bodyRenderer.material.mainTexture = Resources.Load(customTextures [id]) as Texture;
	}

	public void CarPartsSetting()
	{
		for (int i = 0; i < CarStudio.Car.Parts.Count; i++) {
			//CarStudio.objects [CarStudio.Car.Parts[i]].name = "testPartName_" + i;
		}
	}

	/*
	public void SaveImage()
	{
		Rect rect = new Rect (Screen.width*0f, Screen.height*0f, Screen.width*1f, Screen.height*1f);
		// 创建一个RenderTexture对象  
		RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);  
		// 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
		Camera.main.targetTexture = rt;  
		Camera.main.Render();  
		//ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
		c.targetTexture = rt;  
		c.Render();  
		//ps: -------------------------------------------------------------------  

		// 激活这个rt, 并从中中读取像素。  
		RenderTexture.active = rt;  
		Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false);  
		screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
		screenShot.Apply();  

		// 重置相关参数，以使用camera继续在屏幕上显示  
		Camera.main.targetTexture = null; 
		c.targetTexture = null;
		//ps: camera2.targetTexture = null;  
		RenderTexture.active = null; // JC: added to avoid errors  
		GameObject.Destroy(rt);  
		// 最后将这些纹理数据，成一个png图片文件  
//		byte[] bytes = screenShot.EncodeToPNG();  
//		string filename = Application.dataPath + "/Screenshot.png";  
//		System.IO.File.WriteAllBytes(filename, bytes);  
		//Debug.Log(string.Format("截屏了一张照片: {0}", filename));  
		screenShot.SaveToAlbum();
	}
	*/

	public void SaveImage()
	{
		UIManager.instance.gameObject.SetActive (false);
		StartCoroutine (SaveImageEnd ());
	}
		
	IEnumerator SaveImageEnd()
	{
		yield return new WaitForEndOfFrame ();
		Rect rect = new Rect (Screen.width*0f, Screen.height*0f, Screen.width*1f, Screen.height*1f);
		// 先创建一个的空纹理，大小可根据实现需要来设置  
		Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false);  

		// 读取屏幕像素信息并存储为纹理数据，  
		screenShot.ReadPixels(rect, 0, 0); 
		screenShot.Apply(); 

		// 然后将这些纹理数据，成一个png图片文件  
		//		byte[] bytes = screenShot.EncodeToPNG();  
		//		string filename = Application.dataPath + "/Screenshot.png";  
		//		System.IO.File.WriteAllBytes(filename, bytes);  
		//		Debug.Log(string.Format("截屏了一张图片: {0}", filename));  

		screenShot.SaveToAlbum();
		UIManager.instance.gameObject.SetActive (true); 
	}

	public void ChangeBGFunc(int id)
	{
		RenderSettings.skybox = skyboxMats [id];
		floorObj.GetComponent<Renderer> ().material.mainTexture = floors [id];
		foreach (Material item in carBodyMats) {
			item.SetTexture("_ReflectionMap",cubeMapMats[id]);
		}
		foreach (Material item in glassMats) {
			item.SetTexture("_Cube",cubeMapMats[id]);
		}
		selectedBG = id;
		if (Scene1_UI.CarSeleted == "Passat") {
			DoorColorChange.instance.ChangeColor ();
		}
	}

	void InitCarPart()
	{
//		if (CarStudio.Exists("电动踏板")) {
//			CarStudio.RemovePart ("电动踏板");
//		}
		if (CarStudio.Exists("后盖开启")) {
			CarStudio.RemovePart ("后盖开启");
		}
	}

	public void ShowIphone(bool bo,int id)
	{
		iphones [id].SetActive (bo);
		if (bo) {
			iphones [id].transform.DOLocalMoveX ( - 0.065f, 0.5f).SetEase (Ease.OutQuad);
		} else {
			iphones [id].transform.DOLocalMoveX (0, 0.5f).SetEase (Ease.OutQuad).OnComplete(iphoneHideComplete);
		}
	}

	public void iphoneHideComplete()
	{
		foreach (GameObject item in iphones) {
			item.SetActive (false);
		}
	}

	public void CloseBackDoor()
	{
		Texture2D img;
		if (CarStudio.Exists ("后盖开启")) {
			CarStudio.RemovePart ("后盖开启");
			img = Resources.Load (nowCustomButton.thisButton.Icon) as Texture2D;
			GameManager.instance.nowCustomButton.thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
		}
	}

	public void ResetAnimationObj()
	{
		GameObject[] parts = GameObject.FindGameObjectsWithTag("AnimPart");
		foreach (GameObject obj in parts) {
			obj.GetComponent<PartAnimation> ().inAnimation = false;
		}
	}

	public void SetCNGRenderer(bool bo)
	{
		MeshRenderer[] cngRenderers = GameObject.Find ("CNG").GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer item in cngRenderers) {
			item.enabled = bo;
		}
	}
}
