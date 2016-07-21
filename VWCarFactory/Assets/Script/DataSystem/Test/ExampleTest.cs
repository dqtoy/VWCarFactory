using UnityEngine;
using System.Collections;

public class ExampleTest : MonoBehaviour
{
    System.Collections.Generic.List<string> CarPartName = new System.Collections.Generic.List<string> { "涂装", "电子设备", "内饰", "外饰" };
    public Rect windowRect0 = new Rect(20, 20, 300, 300);
    string selectedCar;
    int selectedPart = 0;

    void OnGUI()
    {
        //AppData.CarList 车的列表 比如途观，帕萨特
        foreach (var item in AppData.CarList)
        {
            //选择一个车款式按钮
            if (GUILayout.Button(item))
            {
                selectedCar = item;
            }
        }
        //改装界面
        if (selectedCar != null)
        {
            windowRect0 = GUI.Window(0, windowRect0, DoMyWindow, selectedCar + "改装");
        }
    }
    
    void DoMyWindow(int windowID)
    {
        //绘制4个改装菜单
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
        //绘制改装的具体内容
        foreach (var item in AppData.GetCarPartsByName(selectedCar, CarPartName[selectedPart]))
        {
            //CarPart类型 改装部位，包含配件名字Name，Icon按钮图标路径，ModelPath模型路径
            if (GUILayout.Button(item.Name))
            {

            }
        }

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}