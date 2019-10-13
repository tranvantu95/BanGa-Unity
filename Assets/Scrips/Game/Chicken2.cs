using UnityEngine;
using System.Collections;

public class Chicken2 : ChickenController
{
    public Vector3 pos;
    float y;

    float speedX = -2;

	// Use this for initialization
	void Start () {
        StartCoroutine(autoCreateEggs());
        y = pos.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.current.pause) return;

        pos.x += speedX * Time.deltaTime;
        pos.y = -Mathf.Sin(pos.x) * 1.5f + y;

        transform.position = pos;

	}
}
