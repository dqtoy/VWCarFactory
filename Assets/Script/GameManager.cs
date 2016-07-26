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
	public int nowSelectedCustomType;
	bool cameraIsInside;

	public List<string> customTexturesBtn;
	public List<string> customTextures;

	public List<GameObject> allButtonIcon;

	void Awake()
	{
		instance = this;
		InitData ();
	}

	// Use this for initialization
	void Start () {
		InitialCar ();
	}

	void InitData()
	{
		for (int i = 0; i < AppData.GetCarPaintingByName (AppData.CarList [GameManager.instance.selectedCarID]).Count; i++) {
			customTexturesBtn.Add (AppData.GetCarPaintingByName (AppData.CarList [GameManager.instance.selectedCarID])[i].Icon);
			customTextures.Add (AppData.GetCarPaintingByName (AppData.CarList [GameManager.instance.selectedCarID])[i].ModelPath);//"CarBodyTexture/Passart/tex_" + (i+1));
		}

	}

	public void InitialCar()
	{
		//CarStudio.IsInitObject = false;
		CarStudio.OpenStudio(carType.ToString ());
		CarStudio.LoadCustum (carType.ToString ());
		CarStudio.LoadTemplate (AppData.GetTemplateCarList(carType.ToString())[0]);
		car = CarStudio.objects[CarStudio.Car.CarBaseModle];
		//car = Instantiate (Resources.Load ("CarBody/" + carType.ToString ()), Vector3.zero, Quaternion.identity) as GameObject;
		carPrefab = car.GetComponent<CarPrefab> ();
		car.transform.SetParent (CarControl.instance.transform);
		car.transform.localPosition = Vector3.zero;
		car.transform.localRotation = Quaternion.Euler (0,90,0);
		car.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

		CarPartsSetting ();
	}

	public void CameraChange(bool inside)
	{
		if (cameraIsInside != inside) {
			if (inside) {
				CameraGoto (camInside);
				cameraIsInside = true;
			} else {
				CameraGoto (camOutside);
				cameraIsInside = false;
			}
		}
	}

	public void CameraGoto(Transform pos)
	{
		transform.DOMove (pos.position, 1.0f).SetEase (Ease.OutExpo);
		transform.DORotate (pos.rotation.eulerAngles, 1.0f).SetEase (Ease.OutExpo);
		if (cameraIsInside) {
			car.transform.DORotate (new Vector3 (0, -90, 0), 1.0f).SetEase (Ease.OutExpo);
		} else {
			car.transform.DORotate (new Vector3 (0, 0, 0), 1.0f).SetEase (Ease.OutExpo);
		}
	}

	public void ChangeCustomTexture(int id)
	{
		carPrefab.bodyRenderer.material.mainTexture = Resources.Load(customTextures [id]) as Texture;
	}

	public void CarPartsSetting()
	{
		for (int i = 0; i < CarStudio.Car.Parts.Count; i++) {
			CarStudio.objects [CarStudio.Car.Parts[i]].name = "testPartName_" + i;
		}
	}
}
