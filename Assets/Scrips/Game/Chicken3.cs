using UnityEngine;
using System.Collections;

public class Chicken3 : ChickenController {

    public int soDu = 1;
    public GameObject[] du;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.current.pause) return;

        if (soDu > 0) ThaDu();
	}

    void ThaDu()
    {
        Vector3 pos = transform.position;
        pos.y += -2f * Time.deltaTime;

        transform.position = pos;
    }

    public override void Hit()
    {
        if (soDu > 0)
        {
            soDu--;
            Destroy(du[soDu]);
            if(soDu == 0)
            {
                //Vector3 pos = transform.position;
                //pos.y += 0.5f;
                //TweenPosition twP = TweenPosition.Begin(gameObject, 0.3f, pos);
                //twP.method = UITweener.Method.EaseInOut;

                //StartCoroutine(Delay(0.3f, () => { StartCoroutine(BayLuon()); }));
                StartCoroutine(BayLuon());
                StartCoroutine(autoCreateEggs());
                GetComponent<Animator>().enabled = true;
            }
        }
        else Die();
    }
}
