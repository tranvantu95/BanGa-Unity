using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using System.Collections;

public class GameController : BaseController
{
    public static GameController current;

    public GameObject Player;
    public PlayerController PlayerController;
    public Transform Bullets;
    public GameObject[] Bullet;
    public GameObject EffectStartPlayer;

    public Transform Maps;
    public GameObject[] maps;
    public GameObject currentMap;
    IEnumerator CheckMap;
    Map map;
    int indexMap = - 1;

    public Transform Eggs;
    public Sprite egg2, egg3;

    public GameObject DuiGa, HopQua, Bomb;

    public float timeLife, timePause; 

    public bool gun;
    public float oldTimeCreateBullet, deltaTimeCreateBullet = 0.5f;

    public int score;
    public Text Score;

    public bool pause;
    public bool running
    {
        get { return !pause; }
    }

    void Awake()
    {
        current = this;

        GlobalData.egg2 = egg2;
        GlobalData.egg3 = egg3;
        GlobalData.DuiGa = DuiGa;
        GlobalData.HopQua = HopQua;
        GlobalData.Bomb = Bomb;

        
        GlobalData.CameraSize = GetCameraSize();

        if(Camera.main.aspect < 16 / 9f)
        {
            float width = GlobalData.CameraSize.y * 16 / 9f;
            float t = width / GlobalData.CameraSize.x;
            float height = GlobalData.CameraSize.y * t;
            GlobalData.changeSize = true;
            GlobalData.scaleHeight = t;
            GlobalData.deltaHeight = height - GlobalData.CameraSize.y;
            GlobalData.CameraSize = new Vector2(width, height);
            Camera.main.orthographicSize = height;

            BoxCollider2D background = GetComponent<BoxCollider2D>();
            Vector2 size = background.size;
            size.y *= t;
            background.size = size;

            GameObject Footer = GameObject.FindGameObjectWithTag("footer");
            Vector3 pos = Footer.transform.position;
            pos.y *= t;
            Footer.transform.position = pos;
        }

    }

    // Use this for initialization
    void Start () {
        StartGame();
    }
	
	// Update is called once per frame
	void Update () {
        if (!running) return;

        // Time Life
        timeLife += Time.deltaTime;

        if (PlayerController == null) return;

        // Gun
        if (gun && timeLife - oldTimeCreateBullet > deltaTimeCreateBullet)
        {
            oldTimeCreateBullet = timeLife;
            Shooting();
        }
    }

    void FixedUpdate()
    {
        
    }

    void OnMouseDown()
    {
        if (PlayerController == null || EventSystem.current.IsPointerOverGameObject()) return;

        if(PlayerController.lienThanh) gun = true;
        else Shooting(); ;
    }

    void OnMouseUp()
    {
        gun = false;
    }

    //
    void Shooting()
    {
        CreateBullet();

        if (PlayerController.deltaPos == Vector3.zero)
        {
            PlayerController.deltaPos = new Vector3(0, -0.25f, 0);
            StartCoroutine(Delay(PlayerController.smoothTime, () => {
                if (PlayerController != null) PlayerController.deltaPos = Vector3.zero;
            }));
        }
    }

    // Game life cycle
    public void StartGame()
    {
        CreatePlayer();
        indexMap = -1;
        currentMap = CreateMap();
        CheckMap = checkMap();
        StartCoroutine(CheckMap);

        setScore(0);
    }

    public void RestartGame()
    {
        RemoveAllChild(Eggs);
        if (PlayerController != null) Destroy(PlayerController.gameObject);
        if (currentMap != null)
        {
            Destroy(currentMap);
            StopCoroutine(CheckMap);
        }

        StartGame();
    }

    public void PauseGame()
    {
        pause = true;
        timePause = Time.time;

        if(PlayerController != null && !PlayerController.showEffect)
        {
            CreateEffectStartPlayer();
            PlayerController.StartCoroutine(PlayerController.ShowEffectStart());
        }
    }

    public void ResumeGame()
    {
        pause = false;

        if (PlayerController != null && !PlayerController.showEffectStart)
        {
            PlayerController.StartCoroutine(PlayerController.HideEffectStart());
        }
    }

    // UI
    public void setScore(int score)
    {
        this.score = score;
        Score.text = "Score: " + score.ToString();
    }

    public void addScore(int score)
    {
        setScore(this.score + score);
    }

    //
    public GameObject CreatePlayer()
    {
        GameObject clone = Instantiate(Player) as GameObject;
        clone.transform.position = new Vector3(0, -GlobalData.CameraSize.y - 2, 0);

        PlayerController = clone.GetComponent<PlayerController>();
        CreateEffectStartPlayer();

        return clone;
    }

    public GameObject CreateEffectStartPlayer()
    {
        GameObject clone = Instantiate(EffectStartPlayer) as GameObject;
        clone.transform.SetParent(PlayerController.transform);
        clone.transform.localPosition = Vector3.zero;
        PlayerController.EffectStart = clone.transform;

        return clone;
    }

    public GameObject CreateBullet()
    {
        GameObject clone = Instantiate(Bullet[PlayerController.bulletType].gameObject) as GameObject;
        clone.transform.SetParent(Bullets);
        clone.transform.position = PlayerController.transform.position;

        return clone;
    }

    public GameObject CreateMap()
    {
        indexMap++;
        GameObject Map = maps[indexMap % maps.Length];
        GameObject clone = Instantiate(Map) as GameObject;
        clone.transform.SetParent(Maps);

        map = clone.GetComponent<Map>();
        map.SetEggs(Eggs);

        return clone;
    }

    IEnumerator checkMap()
    {
        if (map.GetChildCount() == 0)
        {
            if(map is Map6)
            {
                ((Map6) map).MoveWall();
            }

            Destroy(currentMap);
            currentMap = null;

            StartCoroutine(Delay2(2f, () => {
                if(currentMap == null)
                {
                    currentMap = CreateMap();
                    CheckMap = checkMap();
                    StartCoroutine(CheckMap);
                }
            }));
        } else
        {
            yield return new WaitForSeconds(1f);
            CheckMap = checkMap();
            StartCoroutine(CheckMap);
        }
    }
}
