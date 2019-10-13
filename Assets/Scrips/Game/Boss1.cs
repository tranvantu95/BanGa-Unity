using UnityEngine;
using System.Collections;

public class Boss1 : ChickenController
{

    public int blood = 100;
    bool hit = false;

    float unit = Mathf.PI * 2 / 7;

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(0, GlobalData.CameraSize.y + 3, 0);
        StartCoroutine(BayLuon());
        StartCoroutine(autoCreateEgg());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator autoCreateEgg()
    {
        yield return new WaitForSeconds(Random.Range(3f, 7f));

        for (int i = 0; i < 7; i++)
        {
            float r = unit * i;
            float cos = Mathf.Cos(r);
            float sin = Mathf.Sin(r);
            Vector3 deltaPos = new Vector3(sin, -cos);
            Vector2 v = new Vector2(50 * sin, 50 * -cos);
            GameObject egg = createEgg();
            egg.transform.position += deltaPos;
            egg.GetComponent<Rigidbody2D>().AddForce(v, ForceMode2D.Force);
        }
        StartCoroutine(autoCreateEgg());
    }

    public override IEnumerator BayLuon()
    {
        TweenPosition twP = TweenPosition.Begin(gameObject, 2, getRandomPos());
        twP.method = UITweener.Method.EaseInOut;
        twP.worldSpace = true;
        twP.from = transform.position;

        yield return new WaitForSeconds(3f);
        StartCoroutine(BayLuon());
    }

    public override Vector2 getRandomPos()
    {
        float x = Random.Range(-GlobalData.CameraSize.x + 1, GlobalData.CameraSize.x - 1);
        float y = Random.Range(-GlobalData.CameraSize.y + 3, GlobalData.CameraSize.y);

        return new Vector2(x, y);
    }

    public override void Hit()
    {
        blood--;
        if(blood == 0) Die();
        else if (!hit)
        {
            hit = true;
            StartCoroutine(Hit(0.01f, 10));
        }
    }

    //
    IEnumerator Hit(float t, int length)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        renderer.color = Color.red;
        yield return new WaitForSeconds(t);
        renderer.color = color;

        length--;
        if (length > 0)
        {
            yield return new WaitForSeconds(t);
            StartCoroutine(Hit(t, length));
        }
        else hit = false;
    }
}

