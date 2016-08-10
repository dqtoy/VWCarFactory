using UnityEngine;
using System.Collections;
using DG.Tweening;
using com.ootii.Messages;

public class PartAnimation : MonoBehaviour {

	void Start()
	{
		AnimationStart ();
	}
	
	public void AnimationStart()
	{
		//transform.DOLocalRotate (new Vector3 (0, 90.0f, 0), 1.5f).SetEase (Ease.InOutExpo).OnComplete (AnimationStartOver);
		//transform.DOLocalMove(new Vector3(0,0,0),0.5f).SetEase (Ease.InOutExpo);
	}

	public void AnimationBack()
	{
		//transform.DOLocalRotate (new Vector3 (0, 90.0f, 0), 1.5f).SetEase (Ease.InOutExpo).OnComplete (AnimationStartOver);
	}

	void AnimationStartOver()
	{
		MessageDispatcher.SendMessage (GameManager.instance.gameObject, "OnAnimationStartFinish", "ASFinish", 0);
	}

	void AnimationBackOver()
	{
		MessageDispatcher.SendMessage (GameManager.instance.gameObject, "OnAnimationEndFinish", "ABFinish", 0);
	}
}
