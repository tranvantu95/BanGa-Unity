using UnityEngine;
using System.Collections;

public class EggController : BaseController {

    bool isBroken;
    bool live = true;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, 13);
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!isBroken)
        {
            float sqrV = coll.relativeVelocity.sqrMagnitude;
            if (sqrV >= 2.25)
            {
                GetComponent<SpriteRenderer>().sprite = sqrV >= 6.25 ? GlobalData.egg3 : GlobalData.egg2;
                GetComponent<Rigidbody2D>().freezeRotation = true;
                transform.rotation = Quaternion.identity;
                isBroken = true;
            }
        }

        if(live && coll.gameObject.tag == "footer")
        {
            live = false;
            Destroy(gameObject, 3);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background")) Destroy(gameObject);
    }

}
