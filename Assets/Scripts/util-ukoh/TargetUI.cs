using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUI : MonoBehaviour
{
    [SerializeField] Sprite param_spritFile;
    [SerializeField] TargetManager _rManager;
    [SerializeField] Canvas _rCanvas;

    Camera _rCamera;
    Dictionary<GameObject, Transform> _rTransformsList;
    List<GameObject> _spritRenderes = new List<GameObject>();

    int _counter;

    public void SetTargetManager(TargetManager manager)
    { 
        _rManager = manager;
        _rTransformsList = _rManager.GetTargetList();
    }

    private void Start()
    {
        _rCanvas = GameObject.Find("UI/Canvas").GetComponent<Canvas>();
        _rCamera = transform.Find("Camera").GetComponent<Camera>();
        AddSpriteRenederer();
    }

    void AddSpriteRenederer()
    {
        GameObject renderer;
        _spritRenderes.Add(renderer = new GameObject("Sprite"));
        renderer.AddComponent<Image>();
        renderer.GetComponent<Image>().sprite = param_spritFile;
        renderer.transform.SetParent(_rCanvas.transform);
        renderer.SetActive(false);
    }
    void Update()
    {
        int activeTargetCount  = _rTransformsList.Count;
        int spriteRendereCount = _spritRenderes.Count;
        int dCount = activeTargetCount - spriteRendereCount;

        if (activeTargetCount > 0)
        { 
            int i = 0;
            foreach (Transform xform in _rTransformsList.Values)
            { 
                if(i < spriteRendereCount)
                {
                    Vector3 screenPoint = _rCamera.WorldToScreenPoint(xform.position);
                    _spritRenderes[i].transform.position = screenPoint;
                    _spritRenderes[i].SetActive(true);
                }
                i++;
            }
        }

        if (dCount < 0)
        {
            for (int i = activeTargetCount; i < spriteRendereCount; i++)
            {
                _spritRenderes[i].SetActive(false);
            }
        }

        AU.Debug.Log(activeTargetCount, AU.LogTiming.Update);
        AU.Debug.Log(spriteRendereCount, AU.LogTiming.Update);
    }

    void FixedUpdate()
    {
        int activeTargetCount = _rTransformsList.Count;
        int spriteRendereCount = _spritRenderes.Count;
        int dCount = activeTargetCount - spriteRendereCount;

        _counter++;
        _counter %= 12000;
        AU.Debug.Log(_counter, AU.LogTiming.Fixed);
        if (_counter % 3 == 1)
        {
            if (_counter == 1)
            {   //Re-assess the list size only at every 12000 update
                //Avoid excessive memory re-allocations;
                int minsize = 4;
                if (dCount < Mathf.Min( -minsize, activeTargetCount * -2))
                {
                    int tarCapacity = Mathf.Max( spriteRendereCount / 2, minsize);
                    for (int i = activeTargetCount + minsize; i < spriteRendereCount; i++)
                    { 
                        Destroy(_spritRenderes[i]);
                    }
                    _spritRenderes.RemoveRange(activeTargetCount + minsize, spriteRendereCount - (activeTargetCount + minsize));
                    _spritRenderes.Capacity = tarCapacity;
                }
            }
            for (int i = 0; i < dCount; i++)
            {
                AddSpriteRenederer();
            }
        }
    }
}
