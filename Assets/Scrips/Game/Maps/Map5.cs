using UnityEngine;
using System.Collections;

public class Map5 : BaseController, Map
{
    public Transform Lines, Sprites, Eggs;
    public GameObject Sprite, Egg;

    public Transform To;
    public float timeTo;

    public bool worldPosition = true;

    public float deltaTimeCreateGroup = 0.0f;
    public float deltaTimeCreateLine = 0.5f;
    public float deltaTimeCreateSprite = 0.0f;

    public int countSpriteInALine = 1;

    public float speedSprite = 10;

    public int totalSprites, spritesCreated, currentSprites, spritesFinishCurver;
    public bool destroyEndPointCurver;

    public bool createEggOnStart;

    void Awake()
    {
        GetTotalSprites();
        currentSprites = totalSprites;

    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateLine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CreateLine()
    {
        for(int l = 0; l < 3; l++)
        {
            StartCoroutine(CreateSprite(-l * 1.5f));

            if (deltaTimeCreateLine > 0) yield return new WaitForSeconds(deltaTimeCreateLine);
        }
    }

    IEnumerator CreateSprite(float deltaY)
    {
        Curver lineCurver = Lines.GetChild(0).GetComponent<Curver>();

        for (int s = 0; s < countSpriteInALine; s++)
        {
            GameObject newSprite = Instantiate(Sprite) as GameObject;
            newSprite.transform.SetParent(Sprites);
            ChickenController chicken = newSprite.GetComponent<ChickenController>();

            chicken.map = this;
            chicken.LineCurver = lineCurver;
            chicken.worldPosition = worldPosition;
            chicken.speed = speedSprite;

            chicken.deltaPos = new Vector3(s * 2, deltaY);

            chicken.Eggs = Eggs;
            chicken.Egg = Egg;
            chicken.createEggOnStart = createEggOnStart;

            chicken.destroyEndPointCurver = destroyEndPointCurver;

            if (!createEggOnStart && !destroyEndPointCurver)
                chicken.ActionEndPointCurver = () =>
                {
                    chicken.StartCoroutine(chicken.autoCreateEggs());
                };

            spritesCreated++;

            if (deltaTimeCreateSprite > 0) yield return new WaitForSeconds(deltaTimeCreateSprite);
        }

        if (spritesCreated == totalSprites) Destroy(Lines.gameObject);
    }

    //
    void GetTotalSprites()
    {        
        totalSprites += countSpriteInALine * 3;
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
        return spritesFinishCurver;
    }

    public void SetChildFinishCurver(int childFinishCurver)
    {
        spritesFinishCurver = childFinishCurver;
    }

    public void ActionAllFinishPointCurver()
    {
        if (To == null) return;
        TweenPosition twP = TweenPosition.Begin(Sprites.gameObject, timeTo, To.position);
        twP.style = UITweener.Style.PingPong;
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
