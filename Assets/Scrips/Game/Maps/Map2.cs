using UnityEngine;
using System.Collections;

public class Map2 : BaseController, Map
{

    public int currentSprites;
    public Transform StartPos, Sprites, Eggs;
    public GameObject Sprite, Egg;

    public float deltaTimeCreateSprite = 0.3f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateSprite());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CreateSprite()
    {
        int max = currentSprites;
        for (int j = 0; j < max; j++)
        {
            GameObject newSprite = Instantiate(Sprite) as GameObject;
            newSprite.transform.SetParent(Sprites);
            Chicken2 chicken = newSprite.GetComponent<Chicken2>();

            chicken.map = this;
            chicken.pos = StartPos.position;

            chicken.Eggs = Eggs.transform;
            chicken.Egg = Egg;

            yield return fixTime(deltaTimeCreateSprite);
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
