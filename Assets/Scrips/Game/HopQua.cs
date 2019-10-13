using UnityEngine;
using System.Collections;

public class HopQua : MonoBehaviour {

    public float speed = -2;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.y += Time.deltaTime * speed;
        transform.position = pos;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background")) Destroy(gameObject);
    }
}
