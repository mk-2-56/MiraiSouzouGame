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

    //�A�N�e�B�u�����ŁATransform��Unity���Őݒ肷��K�v������
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
    //Pos��Rot���w�肵�A�N�e�B�u
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
