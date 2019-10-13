using UnityEngine;
using System.Collections;

public class BaseController : MonoBehaviour {

    public Vector2 GetCameraSize()
    {
        float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        return new Vector2(width, height);
    }

    public IEnumerator Delay(float t, EventDelegate.Callback action)
    {
        yield return new WaitForSeconds(t);
        action();
    }

    public IEnumerator Delay2(float t, EventDelegate.Callback action)
    {
        yield return fixTime(t);
        action();
    }

    public IEnumerator Loop(float t, EventDelegate.Callback action)
    {
        yield return new WaitForSeconds(t);
        action();
        StartCoroutine(Loop(t, action));
    }

    public IEnumerator Loop2(float t, EventDelegate.Callback action)
    {
        action();
        yield return new WaitForSeconds(t);
        StartCoroutine(Loop(t, action));
    }

    public IEnumerator fixTime(float timeDelay)
    {
        float time = Time.time;
        yield return new WaitForSeconds(timeDelay);
        while (GameController.current.pause) yield return new WaitForSeconds(0.05f);
        if (GameController.current.timePause > time) yield return fixTime(time + timeDelay - GameController.current.timePause);
    }

    // 
    public void RemoveAllChild(Transform parent)
    {
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++) Destroy(parent.GetChild(i).gameObject);
    }
}
