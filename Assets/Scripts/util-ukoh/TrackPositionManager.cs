using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class TrackPositionManager : ScriptableObject
{
    [SerializeField] Spline param_trackSpline;
    [SerializeField] float  param_precision = 200.0f;

    int _resolution;

    public void UpdatePositions(Dictionary<int, GameObject> players)
    {
        float time  = Time.time;
        float dtime = time - _timeOld;
        if(dtime < 0.33f)
            return;
        
        _timeOld = time;

        SortedDictionary<float, GameObject> playerProgress= new SortedDictionary<float, GameObject>();

        foreach( var entry in players)
        { 
            GameObject player = entry.Value;
            float  t;
            float3 outPosition;
            SplineUtility.GetNearestPoint(param_trackSpline,
                player.transform.position, out outPosition, out t, _resolution); // using default iterations
            playerProgress.Add(t, player);
        }
        { 
            int i = players.Count;
            foreach(KeyValuePair<float, GameObject> entry in playerProgress)
            {
                entry.Value.GetComponent<CC.Hub>().curPosition = i;
                i--;
            }
        }
    }








    float _timeOld;
    void OnEnable()
    {
        if(param_trackSpline == null)
        {
            param_trackSpline = GameObject.Find("Track").GetComponent<SplineContainer>().Spline;
            float length = param_trackSpline.CalculateLength(float4x4.identity);
            _resolution = (int)(length / param_precision);
        }
    }
}
