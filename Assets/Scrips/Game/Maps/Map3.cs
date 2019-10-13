using UnityEngine;
using System.Collections;

public class Map3 : BaseController, Map
{
    public Transform Sprites, Eggs;
    public GameObject Egg;

    public int currentSprites;

    void Awake()
    {
        currentSprites = Sprites.childCount;
    }

    // Use this for initialization
    void Start () {
        for(int i = 0; i < currentSprites; i++)
        {
            ChickenController chicken = Sprites.GetChild(i).GetComponent<ChickenController>();

            chicken.map = this;

            chicken.Eggs = Eggs;
            chicken.Egg = Egg;
            chicken.createEggOnStart = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
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
