using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Curver : BaseMeshEffect
{
    public Transform Points;
    Vector3[] points;

    public bool worldPosition = true;

    public GameObject MiniPoint, MiniPoints;

    public float smoothness;

    public bool rendered;

    protected override void Awake()
    {
        base.Awake();
        Render();
        if(!rendered) Debug.Log("Render");
    }

    protected override void Start()
    {
        base.Start();
        if (rendered) foreach (Transform child in transform) Destroy(child.gameObject);
        GetComponent<LineRenderer>().enabled = !rendered;
        if (!rendered) Debug.Log("Start");
    }

    void Update()
    {
        if (rendered) return;
        Render();
        if (!rendered) Debug.Log("ReRender");
    }

    public override void ModifyMesh(VertexHelper helper)
    {
        if (!rendered) Debug.Log("ModifyMesh");
    }

    public IEnumerator<Vector3> GetPathsEnumerator()
    {
        if (points == null || points.Length == 1)
            yield break;
        var direction = 1;
        var index = 0;
        while (true)
        {
            yield return points[index];
            if (points.Length == 1)
                continue;
            if (index <= 0)
                direction = 1;
            else if (index >= points.Length - 1)
                direction = -1;
            index = index + direction;
        }
    }

    public Vector3 GetPoint(int curverIndex)
    {
        if (GetLength() == 0) return Vector3.zero;
        return points[curverIndex];
    }

    public int GetIndex(int index)
    {
        if (index < 0) index = -index;
        index %= points.Length * 2 - 2;
        if (index >= points.Length) index = points.Length * 2 - 2 - index;
        return index;
    }

    public int GetLength()
    {
        if (points == null) return 0;
        return points.Length;
    }

    [ContextMenu("Render")]
    public void Render()
    {
        int pointLength = Points.childCount;
        points = new Vector3[pointLength];
        if(worldPosition) for (int i = 0; i < pointLength; i++) points[i] = Points.GetChild(i).position;
        else for (int i = 0; i < pointLength; i++) points[i] = Points.GetChild(i).localPosition;
        points = Curver.MakeSmoothCurve(points, smoothness);

        if (rendered) return;

        // View
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = worldPosition;
        lineRenderer.SetVertexCount(points.Length);
        lineRenderer.SetPositions(points);

        MiniPoints.transform.DestroyChildren();
        //foreach (Transform tr in MiniPoints.transform) DestroyImmediate(tr.gameObject);

        int length = GetLength();
        for (int j = 0; j < length; j++)
        {
            GameObject gOb = Instantiate(MiniPoint) as GameObject;
            gOb.name = "Point " + (j + 1).ToString();
            gOb.transform.SetParent(MiniPoints.transform);
            if(worldPosition) gOb.transform.position = points[j];
            else gOb.transform.localPosition = points[j];
        }
    }

    //arrayToCurve is original Vector3 array, smoothness is the number of interpolations. 
    public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }
}