using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectInfoManager : MonoBehaviour
{
    public static ObjectInfoManager _instance;


    //可直接指定文本文件，TextAsset是专门处理文本文件的类
    public TextAsset objectInfoText;
    //创建字典
    private Dictionary<int, ObjectInfo> objectInfoDict = new Dictionary<int, ObjectInfo>();

    void Awake()
    {
        _instance = this;
        ReadObjectInfoText();
    }

    //查找字典中信息
    public ObjectInfo GetObjectInfoById(int id)
    {
        ObjectInfo info = null;
        objectInfoDict.TryGetValue(id, out info);

        return info; 
    }

    //读取Text文件
    void ReadObjectInfoText()
    {
        string text = objectInfoText.text;
        //将文本信息按行取到数组lineArray中。
        string[] lineArray=text.Split('\n');//按行分数据。
        //遍历数组lineArray
        foreach (string temp in lineArray)
        {
            //按逗号取出单个数据
            string[] valueArray = temp.Split(',');
            //创建类对象存储每条信息
            ObjectInfo info = new ObjectInfo();

            info.id = int.Parse(valueArray[0]);
            info.name = valueArray[1];
            info.icon_name = valueArray[2];
            switch (valueArray[3])
            {
                case "Drug":
                    info.type = ObjectType.Drug;
                    break;
                case "Equip":
                    info.type = ObjectType.Equip;
                    break;
                case "Mat":
                    info.type = ObjectType.Mat;
                    break;
            }
            if (info.type== ObjectType.Drug)
            {
                info.hp = int.Parse(valueArray[4]);
                info.mp = int.Parse(valueArray[5]);
                info.price_sell = int.Parse(valueArray[6]);
                info.price_buy = int.Parse(valueArray[7]);
            }
            else if (info.type == ObjectType.Equip)
            {

                info.attack= int.Parse(valueArray[4]);
                info.defense=int.Parse(valueArray[5]);
                info.speed= int.Parse(valueArray[6]);
                switch (valueArray[7])
                {
                    case "Headgear":
                        info.equipType = EquipType.Headgear;
                        break;
                    case "Armor":
                        info.equipType = EquipType.Armor;
                        break;
                    case "RightHand":
                        info.equipType = EquipType.RightHand;
                        break;
                    case "LeftHand":
                        info.equipType = EquipType.LeftHand;
                        break;
                    case "Shoe":
                        info.equipType = EquipType.Shoe;
                        break;
                    case "Accessory":
                        info.equipType = EquipType.Accessory;
                        break;
                }
                switch (valueArray[8])
                {
                    case "Magician":
                        info.careerType = CareerType.Magician;
                        break;
                    case "Swordman":
                        info.careerType = CareerType.Swordman;
                        break;
                    case "Common":
                        info.careerType = CareerType.Common;
                        break;
                }
                info.price_sell = int.Parse(valueArray[9]);
                info.price_buy = int.Parse(valueArray[10]);
            }
            //添加到字典
            objectInfoDict.Add(info.id,info);
        }  
    }
}

//枚举类型，记数据的种类
public enum ObjectType {Drug,Equip,Mat}
public enum EquipType { Headgear, Armor, RightHand, LeftHand, Shoe, Accessory}
public enum CareerType { Magician,Swordman,Common }

//用于存储单条信息内容
public class ObjectInfo
{//2001,黄金甲,armor0-icon,Equip,0,50,0,Armor,Swordman,150,200
    public int id;
    public string name;
    public string icon_name;
    public ObjectType type;

    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;

    public int attack;
    public int defense;
    public int speed;
    public EquipType equipType;
    public CareerType careerType;
}