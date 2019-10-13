using UnityEngine;
using System.Collections;

public class Map4 : BaseController, Map
{

    public int currentSprites;
    public Transform Sprites, Eggs;
    public GameObject Sprite, Egg;

    public float deltaTimeCreateSprite = 0.5f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateSprites());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CreateSprites()
    {
        int max = currentSprites;
        for (int j = 0; j < max; j++)
        {
            CreateSprite();

            yield return fixTime(deltaTimeCreateSprite);
        }

    }

    void CreateSprite()
    {
        GameObject newSprite = Instantiate(Sprite) as GameObject;
        newSprite.transform.SetParent(Sprites);
        Chicken3 chicken = newSprite.GetComponent<Chicken3>();

        chicken.map = this;
        float x = Random.Range(-GlobalData.CameraSize.x + 0.5f, GlobalData.CameraSize.x - 0.5f);
        chicken.transform.position = new Vector3(x, GlobalData.CameraSize.y + 1, 0);

        chicken.Eggs = Eggs.transform;
        chicken.Egg = Egg;
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
