using UnityEngine;
using System.Collections;

public class DuiGa : BaseController {

    bool live = true;

    // Use this for initialization
    void Start () {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)), ForceMode2D.Force);
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (live && coll.gameObject.tag == "footer")
        {
            live = false;
            Destroy(gameObject, 5f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background")) Destroy(gameObject);
    }
}
