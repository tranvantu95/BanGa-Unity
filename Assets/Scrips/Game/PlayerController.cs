using UnityEngine;
using System.Collections;

public class PlayerController : BaseController {

    public Transform Plane, EffectStart;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public Vector3 deltaPos = Vector3.zero;

    public float scale = 5;

    public bool isProtected; // Được bảo vệ
    public bool showEffect, showEffectStart;

    public bool hit;

    public int bulletType;
    public bool lienThanh;

    bool live = true;

    void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        isProtected = true;
        showEffectStart = true;
        StartCoroutine(ShowEffectStart());
        StartCoroutine(Delay2(3f, () => {
            showEffectStart = false;
            StartCoroutine(HideEffectStart());
        }));

        smoothTime *= 5;
        StartCoroutine(Delay(smoothTime * 3f, () =>
        {
            smoothTime /= 5;
        }));
    }

    // Update is called once per frame
    void Update () {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;
        targetPosition += deltaPos;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Plane.eulerAngles = new Vector3(Mathf.Clamp(-velocity.y * scale, -5, 5), Mathf.Clamp(velocity.x * scale, -45, 45), 0);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!live || GameController.current.pause) return;

        if (other.CompareTag("enemy"))
        {
            if (isProtected) return;

            //if (!hit)
            //{
            //    hit = true;
            //    StartCoroutine(Hit(0.01f, 10));
            //}

            live = false;
            createBomb();
            Destroy(gameObject);
            GameController.current.PlayerController = null;
            GameController.current.StartCoroutine(GameController.current.Delay(1, () =>
            {
                if(GameController.current.PlayerController == null) GameController.current.CreatePlayer();
            }));
        }

        if (other.CompareTag("duiga"))
        {
            Destroy(other.gameObject);

            GameController.current.addScore(3);
        }

        if (other.CompareTag("hopqua"))
        {
            Destroy(other.gameObject);

            if(bulletType < 4) bulletType++;
        }
    }

    //
    IEnumerator Hit(float timeDelay, int length)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        renderer.color = Color.red;
        yield return new WaitForSeconds(timeDelay);
        renderer.color = color;

        length--;
        if (length > 0)
        {
            yield return new WaitForSeconds(timeDelay);
            StartCoroutine(Hit(timeDelay, length));
        }
        else hit = false;
    }

    //
    public IEnumerator ShowEffectStart()
    {
        showEffect = true;

        for (int i = 0; i < 3; i++)
        {
            GameObject Effect = EffectStart.GetChild(i).gameObject;
            Effect.SetActive(true);
            TweenAlpha twA = TweenAlpha.Begin(Effect, 2f, Effect.GetComponent<SpriteRenderer>().color.a);
            twA.from = 0;

            yield return new WaitForSeconds(0.33f);
        }
    }

    public IEnumerator HideEffectStart()
    {
        showEffect = false;
        GameObject Effect = EffectStart.gameObject;

        for (int i = 0; i < 3; i++) TweenAlpha.Begin(EffectStart.GetChild(i).gameObject, 2f, 0);        
        yield return new WaitForSeconds(2f);
        isProtected = false;
        Destroy(Effect);
    }

    //
    GameObject createBomb()
    {
        GameObject clone = Instantiate(GlobalData.Bomb) as GameObject;
        clone.transform.position = transform.position;
        Destroy(clone, 0.15f);

        return clone;
    }
}
