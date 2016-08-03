using UnityEngine;
using System.Collections.Generic;


public class Test2 : MonoBehaviour
{
    

    void Start()
    {

    }

    void Update()
    {

    }
    bool isShowStudio; Rect windowStudio = new Rect(20, 20, 800, 800);
    bool isShowColors; Rect windowColors = new Rect(20, 20, 500, 500);
    bool isShowDescription; Rect windowDescription = new Rect(800, 50, 300, 50);

    IButtonInfo DescriptionButton;

    void OnGUI()
    {
        if (GUILayout.Button("Tiguan"))
        {
            isShowStudio = true;
            CarStudio.OpenStudio("Tiguan");
            DescriptionButton = AppData.GetCarDataByName("Tiguan");
        }
        if (isShowStudio)
        {
            windowStudio = GUI.Window(0, windowStudio, CarStudioWindow, "改装");
        }
        if (isShowColors)
        {
            windowColors = GUI.Window(1, windowColors, ShowColors, "色版");
        }

        windowDescription = GUI.Window(2, windowDescription, ShowDescriptionButton, "案例");

    }


    int selectedYeWu;
    List<string> YewuLieBiao = new List<string> { "个性化设计业务", "特种车业务", "CNG业务", "涂装业务" };
    void CarStudioWindow(int windowID)
    {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < YewuLieBiao.Count; i++)
        {
            if (selectedYeWu == i)
            {
                GUILayout.Toggle(true, YewuLieBiao[i]);
            }
            else
            {
                if (GUILayout.Toggle(false, YewuLieBiao[i]))
                {
                    if (i == 0)
                    {
                        CarStudio.LoadCustum("save");
                    }
                    if (selectedYeWu == 0 && i != 0)
                    {
                        CarStudio.SaveCustumUserCar("save");
                    }
                    selectedYeWu = i;
                    
                }
            }
        }
        GUILayout.EndHorizontal();


        switch (selectedYeWu)
        {
            case 0:
                ShenJiYewu();
                break;
            case 1:
                TeZhongCheYeWu();
                break;
            case 2:
                break;
            case 3:
                ShowPaintingCar();
                break;
            default:
                break;
        }


        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    /// <summary>
    /// 升级业务
    /// </summary>
    void ShenJiYewu()
    {
        var _parts = AppData.GetCarAllParts(CarStudio.Car.CarBaseModle);
        foreach (var _part in _parts)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(_part.Key);
            foreach (var button in _parts[_part.Key])
            {
                GUILayout.BeginVertical();

                GUILayout.Label(button.Name);
                if( GUILayout.Button(Resources.Load<Texture2D>(button.Icon), GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
                {
                    DescriptionButton = button;
                    if (button.Name == "变色模块")
                    {
                        isShowColors = !isShowColors;
                    }

                    if (CarStudio.Exists(button.Name))
                    {
                        CarStudio.RemovePart(button.Name);
                    }
                    else
                    {
                        CarStudio.AddPart(button.Name);
                    }
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();

        }
        //if (GUILayout.Button("save"))
        //{
        //    CarStudio.SaveCustumUserCar("save");
        //}
    }

    void ShowColors(int windowID)
    {
        var button = AppData.GetCarColorsByName(CarStudio.Car.CarBaseModle);
        GUILayout.BeginHorizontal();

        for (int i = 0; i < button.Count; i++)
        {
            GUILayout.BeginVertical();

            GUILayout.Label(button[i].Name);
            if(GUILayout.Button(Resources.Load<Texture2D>(button[i].Icon), GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
            {
                DescriptionButton = button[i];
                if (CarStudio.Exists(button[i].Name))
                {
                    CarStudio.RemovePart(button[i].Name);
                }
                else
                {
                    CarStudio.AddPart(button[i].Name);
                }
            }

            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));

    }

    void TeZhongCheYeWu()
    {
        var _cars = AppData.GetSpecialTemplateCarList(CarStudio.Car.CarBaseModle);
        foreach (var _car in _cars)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(_car.Key);
            foreach (var button in _cars[_car.Key])
            {
                GUILayout.BeginVertical();

                GUILayout.Label(button.Name);
                if (GUILayout.Button(Resources.Load<Texture2D>(button.Icon), GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
                {
                    DescriptionButton = button;
                    CarStudio.LoadTemplate(button.Description);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }

    void ShowDescriptionButton(int windowID)
    {
        GUILayout.BeginHorizontal();
        if(DescriptionButton != null)
        {
            if (!string.IsNullOrEmpty(DescriptionButton.PdfDescription))
            {
                if (GUILayout.Button("PDF"))
                    Debug.Log(DescriptionButton.PdfDescription);
            }
            if (DescriptionButton.TextureDescription!=null)
            {
                if (GUILayout.Button("图片案例"))
                {
                    foreach (var item in DescriptionButton.TextureDescription)
                    {
                        Debug.Log(AppData.GetSamples(item).Asset);
                    }
                }
            }
            if (DescriptionButton.MovieDescription != null)
            {
                if (GUILayout.Button("电影案例"))
                {
                    foreach (var item in DescriptionButton.MovieDescription)
                    {
                        Debug.Log(AppData.GetSamples(item).Asset);
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));

    }

    void ShowPaintingCar()
    {
        var _cars = AppData.GetPaintingTemplateByName(CarStudio.Car.CarBaseModle);
        foreach (var _car in _cars)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(_car.Key);
            foreach (var button in _cars[_car.Key])
            {
                GUILayout.BeginVertical();

                GUILayout.Label(button.Name);
                if (GUILayout.Button(Resources.Load<Texture2D>(button.Icon), GUILayout.MaxHeight(50), GUILayout.MaxWidth(50)))
                {
                    DescriptionButton = button;
                    CarStudio.LoadTemplate(button.Description);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}