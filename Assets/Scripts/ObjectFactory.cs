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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public generateObj(ObjectType Type,Vector3 pos , Quaternion rot)
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

}
