using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using com.ootii.Messages;

public class DragDropUI : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler 
{
    public Transform subUI;

	public void OnDrag(PointerEventData eventData)
	{
        //subUI.position = transform.position;

        GetComponent<RectTransform>().pivot.Set(0,0);
        Debug.Log("transform.localPosition.y " + transform.parent.position.y + " UIManager.instance.scrollBounds.y " + UIManager.instance.scrollBounds.y);
		if (transform.parent.position.y <= 180) {//UIManager.instance.scrollBounds.y) {
			transform.parent.position = new Vector3 (transform.parent.position.x, Input.mousePosition.y, transform.parent.position.z);
		}
       // else
       // {
            //transform.position = new Vector3(transform.position.x, 180, transform.position.z);
       // }
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		UIManager.instance.isBarDraging = true;
		//transform.localScale=new Vector3(0.7f,0.7f,0.7f);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//transform.localPosition = new Vector3 (transform.localPosition.x, UIManager.instance.scrollBounds.y, transform.localPosition.z);
		//MessageDispatcher.SendMessage (UIManager.instance.gameObject,"OnDragFinish","DFinish", 0);
		UIManager.instance.OnBarDragFinish ();
		//transform.localScale=new Vector3(1f,1f,1f);
	}
}