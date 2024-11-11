using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectFactory;


public enum ObjectType
{
    objType_Metor,
    objType_Pebble,     // �����̏���
    objType_BoostMark,  //objType_Coin,
    objType_Max,
}

public class ObjectFactory : MonoBehaviour
{
    public ObjectPool MetorObjPool;
    public ObjectPool pebbleObjPool; 
    public ObjectPool boostMarkPool;
   
    //�A�N�e�B�u�����ŁATransform��Unity���Őݒ肷��K�v������
    public void generateObj(ObjectType Type)
    {
        switch (Type)
        {
            case ObjectType.objType_Metor:
                MetorObjPool.GetObject();
                break;

            case ObjectType.objType_Pebble:
                pebbleObjPool.GetObject(); 
                break;

            case ObjectType .objType_BoostMark:
                boostMarkPool.GetObject();
                break;
            //case ObjectType.objType_River:
            //    break;

            //case ObjectType.objType_Coin:
            //    break;

            //case ObjectType.objType_Max:
            //    break;

            default:
                break;
        }
    }
    //Pos��Rot���w�肵�A�N�e�B�u
    public GameObject generateObj(ObjectType Type,Vector3 pos , Quaternion rot)
    {
        switch (Type)
        {
            case ObjectType.objType_Metor:
                return MetorObjPool.GetObject(pos,rot);

            case ObjectType.objType_Pebble:
                return pebbleObjPool.GetObject(pos, rot);

            case ObjectType .objType_BoostMark:
                return pebbleObjPool.GetObject(pos, rot);
                //case ObjectType.objType_River:
            //    break;
            //case ObjectType.objType_Coin:
            //    break;
            //case ObjectType.objType_Max:
            //    break;
            default:
                break;
        }

        return null;
    }

}
