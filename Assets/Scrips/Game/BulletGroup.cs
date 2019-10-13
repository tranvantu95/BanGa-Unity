using UnityEngine;
using System.Collections;

public class BulletGroup : MonoBehaviour {

    public int count;

    void Awake()
    {
        count = transform.childCount;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
