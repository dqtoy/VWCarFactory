using UnityEngine;
using System.Collections;
using DG.Tweening;
//using com.ootii.Messages;

public class PartAnimation : MonoBehaviour {
	public string thisName;
	public bool isDoor;
	public bool isLeft;
	Vector3 orgPosition;
	Vector3 orgRotation;
	string nowBack;
	bool inAnimation;
	float epMoveValue;

	void Start()
	{
		epMoveValue = 0.05f;
		orgPosition = transform.localPosition;
		orgRotation = transform.localRotation.eulerAngles;
//		if (thisName == "电动踏板" && !isDoor && GameManager.instance.isPartButtonClick) {
//			if (isLeft) {
//				EPLdown ();
//			} else {
//				EPRdown ();
//			}
//		}
	}

	public void SettingAnimation(string partName)
	{
		//Debug.Log ("PlayAnimation " + gameObject.name + " tmpName " + partName);
		if (inAnimation == false) {
			
			string tmpName = partName.Split ('_')[0];
			string isBack = partName.Split ('_')[1];
			nowBack = isBack;
			//Debug.Log ("RecieveSettingAnimation " + tmpName + " , " + isBack);

			if (thisName == tmpName) {
				PlayAnimation (thisName,isBack);
			}
		}
	}

	public void DoorClose()
	{
		if (isDoor) {
			if (isLeft) {
				DoorLclose ();
			} else {
				DoorRclose ();
			}
			//GameManager.instance.isDoorOpen = false;
		}
		else if (thisName == "电动踏板") {
			GameManager.instance.epDown = false;
			if (isLeft) {
				EPLback (false);
			} else {
				EPRback (false);
			}
			//Debug.Log ("epBack");
		}
	}

	public void PlayAnimation(string partName,string isBack)
	{
		if (isBack == "play") {
			switch (partName) {
			case "电动踏板":
				{
					if (isDoor) {
						if (isLeft && !GameManager.instance.isDoorOpen) {
							DoorLopen ();
						} else if(!GameManager.instance.isDoorOpen) {
							DoorRopen ();
						}
					} else {
						GameManager.instance.epDown = true;
						if (isLeft) {
							EPLdown ();
						} else {
							EPRdown ();
						}
					}
				}
				break;
			case "后盖开启":
				Backopne ();
				break;
			case "CNG":
				CNGinstall ();
				break;
			default:
				break;
			}
		} else {
			switch (partName) {
			case "电动踏板":
				{
					if (isDoor) {
						if (isLeft && GameManager.instance.isDoorOpen) {
							DoorLclose ();
						} else if(GameManager.instance.isDoorOpen){
							DoorRclose ();
						}
					} else {
						Debug.Log ("epBack");
						GameManager.instance.epDown = false;
						if (isLeft) {
							
							EPLback (true);
						} else {
							EPRback (true);
						}
					}
				}
				break;
			case "后盖开启":
				Backclose ();
				break;
			case "CNG":
				CNGremove ();
				break;
			default:
				break;
			}
		}
	}

	public void EPRout(){
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.05f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete(AnimationStartOver);
	}

	public void EPRdown(){
		inAnimation = true;
		//transform.localPosition = new Vector3 (transform.localPosition.x - 0.05f, transform.localPosition.y + 0.05f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.05f, transform.localPosition.y - 0.03f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPRout);
		//右踏板伸出
	}

	public void EPRback(bool isRemove){
		inAnimation = true;
		if (isRemove) {
			transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.05f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPRup);
		} else {
			transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.05f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPRupDontRemove);
		}
		//右踏板收回
	}

	public void EPRup(){
		//transform.localPosition = new Vector3 (transform.localPosition.x + 0.1f, transform.localPosition.y - 0.05f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.05f, transform.localPosition.y + 0.03f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (RemovePart );
	}

	public void EPRupDontRemove()
	{
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.05f, transform.localPosition.y + 0.03f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete(AnimationBackOver);
	}

	public void EPLout(){
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.05f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete(AnimationStartOver);
	}

	public void EPLdown(){
		inAnimation = true;
		//transform.localPosition = new Vector3 (transform.localPosition.x + 0.1f, transform.localPosition.y + 0.05f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.05f, transform.localPosition.y - 0.03f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPLout);
		//左踏板伸出
	}

	public void EPLback(bool isRemove){
		inAnimation = true;
		if (isRemove) {
			transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.05f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPLup);
		} else {
			transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.05f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPLupDontRemove);

		}
		//左踏板收回
	}
		
	public void EPLup(){
		//transform.localPosition = new Vector3 (transform.localPosition.x - 0.1f, transform.localPosition.y - 0.05f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.05f, transform.localPosition.y + 0.03f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (RemovePart);
	}

	public void EPLupDontRemove()
	{
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.05f, transform.localPosition.y + 0.03f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete(AnimationBackOver);
	}

	public void Backopne(){
		inAnimation = true;
		transform.DOLocalRotate (new Vector3 (transform.localRotation.x , transform.localRotation.y, transform.localRotation.z), 2.5f).SetEase (Ease.InOutExpo).OnComplete (AnimationStartOver);
		//后盖开启
	}

	public void Backclose(){
		inAnimation = true;
		Debug.Log ("关闭 " + gameObject.name);
		//transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.x +12.0f, transform.localRotation.y, transform.localRotation.z));
		transform.DOLocalRotate (new Vector3 (transform.localRotation.x -90.0f, transform.localRotation.y, transform.localRotation.z), 2.5f).SetEase (Ease.InOutExpo).OnComplete (RemovePart);
		//后盖关闭
	}

	public void CNGinstall(){
		inAnimation = true;
		transform.DOMove(new Vector3(0,0,0),2.5f).SetEase (Ease.InOutExpo);
		//CNG安装
	}

	public void CNGremove(){
		inAnimation = true;
		transform.DOMove(new Vector3(0,0,-1.5f),2.5f).SetEase (Ease.InOutExpo);
		//CNG移除
	}

	public void DoorLopen(){
		inAnimation = true;
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 70.0f), 3.0f).SetEase (Ease.OutExpo).OnComplete (AnimationStartOver);
		//左门开启
	}

	public void DoorLclose(){
		inAnimation = true;
		//transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + 70.0f));
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, -70.0f), 1.0f).SetEase (Ease.OutExpo).OnComplete (AnimationBackOver);
		//左门关闭
	}

	public void DoorRopen(){
		inAnimation = true;
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, -70.0f), 3.0f).SetEase (Ease.OutExpo).OnComplete (AnimationStartOver);
		//右门开启
	}

	public void DoorRclose(){
		inAnimation = true;
		//transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z - 70.0f));
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 70.0f), 1.0f).SetEase (Ease.OutExpo).OnComplete (AnimationBackOver);
		//右门关闭
	}

	public void AnimationStart()
	{
		transform.DOLocalRotate (new Vector3 (0, 90.0f, 0), 1.5f).SetEase (Ease.InOutExpo).OnComplete (AnimationStartOver);
		transform.DOLocalMove(new Vector3(0,0,0),0.5f).SetEase (Ease.InOutExpo);
	}

	public void AnimationBack()
	{
		transform.DOLocalRotate (new Vector3 (0, 90.0f, 0), 1.5f).SetEase (Ease.InOutExpo).OnComplete (AnimationStartOver);
	}

	void AnimationStartOver()
	{
		//MessageDispatcher.SendMessage (GameManager.instance.gameObject, "OnAnimationStartFinish", "ASFinish", 0);
		if (isDoor) {
			GameManager.instance.isDoorOpen = true;
		}
		Debug.Log ("startOver");
		inAnimation = false;
	}

	void AnimationBackOver()
	{
		//MessageDispatcher.SendMessage (GameManager.instance.gameObject, "OnAnimationEndFinish", "ABFinish", 0);
		if (isDoor) {
			GameManager.instance.isDoorOpen = false;
		}
		inAnimation = false;
	}

	void RemovePart()
	{
		AnimationStartOver ();
		CarStudio.RemovePart (thisName);
	}
}
