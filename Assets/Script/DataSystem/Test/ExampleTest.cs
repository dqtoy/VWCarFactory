using UnityEngine;
using System.Collections;

public class ExampleTest : MonoBehaviour
{
	System.Collections.Generic.List<string> CarPartName = new System.Collections.Generic.List<string> { "内饰", "外饰", "电子设备","涂装" };
    Rect windowRect0 = new Rect(20, 20, 300, 300);
    Rect windowRect1 = new Rect(50, 50, 800, 800);
    Rect windowRect2 = new Rect(100, 100, 300, 300);
    Rect windowRect3 = new Rect(150, 150, 300, 300);


    string selectedCar;
    int selectedPart = 0;
    //是否显示案例图片，用户自定义，内置模板车
    bool isShowSamples, isShowUserCars, isShowTemplate;
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
       
        foreach (var item in AppData.CarList) //AppData.CarList 车的列表 比如途观，帕萨特
        {
            //选择一个车款式按钮
            if (GUILayout.Button(item))
            {
                selectedCar = item;
                CarStudio.OpenStudio(item);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);

        if (GUILayout.Button("用户自定义车列表"))   //显示用户自定义的车的按钮
        {
            isShowUserCars = true;
        }

        //显示各种界面窗口
        if (selectedCar != null)
        {
            windowRect0 = GUI.Window(0, windowRect0, CarStudioWindow, selectedCar + "改装");
        }
        if (isShowSamples)//案例模板
        {
            windowRect1 = GUI.Window(1, windowRect1, SampleWindow,  "案例模板");
        }
        if (isShowUserCars)//用户车列表
        {
            windowRect2 = GUI.Window(2, windowRect2, UserCarWindow, "自定义车辆列表");
        }
        if (isShowTemplate)
        {
            windowRect3 = GUI.Window(3, windowRect3, TemplateList, "模板车辆列表");
        }
    }
    
    //改装界面窗口
    void CarStudioWindow(int windowID)
    {
        //绘制案例,模板 按钮
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("案例图片"))
        {
            isShowSamples = true;
        }
        if (GUILayout.Button("模板车辆"))
        {
            isShowTemplate = true;
        }
        GUILayout.EndHorizontal();
        

        GUILayout.BeginHorizontal();
        //绘制4个改装菜单  按钮
        for (int i = 0; i < CarPartName.Count; i++)
        {

            if (selectedPart == i)
            {
                GUILayout.Toggle(true, CarPartName[i]);
            }
            else
            {
                if (GUILayout.Toggle(false, CarPartName[i]))
                {
                    selectedPart = i;
                }
            }
        }
        GUILayout.EndHorizontal();
		//Debug.Log (CarStudio.Car.CarBaseModle + " , " + CarPartName[selectedPart]);
        //绘制改装的具体内容 按钮
        foreach (var item in AppData.GetCarPartsByName(CarStudio.Car.CarBaseModle, CarPartName[selectedPart]))
        {
            //CarPart类型 改装部位，包含配件名字Name，Icon按钮图标路径，ModelPath模型路径
            if (GUILayout.Button(item.Name))
            {
                //判断是否存在某个车配件 添加删除 配件   Exists是否存在  RemovePart删除组件  AddPart添加组件
                if (CarStudio.Exists(item.Name))
                {
                    CarStudio.RemovePart(item.Name);            
                }
                else
                {
                    CarStudio.AddPart(item.Name);
                }
            }
        }

        

        GUILayout.Space(30);
        //保存配置按钮
        if (GUILayout.Button("保存"))
        {
            //保存配置  这里是基于时间保存，你可以根据玩家输入的名字保存
            CarStudio.SaveCustumUserCar(System.DateTime.Now.ToString("yy-MM-dd-HH-mm-ss"));
        }
        if (GUILayout.Button("close"))
        {
            CarStudio.CloseStudio();

            selectedCar = null;
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    int sampleSelected = 0;
    void SampleWindow(int windowID)
    {
        var _samples = AppData.GetCarSamples(CarStudio.Car.CarType);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("上一页"))
        {
            sampleSelected = sampleSelected - 1 < 0 ? 0 : sampleSelected - 1;
        }
        if (GUILayout.Button("下一页"))
        {
            sampleSelected = sampleSelected + 1 > (_samples.Count - 1) ? (_samples.Count - 1) : sampleSelected + 1;
        }
        GUILayout.EndHorizontal();
        GUILayout.Label(_samples[sampleSelected].Title);
        GUILayout.Label(_samples[sampleSelected].Tag);
        Texture2D tex = Resources.Load<Texture2D>(_samples[sampleSelected].Image);
        GUILayout.Label(tex);
        GUILayout.Label(_samples[sampleSelected].Description);



        GUILayout.Space(30);
        if (GUILayout.Button("close"))
        {
            isShowSamples = false;
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));

    }

    void UserCarWindow(int windowId)
    {
        foreach (var item in AppData.GetUserCustumCarsList())
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(item))
            {
                CarStudio.LoadCustum(item);
                selectedCar = item;
            }
            if (GUILayout.Button("Delete"))
            {
                AppData.DeleteCustumCar(item);
            }

            GUILayout.EndHorizontal();

        }


        GUILayout.Space(30);
        if (GUILayout.Button("close"))
        {
            isShowUserCars = false;
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));

    }

    void TemplateList(int windowId)
    {
        
        foreach (var item in AppData.GetTemplateCarList(CarStudio.Car.CarBaseModle))
        {
            if (GUILayout.Button(item))
            {
                CarStudio.LoadTemplate(item);
                isShowTemplate = false;
                selectedCar = item;
            }
        }

        GUILayout.Space(30);
        if (GUILayout.Button("close"))
        {
            isShowTemplate = false;
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}