using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using System.Collections;

public class ChickenController : BaseController, ChickenInterface {

    public Map map;
    public Curver LineCurver;
    public bool worldPosition = true;
    public float speed = 1.0f;
    public float s = 0.0f;
    public float delay;

    public bool fixSpeed = true;
    public bool loop, revert;
    public int index, curverIndex;
    public Vector3 nextPoint;
    public Vector3 deltaPos = Vector3.zero;

    public bool fixEndAngle = true;
    public float endAngle = 90;
    public float angleDefault = 90;

    public EventDelegate.Callback ActionEndPointCurver;
    public bool destroyEndPointCurver;
    bool runPointCurver;

    public Transform Eggs;
    public GameObject Egg;
    public bool createEggOnStart;

    bool inScreen;
    public bool inWall;

    public int score = 5;

    public bool pauseWhenGamePause;

    bool died = false;

    void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        if (delay == 0) StartGame();
        else StartCoroutine(Delay(delay, () =>
        {
            StartGame();
        }));
    }
	
	// Update is called once per frame
	void Update () {
        if (pauseWhenGamePause && GameController.current.pause) return;

        UpdatePointCurver();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet") && other.transform.position.y <= GlobalData.CameraSize.y)
        {
            other.GetComponent<BulletController>().boom();

            Hit();
        }

        if(other.CompareTag("background"))
        {
            inScreen = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background"))
        {
            inScreen = false;
            if(!runPointCurver) DestroyChicken();
        }

        if (other.CompareTag("wall"))
        {
            OutWall();
        }
    }

    //
    void StartGame()
    {
        if (LineCurver != null) StartPointCurver();
        if (createEggOnStart) StartCoroutine(autoCreateEggs());
    }

    #region Line Curver
    void StartPointCurver()
    {
        index = revert ? LineCurver.GetLength() - 1 : 0;
        transform.position = LineCurver.GetPoint(index) + this.deltaPos;

        NextPointCurver();
        runPointCurver = true;
    }

    void UpdatePointCurver()
    {
        if (!runPointCurver) return;

        s = Time.deltaTime * speed;

        if (CheckGetNewPointCurver(s) && !NextPointCurver()) EndPointCurver();

        if (worldPosition) transform.position = Vector3.MoveTowards(transform.position, nextPoint, s);
        else transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPoint, s);

    }

    void EndPointCurver()
    {
        if (destroyEndPointCurver)
        {
            DestroyChicken();
            return;
        }

        if (ActionEndPointCurver != null) ActionEndPointCurver();

        map.SetChildFinishCurver(map.GetChildFinishCurver() + 1);
        if (isAllFinishPointCurver()) map.ActionAllFinishPointCurver();

        runPointCurver = false;
        if (fixEndAngle) transform.rotation = Quaternion.Euler(0, 0, endAngle - angleDefault);
    }

    bool NextPointCurver()
    {
        index = revert ? index - 1 : index + 1;
        if (!loop && (index < 0 || index >= LineCurver.GetLength())) return false;
        curverIndex = LineCurver.GetIndex(index);
        nextPoint = LineCurver.GetPoint(curverIndex) + this.deltaPos;
        if (fixSpeed && CheckGetNewPointCurver(s)) return NextPointCurver();

        Vector3 deltaPos = nextPoint - transform.position;
        float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - angleDefault);

        return true;
    }

    bool CheckGetNewPointCurver(float s)
    {
        if ((nextPoint - transform.position).sqrMagnitude < Mathf.Pow(s, 2)) return true;
        return false;
    }

    bool isAllFinishPointCurver()
    {
        return map.GetChildFinishCurver() >= map.GetChildCount();
    }
    #endregion

    //
    public GameObject createEgg()
    {
        GameObject clone = Instantiate(Egg) as GameObject;
        clone.transform.SetParent(Eggs);
        clone.transform.position = transform.position;

        return clone;
    }

    public IEnumerator autoCreateEggs()
    {
        yield return new WaitForSeconds(Random.Range(3, 30));
        if(inScreen) createEgg();
        StartCoroutine(autoCreateEggs());
    }

    //
    GameObject createDuiGa()
    {
        GameObject gOb;
        int number = Random.Range(0, 100);
        if (number < 3) gOb = GlobalData.HopQua;
        else if (number < 50) gOb = GlobalData.DuiGa;
        else return null;

        GameObject clone = Instantiate(gOb) as GameObject;
        clone.transform.SetParent(Eggs);
        clone.transform.position = transform.position;

        return clone;
    }

    //
    public void Create()
    {
        
    }

    public void Live()
    {

    }

    public void Die()
    {
        DestroyChicken();
        createDuiGa();

        if (!runPointCurver) map.SetChildFinishCurver(map.GetChildFinishCurver() - 1);
        else if (isAllFinishPointCurver()) map.ActionAllFinishPointCurver();

        GameController.current.addScore(score);
    }

    public virtual void Hit()
    {
        Die();
    }

    //
    void DestroyChicken()
    {
        if (died) return;
        died = true;

        Destroy(gameObject);
        map.SetChildCount(map.GetChildCount() - 1);

        if (inWall)
        {
            inWall = false;
            Map6 map6 = (Map6)map;
            map6.chickenInWall--;
            if (map6.chickenInWall <= 0) map6.CancelWall();
        }

    }

    //
    public virtual IEnumerator BayLuon()
    {
        TweenPosition twP = TweenPosition.Begin(gameObject, 2, getRandomPos());
        twP.method = UITweener.Method.EaseInOut;
        if(!twP.worldSpace)
        {
            twP.worldSpace = true;
            twP.from = transform.position;
        }

        yield return new WaitForSeconds(3);
        StartCoroutine(BayLuon());
    }

    public virtual Vector2 getRandomPos()
    {
        float x = Random.Range(-GlobalData.CameraSize.x + 1, GlobalData.CameraSize.x - 1);
        float y = Random.Range(-GlobalData.CameraSize.y + 1, GlobalData.CameraSize.y - 1);

        return new Vector2(x, y);
    }

    // For map6
    void OutWall()
    {
        if (inWall)
        {
            inWall = false;
            Map6 map6 = (Map6) map;
            map6.chickenInWall--;
            if (map6.chickenInWall <= 0) map6.CancelWall();

            StartCoroutine(Delay(1f, () =>
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().isTrigger = true;

                TweenRotation twP = TweenRotation.Begin(gameObject, 0.5f, Quaternion.identity);
                twP.quaternionLerp = true;
                StartCoroutine(BayLuon());
                StartCoroutine(autoCreateEggs());
            }));

        }

    }

    #region Scale Time
    public float scaleTime(float time, float deltaTime, Ease ease)
    {
        float time0 = (int)(time / deltaTime) * deltaTime;
        float time1 = time - time0;

        time = ease(time1, time0, deltaTime, deltaTime);

        return time;
    }

    // Ease
    // (current frame, start value, delta value, total frame)
    public delegate float Ease(float a, float b, float c, float d);

    public static float LINEAR(float a, float b, float c, float d)
    {
        return c * a / d + b;
    }

    public static float QUAD_EASEIN(float t, float b, float c, float d)
    {
        return c * (t /= d) * t + b;
    }

    public static float QUAD_EASEOUT(float t, float b, float c, float d)
    {
        return -c * (t /= d) * (t - 2) + b;
    }

    public static float QUAD_EASEINOUT(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1)
            return c / 2 * t * t + b;
        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    }

    public static float EXPO_EASEINOUT(float t, float b, float c, float d)
    {
        if (t == 0)
            return b;
        if (t == d)
            return b + c;
        if ((t /= d / 2) < 1)
            return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
    }
    #endregion
}
