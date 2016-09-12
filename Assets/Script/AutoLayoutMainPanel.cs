using UnityEngine;
using System.Collections;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.iOS;
#endif


public class AutoLayoutMainPanel : MonoBehaviour {
    public RectTransform car1, car2, tools, meunButtons, CompanyProfile, link, about, titl;
    public UnityEngine.UI.GridLayoutGroup grid;
    public Scene1_UI sceneUI;

	public Sprite aboutSprite, serviceSprite;
	// Use this for initialization
	void Start ()
    {
        int width, height;
        width = Screen.width;
        height = Screen.height;
        int gcd = GCD(width, height);
        width = width / gcd;
        height = height / gcd;
        SixteenNine();
#if UNITY_IOS && !UNITY_EDITOR
        if (width==4&& height == 3)
        {

        }
        else if(width==16&& height == 9)
        {
            SixteenNine();
        }
        else if (Device.generation == DeviceGeneration.iPadUnknown)
        {

        }
        else
        {
            SixteenNine();
        }
#endif
#if UNITY_ANDROID

        if (width==4&& height == 3)
        {
            //SixteenNine();
        }
        else if(width==16&& height == 9)
        {
            SixteenNine();
        }
        else
        {
            SixteenNine();
        }
#endif

        sceneUI.OpenPanel(0);

    }



    /// <summary>
    /// 最大公约数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    int GCD(int a, int b)
    {
        int gcd = 1;
        int min = a > b ? b : a;
        for (int i = min; i >= 1; i--)
        {
            if (a % i == 0 && b % i == 0)
            {
                gcd = i;
                break;
            }
        }
        return gcd;
    }
    void Update()
    {
       // Debug.Log(link.rect + "    " + link.sizeDelta + "    " + link.anchoredPosition);
        //Debug.Log(CompanyProfile.sizeDelta);
        //Debug.Log(CompanyProfile.anchoredPosition);
    }

    void SixteenNine()
    {
        //car1.anchorMin = new Vector2(0.5f, 0);
        //car1.anchorMax = new Vector2(0.5f, 0);
        //car1.anchoredPosition = new Vector2(-140.5f, 36.8f);
        //car1.sizeDelta = new Vector2(281, 108);

        //car2.anchorMin = new Vector2(0.5f, 0);
        //car2.anchorMax = new Vector2(0.5f, 0);
        //car2.anchoredPosition = new Vector2(151.5f, 28.8f);
        //car2.sizeDelta = new Vector2(303, 92);

        

        titl.anchoredPosition = new Vector2(0, -26.62f);

        for (int i = 0; i < sceneUI.MenuButtons.Length; i++)
        {
			sceneUI.MenuButtons [i].transform.Find ("buttonIconAndName").GetComponent<RectTransform> ().sizeDelta = new Vector2 (62, 62);
			sceneUI.MenuButtons [i].transform.Find ("buttonIconAndName/icon").GetComponent<RectTransform> ().sizeDelta = new Vector2 (30.1f, 32.6f);
			sceneUI.MenuButtons [i].transform.Find ("buttonIconAndName/icon").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -16.3f);
			sceneUI.MenuButtons [i].transform.Find ("buttonIconAndName/Text").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 16.5f);
        }



		transform.Find ("main/buttons/button").GetComponent<RectTransform> ().sizeDelta = new Vector2 (400, 130.7f);
		transform.Find ("main/buttons/button").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-200, -21.9f);
		transform.Find ("main/buttons/button/Image").GetComponent<UnityEngine.UI.Image> ().sprite = aboutSprite;
//		transform.Find ("main/buttons/button/Text").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-200, -21.9f);
//		transform.Find ("main/buttons/button/Text (1)").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-200, -21.9f);
		transform.Find ("main/buttons/button (1)").GetComponent<RectTransform> ().sizeDelta = new Vector2 (400, 130.7f);
		transform.Find ("main/buttons/button (1)").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (200, -21.9f);
		transform.Find ("main/buttons/button (1)/Image").GetComponent<UnityEngine.UI.Image> ().sprite = serviceSprite;

		tools.anchorMin = new Vector2(0.5f, 0);
		tools.anchorMax = new Vector2(0.5f, 0);
		tools.anchoredPosition = new Vector2(0, 154.9f);
		tools.sizeDelta = new Vector2(375.9f, 48.4f);

		car1.transform.parent.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 108);

        //meunButtons.sizeDelta = new Vector2(150, 267.5f);
        //meunButtons.anchoredPosition = new Vector2(0, -174.75f);

        CompanyProfile.sizeDelta = new Vector2(800, 0);
        CompanyProfile.anchoredPosition = new Vector2(75, 0);
        
        

        grid.cellSize = new Vector2(40, 40);
        grid.spacing = new Vector2(26.96f, 0);

        link.sizeDelta = new Vector2(-150, 0);
        link.anchoredPosition = new Vector2(75, 0);

        about.sizeDelta = new Vector2(-150, 0);
        about.anchoredPosition = new Vector2(75, 0);

    }
}
