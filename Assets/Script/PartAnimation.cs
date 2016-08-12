using UnityEngine;
using System.Collections;
using DG.Tweening;
using com.ootii.Messages;

public class PartAnimation : MonoBehaviour {
	public string thisName;
	public bool isDoor;
	public bool isLeft;

	void Start()
	{
		
	}

	public void SettingAnimation(string partName)
	{

		string tmpName = partName.Split ('_')[0];
		string isBack = partName.Split ('_')[1];
		Debug.Log ("RecieveSettingAnimation " + tmpName + " , " + isBack);
		if (thisName == tmpName) {
			PlayAnimation (thisName,isBack);
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
			GameManager.instance.isDoorOpen = false;
		}

	}

	public void PlayAnimation(string partName,string isBack)
	{
		if (isBack == "play") {
			switch (partName) {
			case "电动踏板":
				{
					if (isDoor) {
						if (isLeft) {
							DoorLopen ();
						} else {
							DoorRopen ();
						}
					} else {
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
						if (isLeft) {
							EPLback ();
						} else {
							EPRback ();
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
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.1f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear);
	}

	public void EPRdown(){
		transform.localPosition = new Vector3 (transform.localPosition.x + 0.2f, transform.localPosition.y + 0.1f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.1f, transform.localPosition.y - 0.1f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPRout);
		//右踏板伸出
	}

	public void EPRback(){
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.1f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPRup);
		//右踏板收回
	}

	public void EPRup(){
		transform.localPosition = new Vector3 (transform.localPosition.x + 0.2f, transform.localPosition.y + 0.1f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.1f, transform.localPosition.y - 0.1f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (RemovePart);
	}

	public void EPLout(){
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.1f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear);
	}

	public void EPLdown(){
		transform.localPosition = new Vector3 (transform.localPosition.x - 0.2f, transform.localPosition.y + 0.1f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.1f, transform.localPosition.y - 0.1f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPLout);
		//左踏板伸出
	}

	public void EPLback(){
		transform.DOLocalMove (new Vector3 (transform.localPosition.x - 0.1f, transform.localPosition.y, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (EPLup);
		//左踏板收回
	}
		
	public void EPLup(){
		transform.localPosition = new Vector3 (transform.localPosition.x - 0.2f, transform.localPosition.y + 0.1f, transform.localPosition.z);
		transform.DOLocalMove (new Vector3 (transform.localPosition.x + 0.1f, transform.localPosition.y - 0.1f, transform.localPosition.z), 1.5f).SetEase (Ease.Linear).OnComplete (RemovePart);
	}

	public void Backopne(){
		transform.DOLocalRotate (new Vector3 (-13.0f, 0, 0), 2.5f).SetEase (Ease.InOutExpo).OnComplete (AnimationStartOver);
		//后盖开启
	}

	public void Backclose(){
		transform.DORotate (new Vector3 (0, 0, 0), 2.5f).SetEase (Ease.InOutExpo).OnComplete (AnimationStartOver);
		//后盖关闭
	}

	public void CNGinstall(){
		transform.DOMove(new Vector3(0,0,0),2.5f).SetEase (Ease.InOutExpo);
		//CNG安装
	}

	public void CNGremove(){
		transform.DOMove(new Vector3(0,0,-1.5f),2.5f).SetEase (Ease.InOutExpo);
		//CNG移除
	}

	public void DoorLopen(){
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 70.0f), 3.0f).SetEase (Ease.OutExpo).OnComplete (AnimationStartOver);
		//左门开启
	}

	public void DoorLclose(){
		//transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z + 70.0f));
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, -70.0f), 1.0f).SetEase (Ease.OutExpo).OnComplete (AnimationStartOver);
		//左门关闭

	}

	public void DoorRopen(){
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, -70.0f), 3.0f).SetEase (Ease.OutExpo).OnComplete (AnimationStartOver);
		//右门开启
	}

	public void DoorRclose(){
		
		//transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z - 70.0f));
		transform.DOLocalRotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 70.0f), 1.0f).SetEase (Ease.OutExpo).OnComplete (AnimationStartOver);
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
		MessageDispatcher.SendMessage (GameManager.instance.gameObject, "OnAnimationStartFinish", "ASFinish", 0);
	}

	void AnimationBackOver()
	{
		MessageDispatcher.SendMessage (GameManager.instance.gameObject, "OnAnimationEndFinish", "ABFinish", 0);
	}

	void RemovePart()
	{
		CarStudio.RemovePart (thisName);
	}
}
