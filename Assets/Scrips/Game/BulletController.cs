using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    public BulletGroup group;
    public float speed = 10;
    float vx, vy;

	// Use this for initialization
	void Start () {
        Vector3 angle = transform.eulerAngles;

        float radian = angle.z * Mathf.PI / 180;
        vy = speed * Mathf.Cos(radian);
        vx = - speed * Mathf.Sin(radian);
    }

    // Update is called once per frame
    void Update () {
        Vector3 pos = transform.position;
        pos.y += Time.deltaTime * vy;
        pos.x += Time.deltaTime * vx;
        transform.position = pos;
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background"))
        {
            DestroyBullet();
        }
    }

    //
    public void boom()
    {
        DestroyBullet();
    }

    //
    void DestroyBullet()
    {
        group.count--;
        if (group.count == 0) Destroy(transform.parent.gameObject);
        else Destroy(gameObject);
    }

}
