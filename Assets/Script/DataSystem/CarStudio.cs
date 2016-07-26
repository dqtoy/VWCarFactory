using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CarStudio
{
    public static CustumCar Car { get { return car; } }
    public static bool IsInitObject = true;
    static CustumCar car;
	static public Dictionary<string, GameObject> objects;

    static CarStudio()
    {
        objects = new Dictionary<string, GameObject>();
    }

    #region 公有函数
    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="__part"></param>
    /// <returns></returns>
    public static bool AddPart(CarPart __part)
    {
        return AddPart(__part.Name);
    }
    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="__partName">组件名字</param>
    /// <returns></returns>
    public static bool AddPart(string __partName)
    {
        CarPart _part = AppData.GetCarPartData(Car.CarBaseModle, __partName);
        if (_part == null)
        {
            Debug.Log("找不到指定组件");
            return false;
        }
        if (!car)
        {
            Debug.Log("数据Car不能为空");
            return false;
        }
        if (AppData.GetCarPaintingByName(car.CarBaseModle).Exists((_painting) => _painting.Name == _part.Name))
        {
            car.BodyTexture = _part.Name;
            InitObject(_part.Name, _part.ModelPath);
            return true;
        }
        else if (AppData.GetCarDataByName(car.CarBaseModle).CustumParts.Exists((_temp) => _temp.Name == _part.Name))
        {
            if (car.Parts.Exists((_s) => _s == _part.Name))
            {
                Debug.Log("配件已存在");
                return false;
            }
            else
            {
                car.Parts.Add(_part.Name);
                InitObject(_part.Name, _part.ModelPath);
                return true;
            }
        }
        else
        {
            Debug.Log("该车型不存在" + _part.Name + "的组件");
            return false;
        }
        
    }

    /// <summary>
    /// 移除指定的组件
    /// </summary>
    /// <param name="__partName"></param>
    /// <returns></returns>
    public static bool RemovePart(string __partName)
    {
        CarPart _part = AppData.GetCarPartData(car.CarBaseModle, __partName);
        if (_part == null)
        {
            Debug.Log("找不到指定组件");
            return false;
        }
        if (!car)
        {
            Debug.Log("数据Car不能为空");
            return false;
        }
        if (car.BodyTexture == _part.Name)
        {
            InitObject(car.CarBaseModle, _part.ModelPath);
            car.BodyTexture = CustumCar.DefaultTexture;
            return true;
        }
        if (car.Parts.Exists((_s) =>
                                {
                                    if (_s == _part.Name)
                                    {
                                        DeleteModel(_s);
                                        car.Parts.Remove(_s);
                                        return true;
                                    }
                                    return false;
                                }))
        {
            return true;
        }
        else
        {
            Debug.Log("指定组件不存在");
            return false;
        }
    }

    /// <summary>
    /// 移除指定的组件
    /// </summary>
    /// <param name="__part"></param>
    /// <returns></returns>
    public static bool RemovePart(CarPart __part)
    {
        return RemovePart(__part.Name);
    }

    /// <summary>
    /// 是否存在某个组件
    /// </summary>
    /// <param name="__partName"></param>
    /// <returns></returns>
    public static bool Exists(string __partName)
    {
        if (car.BodyTexture == __partName)
        {
            return true;
        }
        if (car.Parts.Contains(__partName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="__name">自定义车名</param>
    /// <returns></returns>
    public static bool SaveCustumUserCar(string __name)
    {
        if (string.IsNullOrEmpty(car.CarBaseModle))
        {
            Debug.Log(car.CarBaseModle + "不能为空");
            return false;
        }
        if (string.IsNullOrEmpty(car.BodyTexture))
        {
            Debug.Log(car.BodyTexture + "不能为空");
            return false;
        }
        car.CustumCarName = __name;
        string _filePath = CustumCar.Path + "/" + __name + ".json";
        if (File.Exists(_filePath))
        {
            Debug.Log("指定名字的自定义车已存在，尝试换个名字");
            return false;
        }
        else
        {
            File.WriteAllText(_filePath, LitJson.JsonMapper.ToJson(car));
            return true;
        }
    }

    /// <summary>
    /// 读取自定义车数据
    /// </summary>
    /// <param name="__fileName"></param>
    /// <returns></returns>
    public static void LoadCustum(string __custumCarName)
    {
        string _fileName = CustumCar.Path + "/" + __custumCarName + ".json";
        if (AppData.GetUserCustumCarsList().Contains(__custumCarName) && File.Exists(_fileName))
        {
            car = LitJson.JsonMapper.ToObject<CustumCar>(File.ReadAllText(_fileName));
            InitCar();
        }
        else
        {
            Debug.Log("指定自定义车数据不存在");
        }
    }

    /// <summary>
    /// 读取模板车数据
    /// </summary>
    /// <param name="__fileName"></param>
    /// <returns></returns>
    public static void LoadTemplate(string __TemplateCarName)
    {
        
        string _fileName = Application.streamingAssetsPath + "/Data/Template/" + __TemplateCarName + ".json";
        if (File.Exists(_fileName))
        {
            car = LitJson.JsonMapper.ToObject<CustumCar>(File.ReadAllText(_fileName));
            InitCar();
        }
        else
        {
            Debug.Log("指定模板数据不存在");
        }
    }

    /// <summary>
    /// 创建一个空的Car
    /// </summary>
    /// <param name="__carType"></param>
    public static void OpenStudio(string __carName)
    {
        car = new CustumCar(__carName);
        InitCar();
    }

    public static void CloseStudio()
    {
        DeleteAllModel();
        car = null;
    }

    /// <summary>
    /// 获取所有的用户自定义车名字
    /// </summary>
    /// <returns></returns>
    public static List<string> GetUserCustumCarsList()
    {
        return AppData.GetUserCustumCarsList();
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="__name"></param>
    public static void DeleteCustumCar(string __name)
    {
        AppData.DeleteCustumCar(__name);
    }
    #endregion

    /// <summary>
    /// 生成模型
    /// </summary>
    /// <param name="__name">模型名字</param>
    /// <param name="__path">模型路径</param>
    static void InitObject(string __name, string __path)
    {
        if (IsInitObject)
        {
            object _asset = Resources.Load(__path);
            if (_asset is GameObject)
            {
                GameObject _obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(__path));
                objects.Add(__name, _obj);
            }
            else
            {
                Texture2D _tex = _asset as Texture2D;
				GameObject _carBody = objects[car.CarBaseModle];
				_carBody.GetComponent<CarPrefab>().bodyRenderer.material.mainTexture = _tex;
            }

            
        }
    }
    /// <summary>
    /// 删除模型
    /// </summary>
    /// <param name="__name"></param>
    static void DeleteModel(string __name)
    {
        if (IsInitObject)
        {
            GameObject.Destroy(objects[__name]);
            objects.Remove(__name);
        }
    }

    static void InitCar()
    {
        if (IsInitObject)
        {
            DeleteAllModel();
            CarData _data = AppData.GetCarDataByName(car.CarBaseModle);
            InitObject(car.CarBaseModle, _data.CarBody);
            CarPart _paintingPart = AppData.GetCarPartData(car.CarBaseModle, car.BodyTexture);
            InitObject(_paintingPart.Name, _paintingPart.ModelPath);
            for (int i = 0; i < car.Parts.Count; i++)
            {
                CarPart _tempPart = AppData.GetCarPartData(car.CarBaseModle, car.Parts[i]);
                InitObject(_tempPart.Name, _tempPart.ModelPath);
            }
        }
    }

    static void DeleteAllModel()
    {
        if (IsInitObject)
        {
            var _temp = objects.GetEnumerator();
            while (_temp.MoveNext())
            {
                GameObject.Destroy(_temp.Current.Value);
            }
            objects.Clear();
        }
    }
}
/// <summary>
/// 自定义车
/// </summary>
public class CustumCar
{
    public string CustumCarName, CarBaseModle, CarType, BodyTexture;
    /// <summary>
    /// 组件列表
    /// </summary>
    public List<string> Parts;

    /// <summary>
    /// 数据路径
    /// </summary>
    public static string Path { get { return m_path; } }
    public static string DefaultTexture { get { return "Default"; } }
    private static string m_path;


    public CustumCar()
    {
        CarType = CustumCarName = CarBaseModle = BodyTexture = string.Empty;
        Parts = new List<string>();
    }

    public CustumCar(string __carName)
    {
        CarBaseModle = __carName;
        CarType = AppData.GetCarDataByName(__carName).Type;
        BodyTexture = DefaultTexture;
        CustumCarName = string.Empty;
        Parts = new List<string>();
    }
    static CustumCar()
    {
        m_path = UnityEngine.Application.persistentDataPath + "/Custum";
        Debug.Log(m_path);
        if (!Directory.Exists(m_path))
        {
            Directory.CreateDirectory(m_path);
        }
    }

    public static implicit operator bool(CustumCar car)
    {
        return car == null ? false : true;
    }
}