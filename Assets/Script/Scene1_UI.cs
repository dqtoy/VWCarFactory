using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Scene1_UI : MonoBehaviour
{
    [System.Serializable]
    public class CompanyProfile_UIGroup
    {
        public GameObject topBG, content;
        public UnityEngine.UI.Image buttonBG;
    }
    [System.Serializable]
    public class ProfessionProfile_UIGroup
    {
        public Image icon;
        public Text lable;
        public GameObject arrow;
        public GameObject content;
    }
    public GameObject Loading;
    public GameObject[] UIs;
    public Image[] MenuButtons;

    public GameObject CompanyContent, ProfessionContent, linkContent, aboutContent;
    public Scrollbar CompanyScrollbar, ProfessionScrollbar;
    public CompanyProfile_UIGroup[] CompanyProfileUIs;
    public ProfessionProfile_UIGroup[] ProfessionProfileUIs;
    Color gray, blue, light_gray, white;
    public RectTransform[] viewports;
    public RectTransform link;

    public static string CarSeleted;
    // Use this for initialization
    void Awake()
    {
        gray = new Color(0.607f, 0.607f, 0.607f, 1);
        blue = new Color(0, 0.376f, 0.6901f, 1);
        light_gray = new Color(0.8f, 0.8f, 0.8f, 1);
        white = new Color(0.941f, 0.9607f, 0.96470f);
    }

    // Update is called once per frame
    void OnGUI()
    {

    }


    public void OpenCompanyDescriptionSub(int __index)
    {
        for (int i = 0; i < CompanyProfileUIs.Length; i++)
        {
            CompanyProfileUIs[i].topBG.SetActive(false);
            CompanyProfileUIs[i].buttonBG.color = Color.clear;
            CompanyProfileUIs[i].content.SetActive(false);
        }
        CompanyProfileUIs[__index].topBG.SetActive(true);
        CompanyProfileUIs[__index].buttonBG.color = white;
        CompanyProfileUIs[__index].content.SetActive(true);
        CompanyScrollbar.value = 1;
        CompanyScrollbar.transform.parent.GetComponent<ScrollRect>().content = CompanyProfileUIs[__index].content.GetComponent<RectTransform>();
    }

    public void OpenProfessionDescriptionSub(int __index)
    {
        for (int i = 0; i < ProfessionProfileUIs.Length; i++)
        {
            ProfessionProfileUIs[i].arrow.SetActive(false);
            ProfessionProfileUIs[i].icon.color = gray;
            ProfessionProfileUIs[i].lable.color = gray;
            ProfessionProfileUIs[i].content.SetActive(false);
        }

        ProfessionProfileUIs[__index].arrow.SetActive(true);
        ProfessionProfileUIs[__index].icon.color = blue;
        ProfessionProfileUIs[__index].lable.color = blue;
        ProfessionProfileUIs[__index].content.SetActive(true);
        ProfessionScrollbar.value = 1;
        ProfessionScrollbar.transform.parent.GetComponent<ScrollRect>().content = ProfessionProfileUIs[__index].content.GetComponent<RectTransform>();
        
    }

    public void OpenPanel(int __index)
    {
        for (int i = 0; i < UIs.Length; i++)
        {
            UIs[i].SetActive(false);
        }
        UIs[__index].SetActive(true);
        for (int i = 0; i < MenuButtons.Length; i++)
        {
            MenuButtons[i].color = white;
            MenuButtons[i].transform.Find("Image/Text").GetComponent<Text>().color = gray;
            MenuButtons[i].transform.Find("Image/icon").GetComponent<Image>().color = gray;

        }
        MenuButtons[__index].color = light_gray;
        MenuButtons[__index].transform.FindChild("Image/Text").GetComponent<Text>().color = blue;
        MenuButtons[__index].transform.FindChild("Image/icon").GetComponent<Image>().color = blue;
        CloseMenu();
        for (int i = 0; i < viewports.Length; i++)
        {
            viewports[i].anchorMin = Vector2.zero;
            viewports[i].anchorMax = Vector2.one;
        }
        switch (__index)
        {
            case 0:
                
                break;
            case 1:
                OpenCompanyDescriptionSub(0);
                CompanyContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                break;
            case 2:
                OpenProfessionDescriptionSub(0);
                ProfessionContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                break;
            case 3:
                linkContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
                //link.anchoredPosition = new Vector2(0, -250);
                break;
            case 4:
                aboutContent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                break;
            case 5:
                break;
            default:
                break;
        }
        
    }

    public void OpenMenu()
    {
        RectTransform _rectTransform = GetComponent<RectTransform>();
        if (Mathf.Abs((_rectTransform.anchoredPosition.x - 75)) < 5)
        {
			_rectTransform.DOAnchorPos(new Vector2(-75, 0), 0.5f).SetEase(Ease.InOutExpo);
        }
        else
        {
			_rectTransform.DOAnchorPos(new Vector2(75, 0), 0.5f).SetEase(Ease.InOutExpo);
        }
    }

    public void CloseMenu()
    {
		GetComponent<RectTransform>().DOAnchorPos(new Vector2(-75, 0), 0.5f).SetEase(Ease.InOutExpo);
    }

    public void SeletCar(string __name)
    {
        CarSeleted = __name;
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        Loading.SetActive(true);

        //StartCoroutine(LoadingScene("Main"));
    }
    IEnumerator LoadingScene(string __name)
    {
        yield return new WaitForSeconds(1);
        var _async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(__name);
        yield return _async;
    }

}
