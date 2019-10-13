using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using System.Collections;

public class Map1 : BaseController, Map
{
    public Transform Lines, Sprites, Eggs;
    public GameObject Sprite, Egg;

    public Transform To;
    public float timeTo;

    public bool worldPosition = true;
    public Vector3 deltaPos;
    public bool fixHeight;

    public float deltaTimeCreateGroup = 0.0f;
    public float deltaTimeCreateLine = 0.3f;
    public float deltaTimeCreateSprite = 0.0f;

    public int countSpriteInALine = 1;

    public float speedSprite = 10;

    public int totalSprites, spritesCreated, currentSprites, spritesFinishCurver;
    public bool destroyEndPointCurver;

    public bool createEggOnStart;

    public bool pauseWhenGamePause;

    void Awake()
    {
        GetTotalSprites();
        currentSprites = totalSprites;
    }

    // Use this for initialization
    void Start () {
        if(fixHeight && GlobalData.changeSize) deltaPos = new Vector3(0, GlobalData.deltaHeight, 0);
        StartCoroutine(CreateGroup());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator CreateGroup()
    {
        int countGroup = Lines.transform.childCount;
        for(int g = 0; g < countGroup; g++)
        {
            Transform GroupLine = Lines.transform.GetChild(g);
            StartCoroutine(CreateLine(GroupLine));

            if (deltaTimeCreateGroup > 0)
            {
                if (pauseWhenGamePause) yield return fixTime(deltaTimeCreateGroup);
                else yield return new WaitForSeconds(deltaTimeCreateGroup);
            }
        }
    }

    IEnumerator CreateLine(Transform Group)
    {
        int countLine = Group.transform.childCount;
        for (int l = 0; l < countLine; l++)
        {
            Curver lineCurver = Group.GetChild(l).GetComponent<Curver>();
            StartCoroutine(CreateSprite(lineCurver));

            if (deltaTimeCreateLine > 0)
            {
                if (pauseWhenGamePause) yield return fixTime(deltaTimeCreateLine);
                else yield return new WaitForSeconds(deltaTimeCreateLine);
            }
        }
    }

    IEnumerator CreateSprite(Curver lineCurver)
    {
        for (int s = 0; s < countSpriteInALine; s++)
        {
            GameObject newSprite = Instantiate(Sprite) as GameObject;
            newSprite.transform.SetParent(Sprites);
            ChickenController chicken = newSprite.GetComponent<ChickenController>();

            chicken.map = this;
            chicken.LineCurver = lineCurver;
            chicken.worldPosition = worldPosition;
            chicken.speed = speedSprite;
            if (fixHeight) chicken.deltaPos = deltaPos;

            chicken.pauseWhenGamePause = pauseWhenGamePause;

            chicken.Eggs = Eggs;
            chicken.Egg = Egg;
            chicken.createEggOnStart = createEggOnStart;

            chicken.destroyEndPointCurver = destroyEndPointCurver;

            if(!createEggOnStart && !destroyEndPointCurver)
                chicken.ActionEndPointCurver = () =>
                {
                    chicken.StartCoroutine(chicken.autoCreateEggs());
                };

            spritesCreated++;

            if (deltaTimeCreateSprite > 0)
            {
                if (pauseWhenGamePause) yield return fixTime(deltaTimeCreateSprite);
                else yield return new WaitForSeconds(deltaTimeCreateSprite);
            }
        }

        if (spritesCreated == totalSprites) Destroy(Lines.gameObject);
    }

    //
    void GetTotalSprites()
    {
        foreach (Transform group in Lines)
            totalSprites += group.childCount * countSpriteInALine;
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
