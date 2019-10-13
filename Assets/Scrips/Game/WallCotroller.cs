using UnityEngine;
using System.Collections;

public class WallCotroller : MonoBehaviour {

    public int blood = 15;
    bool hit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullet"))
        {
            other.GetComponent<BulletController>().boom();

            Hit();
        }

    }

    public void Hit()
    {
        blood--;
        if (blood == 0) Destroy(gameObject);
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
