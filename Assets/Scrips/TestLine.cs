using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLine : MonoBehaviour {

    public enum FollowType
    {
        MoveTowards,
        Lerp
    }
    public FollowType type = FollowType.MoveTowards;
    public Curver curver;
    public float speed = 1.0f;
    public float t;
    Vector3 spacePath;

    public bool revert;
    public int index, curverIndex;
    public Vector3 nextPoint;

    // Use this for initialization
    void Start () {
        index = revert ? curver.GetLength() - 1 : 0;
        transform.position = curver.GetPoint(index);

        NextPoint();
    }

    // Update is called once per frame
    void FixedUpdate() {

        float s = Time.fixedDeltaTime * speed;

        if (type == FollowType.MoveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, s);
        }
        else if (type == FollowType.Lerp)
        {
            transform.position = Vector3.Lerp(transform.position, nextPoint, Time.fixedDeltaTime * t);
        }

        if ((nextPoint - transform.position).sqrMagnitude < Mathf.Pow(s * 2, 2))
        {
            NextPoint();
        }
    }

    public void NextPoint()
    {
        index = revert ? index - 1 : index + 1;
        curverIndex = curver.GetIndex(index);
        nextPoint = curver.GetPoint(curverIndex);

        Vector3 deltaPos = nextPoint - transform.position;
        float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        t = speed * 3 / deltaPos.magnitude;
    }
}
