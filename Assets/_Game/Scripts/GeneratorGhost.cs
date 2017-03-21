using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools.Pool;

public struct WorldSreenPosition
{
    public float up;
    public float down;
    public float left;
    public float right;
}

public class GeneratorGhost : MonoBehaviour {

    private const int GHOST_COUNT_MAX = 10;
    private const float SPEED_MAX = 5;
    private const float SPEED_MIN = 2;

    [SerializeField] private GameObject _prefGhost;

    public static GeneratorGhost instance { get; private set; }
    public WorldSreenPosition screenWorldBorders;
    private int _ghostsCount = 0;
    public int ghostsCount
    {
        get
        {
            return _ghostsCount;
        }
        set
        {
            _ghostsCount = value;
            uiController.ChangeGhostsCount(value, GHOST_COUNT_MAX);
        }
    }
    public bool isGamePaused { get; set;}

    private Pool<Ghost> _poolGhost;
    private int _generationPeriod = 1;
    private WaitForSeconds _await;

    private ControllerUi _uiController;
    private ControllerUi uiController
    {
        get
        {
            if(_uiController==null)
            {
                _uiController = GameObject.FindObjectOfType<ControllerUi>();
            }
            return _uiController;
        }
    }

    private void Awake()
    {
        InitSingleton();
        _poolGhost = new Pool<Ghost>("Ghosts", _prefGhost);
        _await = new WaitForSeconds(_generationPeriod);
    }

	private void Start () {
        InitViewBorders();
        uiController.ChangeGhostsCount(0, GHOST_COUNT_MAX);
    }

	void Update () {
        CheckForInput();
	}

    public void StartGenerator()
    {
        StartCoroutine(Generator());
    }
    public void SetPause(bool isPause)
    {
        this.isGamePaused = isPause;
        if(isPause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    private void InitViewBorders()
    {
       var upperRight = Camera.main.ViewportToWorldPoint(Vector2.one);
       var downLeft = Camera.main.ViewportToWorldPoint(Vector2.zero);
        screenWorldBorders.right = upperRight.x;
        screenWorldBorders.up = upperRight.y;
        screenWorldBorders.down = downLeft.y;
        screenWorldBorders.left = downLeft.x;
    }
    private void InitSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There are two Ghost Generators in the scene!");
        }
    }
    private void GenerateGhost()
    {
        var ghost = _poolGhost.GetObjectFromPool();
        ghost.InitDefaults(UnityEngine.Random.Range(SPEED_MIN, SPEED_MAX), screenWorldBorders);
        ghostsCount++;
    }
    private void CheckForInput()
    {
        if (isGamePaused) return;

        if(Application.isMobilePlatform)
        {
            foreach (var item in Input.touches)
            {
                if(item.phase == TouchPhase.Began)
                {
                    HandlerInput(item.position);
                }
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                HandlerInput(Input.mousePosition);
            }
        }
    }
    private void HandlerInput(Vector2 clickPosition)
    {
        Vector3 ray = Camera.main.ScreenToWorldPoint(clickPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector3.zero);
        if (hit.transform != null && hit.transform.GetComponent<Ghost>() != null)
        {
            hit.transform.gameObject.SetActive(false);
            ghostsCount--;
        }
        
    }
    private IEnumerator Generator()
    {
        while(true)
        {
            yield return _await;
            if (_poolGhost.GetActiveElements().Count < GHOST_COUNT_MAX)
            {
                GenerateGhost();
            }
        }
    }
}
