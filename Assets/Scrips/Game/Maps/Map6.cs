using UnityEngine;
using System.Collections;

public class Map6 : MonoBehaviour, Map {

    public Transform Sprites, Eggs;
    public GameObject Egg;
    public float speedMoveWall = 1;
    public int currentSprites;
    public int chickenInWall;

    void Awake()
    {
        currentSprites = Sprites.childCount;
        chickenInWall = currentSprites;
    }

    // Use this for initialization
    void Start () {
        if(GlobalData.changeSize)
        {
            transform.Translate(new Vector3(0, GlobalData.deltaHeight, 0));
            TweenPosition twP = GetComponent<TweenPosition>();
            twP.from = transform.position;
            twP.duration = (twP.from.y - twP.to.y) / speedMoveWall;
        }

        for (int i = 0; i < currentSprites; i++)
        {
            ChickenController chicken = Sprites.GetChild(i).GetComponent<ChickenController>();

            chicken.map = this;
            chicken.inWall = true;

            chicken.Eggs = Eggs;
            chicken.Egg = Egg;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //
    public void MoveWall()
    {
        Transform wall = transform.GetChild(0);
        wall.transform.SetParent(null);
        Vector3 pos = wall.transform.position;
        pos.y = -12;
        TweenPosition twP = TweenPosition.Begin(wall.gameObject, 10f, pos);
        twP.method = UITweener.Method.EaseIn;
        twP.SetOnFinished(() =>
        {
            Destroy(wall.gameObject);
        });

        wall.GetComponent<ConstantForce2D>().enabled = false;
        for (int i = 0; i < wall.childCount; i++)
        {
            wall.GetChild(i).GetComponent<Collider2D>().enabled = false;
        }
    }

    //
    public void CancelWall()
    {
        Transform wall = transform.GetChild(0);
        wall.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < wall.childCount; i++)
        {
            wall.GetChild(i).GetComponent<Collider2D>().isTrigger = true;
        }
    }

    //
    public int GetChildCount()
    {
        return currentSprites;
    }

    public void SetChildCount(int childCount)
    {
        currentSprites = childCount;
    }

    public int GetChildFinishCurver()
    {
        return 0;
    }

    public void SetChildFinishCurver(int childFinishCurver)
    {
    }

    public void ActionAllFinishPointCurver()
    {
    }

    public Transform GetEggs()
    {
        return Eggs;
    }

    public void SetEggs(Transform Eggs)
    {
        this.Eggs = Eggs;
    }

}
