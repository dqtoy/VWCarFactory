using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using com.ootii.Messages;

public class Touch3DUIManager : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler 
{

	public void OnDrag(PointerEventData eventData)
	{
		//MessageDispatcher.SendMessage (CarControl.instance.gameObject,"OnDraging","DDrag", 0);
		CarControl.instance.OnDrag ();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//MessageDispatcher.SendMessage (CarControl.instance.gameObject,"OnDragStart","DStart", 0);
		CarControl.instance.OnDown ();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//MessageDispatcher.SendMessage (CarControl.instance.gameObject,"OnDragFinish","DFinish", 0);
		CarControl.instance.OnUp ();
	}
}