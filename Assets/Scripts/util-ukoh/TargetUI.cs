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
        _counter++;
        _counter %= 3;
        int activeTargetCount  = _rTransformsList.Count;
        int spriteRendereCount = _spritRenderes.Count;

        if (_counter == 1)
        {   if (spriteRendereCount < activeTargetCount)
            {
                int dCount = activeTargetCount -spriteRendereCount;
                for (int i = 0; i < dCount; i++)
                {
                    AddSpriteRenederer();
                }
            }
        }

        

        if (activeTargetCount > 0)
        { 
            int i = 0;
            foreach (Transform xform in _rTransformsList.Values)
            { 
                if(i > spriteRendereCount)
                    return;
                Vector3 screenPoint = _rCamera.WorldToScreenPoint(xform.position);
                _spritRenderes[i].transform.position = screenPoint;
                _spritRenderes[i].SetActive(true);
            }
        }
    }
}
