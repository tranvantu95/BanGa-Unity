using UnityEngine;
using System.Collections;

public class MapBoss : MonoBehaviour, Map
{

    public int currentSprites;
    public Transform Sprites, Eggs;
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
            ChickenController chicken = newSprite.GetComponent<ChickenController>();

            chicken.map = this;

            chicken.Eggs = Eggs.transform;
            chicken.Egg = Egg;

            yield return new WaitForSeconds(deltaTimeCreateSprite);
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