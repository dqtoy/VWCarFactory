//0802
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CarStudio
{
    public static CustumCar Car { get { return car; } }
    public static bool IsInitObject = true;
    public static Dictionary<string, GameObject> objects;

    static CustumCar car;
    static Dictionary<string, List<GameObject>> partObjects;
    static Dictionary<string, GameObject> oldPartObjects;
    static Dictionary<string, Material> oldPartMaterials;
    static Dictionary<string, Texture> oldTextures;
    static string carBodyName = "CarBody";

    static CarStudio()
    {
        objects = new Dictionary<string, GameObject>();
        partObjects = new Dictionary<string, List<GameObject>>();
        oldPartObjects = new Dictionary<string, GameObject>();
        oldPartMaterials = new Dictionary<string, Material>();
        oldTextures = new Dictionary<string, Texture>();
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
        if (AppData.GetCarDataByName(car.CarBaseModle).CustumParts.Exists((_temp) => _temp.Name == _part.Name))
        {
            if (car.Parts.Exists((_s) => _s == _part.Name))
            {
                Debug.Log("配件已存在");
                return false;
            }
            else
            {
                car.Parts.Add(_part.Name);
                InitObject(_part);
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
        if (car.Parts.Exists((_s) =>
                                {
                                    if (_s == _part.Name)
                                    {
                                        DeletePart(_part);
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
    public static void SaveCustumUserCar(string __name)
    {
        if (string.IsNullOrEmpty(car.CarBaseModle))
        {
            Debug.Log(car.CarBaseModle + "不能为空");
        }

        car.Name = __name;
        string _filePath = CustumCar.Path + "/" + __name + ".json";
        File.WriteAllText(_filePath, LitJson.JsonMapper.ToJson(car));

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
    /// <param name="__TemplateCarName"></param>
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
            car = new CustumCar(car.CarBaseModle);
            InitCar();
            //Debug.Log("指定模板数据不存在");
        }
    }

    public static CustumCar GetTemplate(string __Name)
    {
        string _fileName = Application.streamingAssetsPath + "/Data/Template/" + __Name + ".json";
        if (File.Exists(_fileName))
        {
            CustumCar _car = LitJson.JsonMapper.ToObject<CustumCar>(File.ReadAllText(_fileName));
            return _car;
        }
        else
        {
            
            throw new System.Exception("指定模板数据不存在");
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

    /// <summary>
    /// 删除所有模型
    /// </summary>
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

    ///// <summary>
    ///// (弃用)生成模型
    ///// </summary>
    ///// <param name="__name">模型名字</param>
    ///// <param name="__path">模型路径</param>
    //static void InitObject(string __name, string __path)
    //{
    //    if (IsInitObject)
    //    {
    //        object _asset = Resources.Load(__path);
    //        if (_asset is GameObject)
    //        {
    //            GameObject _obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(__path));
    //            objects.Add(__name, _obj);
    //        }
    //        else
    //        {
    //            Texture2D _tex = _asset as Texture2D;
    //            GameObject _carBody = objects[car.CarBaseModle];
    //            _carBody.GetComponent<Renderer>().material.mainTexture = _tex;
    //        }

            
    //    }
    //}

    /// <summary>
    /// 生成模型
    /// </summary>
    /// <param name="__name">模型名字</param>
    /// <param name="__path">模型路径</param>
    static void InitObject(CarPart __part)
    {
        if (IsInitObject)
        {
            if (__part.Assets == null)
                return;
            Object _asset = Resources.Load(__part.Assets[0].AssetPath);
            if (__part.Name == carBodyName)
            {
                GameObject _obj = GameObject.Instantiate(_asset) as GameObject;
                objects.Add(carBodyName, _obj);
                Component[] _coms = _obj.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < _coms.Length; i++)
                {
                    objects.Add(_coms[i].name, _coms[i].gameObject);
                }
            }
            else
            {
                partObjects.Add(__part.Name, new List<GameObject>());
                
                for (int i = 0; i < __part.Assets.Count; i++)
                {
                    Object _tempAsset = Resources.Load(__part.Assets[i].AssetPath);
                    if (_tempAsset is GameObject) //如果资源是模型
                    {
                        GameObject _obj = GameObject.Instantiate(_tempAsset) as GameObject;
                        partObjects[__part.Name].Add(_obj);
                        if (!string.IsNullOrEmpty(__part.Assets[i].Target))
                        {
                            if (objects.ContainsKey(__part.Assets[i].Target))
                            {
                                if (!oldPartObjects.ContainsKey(__part.Assets[i].Target))
                                {
                                    oldPartObjects.Add(__part.Assets[i].Target, objects[__part.Assets[i].Target]);
                                    objects[__part.Assets[i].Target].SetActive(false);
                                }
                                else
                                {
                                    for (int j = 0; j < car.Parts.Count; j++)
                                    {
                                        CarPart _tempPart = AppData.GetCarPartData(car.CarBaseModle, car.Parts[j]);
                                        if (_tempPart.HasTarget(__part.Assets[i].Target))
                                        {
                                            RemovePart(_tempPart);
                                            oldPartObjects.Add(__part.Assets[i].Target, objects[__part.Assets[i].Target]);
                                            objects[__part.Assets[i].Target].SetActive(false);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new System.Exception("组件" + __part.Name + "的Asset[" + i + "]的Target不存在");
                            }
                        }
                    }
                    else if(_tempAsset is Material)//如果资源是材质
                    {
                        if (!string.IsNullOrEmpty(__part.Assets[i].Target))
                        {

                            if (objects.ContainsKey(__part.Assets[i].Target))
                            {
                                MeshRenderer _renderer = objects[__part.Assets[i].Target].GetComponent<MeshRenderer>();
                                if (!oldPartMaterials.ContainsKey(__part.Assets[i].Target))
                                {
                                    oldPartMaterials.Add(__part.Assets[i].Target, _renderer.material);
                                }
                                else
                                {
                                    for (int j = 0; j < car.Parts.Count; j++)
                                    {
                                        CarPart _tempPart = AppData.GetCarPartData(car.CarBaseModle, car.Parts[j]);
                                        if (_tempPart.HasTarget(__part.Assets[i].Target))
                                        {
                                            RemovePart(_tempPart);
                                            oldPartMaterials.Add(__part.Assets[i].Target, _renderer.material);
                                            break;
                                        }
                                    }
                                }
                                _renderer.material = _tempAsset as Material;
                            }
                            else
                            {
                                throw new System.Exception("组件" + __part.Name + "的Asset[" + i + "]的Target不存在");
                            }
                        }
                    }
                    else if (_tempAsset is Texture2D)//如果资源是贴图
                    {
                        if (!string.IsNullOrEmpty(__part.Assets[i].Target))
                        {

                            if (objects.ContainsKey(__part.Assets[i].Target))
                            {
                                if (oldTextures.ContainsKey(__part.Assets[i].Target))
                                {
                                    for (int j = 0; j < car.Parts.Count; j++)
                                    {
                                        CarPart _tempPart = AppData.GetCarPartData(car.CarBaseModle, car.Parts[j]);
                                        if (_tempPart.HasTarget(__part.Assets[i].Target))
                                        {
                                            RemovePart(_tempPart);
                                            break;
                                        }
                                    }
                                }

                                MeshRenderer _renderer = objects[__part.Assets[i].Target].GetComponent<MeshRenderer>();
                                oldTextures.Add(__part.Assets[i].Target, _renderer.material.mainTexture);
                                _renderer.material.mainTexture = _tempAsset as Texture2D;
                            }
                            else
                            {
                                throw new System.Exception("组件" + __part.Name + "的Asset[" + i + "]的Target不存在");
                            }
                        }
                    }
                }
            }
            


        }
    }


    ///// <summary>
    ///// (弃用)删除模型
    ///// </summary>
    ///// <param name="__name"></param>
    //static void DeleteModel(string __name)
    //{
    //    if (IsInitObject)
    //    {
    //        GameObject.Destroy(objects[__name]);
    //        objects.Remove(__name);
    //    }
    //}

    static void DeletePart(CarPart __part)
    {
        if (IsInitObject)
        {
            //组件物体删掉
            if (partObjects.ContainsKey(__part.Name))
            {
                for (int i = 0; i < partObjects[__part.Name].Count; i++)
                {
                    GameObject.Destroy(partObjects[__part.Name][i]);
                }
                partObjects.Remove(__part.Name);
            }
            
            for (int i = 0; i < __part.Assets.Count; i++)
            {
                if (string.IsNullOrEmpty(__part.Assets[i].Target))
                    continue;

                //旧的组件还原
                if (oldPartObjects.ContainsKey(__part.Assets[i].Target))
                {
                    oldPartObjects[__part.Assets[i].Target].SetActive(true);
                    oldPartObjects.Remove(__part.Assets[i].Target);
                }

                //材质还原
                if (oldPartMaterials.ContainsKey(__part.Assets[i].Target))
                {
                    GameObject _obj = objects[__part.Assets[i].Target];
                    MeshRenderer _renderer = _obj.GetComponent<MeshRenderer>();
                    GameObject.Destroy(_renderer.material);
                    _renderer.material = oldPartMaterials[__part.Assets[i].Target];
                    oldPartMaterials.Remove(__part.Assets[i].Target);
                }

                //移除贴图
                if (oldTextures.ContainsKey(__part.Assets[i].Target))
                {
                    GameObject _obj = objects[__part.Assets[i].Target];
                    MeshRenderer _renderer = _obj.GetComponent<MeshRenderer>();
                    _renderer.material.mainTexture = oldTextures[__part.Assets[i].Target];
                    oldTextures.Remove(__part.Assets[i].Target);

                }
            }
            
        }
    }

    static void InitCar()
    {
        if (IsInitObject)
        {
            DeleteAllModel();
            CarData _data = AppData.GetCarDataByName(car.CarBaseModle);
            InitObject(AppData.GetCarPartData(car.CarBaseModle, carBodyName));
            
            for (int i = 0; i < car.Parts.Count; i++)
            {
                if (car.Parts[i] == carBodyName)
                    continue;
                CarPart _tempPart = AppData.GetCarPartData(car.CarBaseModle, car.Parts[i]);
                InitObject(_tempPart);
            }
        }
    }

    static void DeleteAllModel()
    {
        if (IsInitObject)
        {
            //删光就材质
            foreach (var _key in oldPartMaterials.Keys)
            {
                GameObject _obj = objects[_key];
                MeshRenderer _renderer = _obj.GetComponent<MeshRenderer>();
                GameObject.Destroy(_renderer.material);
                GameObject.Destroy(oldPartMaterials[_key]);
            }
            oldPartMaterials.Clear();

            foreach (var _key in oldTextures.Keys)
            {
                GameObject _obj = objects[_key];
                MeshRenderer _renderer = _obj.GetComponent<MeshRenderer>();
                GameObject.Destroy(_renderer.material.mainTexture);
                GameObject.Destroy(oldTextures[_key]);
            }
            oldTextures.Clear();

            oldPartObjects.Clear();

            foreach (var _key in partObjects.Keys)
            {
                for (int i = 0; i < partObjects[_key].Count; i++)
                {
                    GameObject.Destroy(partObjects[_key][i]);
                }
            }
            partObjects.Clear();

            if (objects.ContainsKey(carBodyName))
                GameObject.Destroy(objects[carBodyName]);
            objects.Clear();
        }
    }
}
/// <summary>
/// 自定义车
/// </summary>
public class CustumCar:IButtonInfo
{
    /// <summary>
    /// 文件名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 标签
    /// </summary>
    public string Tag { get; set; }
    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }
    public string Type { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    public string PdfDescription { get; set; }

    public List<string> MovieDescription { get; set; }
    public List<string> TextureDescription { get; set; }

    /// <summary>
    /// 基本模型的名字
    /// </summary>
    public string CarBaseModle;
    public string BodyTexture;
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
        Name = Tag = Icon = Type = Description = CarBaseModle = BodyTexture = string.Empty;
        Parts = new List<string>();
    }

    public CustumCar(string __carName)
    {
        CarBaseModle = __carName;
        Type = AppData.GetCarDataByName(__carName).Type;
        Name = Tag = Icon = Description = BodyTexture = string.Empty;

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