using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectFactory;

public class ObjectFactory : MonoBehaviour
{
    public ObjectPool rockOBJPool;
     
    public enum ObjectType
    {
        objType_Rock,
        objType_River,
        objType_Coin,
        objType_Max,
    }

    //アクティブだけで、TransformはUnity側で設定する必要がある
    public void generateObj(ObjectType Type)
    {
        switch (Type)
        {
            case ObjectType.objType_Rock:
                rockOBJPool.GetObject();
                break;
            case ObjectType.objType_River:
                break;
            case ObjectType.objType_Coin:
                break;
            case ObjectType.objType_Max:
                break;
            default:
                break;
        }
    }
    //PosとRotを指定しアクティブ
    public void generateObj(ObjectType Type,Vector3 pos , Quaternion rot)
    {
        switch (Type)
        {
            case ObjectType.objType_Rock:
                rockOBJPool.GetObject(pos,rot);
                break;
            case ObjectType.objType_River:
                break;
            case ObjectType.objType_Coin:
                break;
            case ObjectType.objType_Max:
                break;
            default:
                break;
        }
    }

}
