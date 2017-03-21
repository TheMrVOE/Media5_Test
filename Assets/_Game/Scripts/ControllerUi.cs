using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControllerUi : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField]
    private Button _btnPlay;
    [SerializeField]
    private Animator _animUiController;

    [Header("Game")]
    [SerializeField]
    private Button _btnMenu;
    [SerializeField]
    private TextMeshProUGUI _txtGhostsCount;
    [SerializeField]
    private Animator _animTxtGhostsCount;

    [Header("Pause")]
    [SerializeField]
    private GameObject _panelPause;
    [SerializeField]
    private Button _btnContinue;
    [SerializeField]
    private Button _btnRestart;

    private int _currentCount = 0;

    private void Awake()
    {
        AddLsiteners();
    }
    private void Start()
    {
        _panelPause.SetActive(false);
    }

    public void ChangeGhostsCount(int value, int maxCount)
    {
        _txtGhostsCount.text = value + "/" + maxCount;

        if(_currentCount> value)
        {
            _animTxtGhostsCount.SetTrigger("PlaySub");
        }
        else
        {
            _animTxtGhostsCount.SetTrigger("PlayAdd");
        }
        _currentCount = value;
    }
    private void AddLsiteners()
    {
        _btnPlay.onClick.AddListener(()=> {
            _animUiController.SetTrigger("Rise");
            StartCoroutine(LateGeneration());
        });

        _btnMenu.onClick.AddListener(()=> {
            _panelPause.SetActive(true);
            GeneratorGhost.instance.SetPause(true);
        });
        _btnRestart.onClick.AddListener(()=> {
            GeneratorGhost.instance.SetPause(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        });
        _btnContinue.onClick.AddListener(()=> {
            _panelPause.SetActive(false);
            GeneratorGhost.instance.SetPause(false);
        });
    }
    private IEnumerator LateGeneration()
    {
        yield return new WaitForSeconds(2);
        GeneratorGhost.instance.StartGenerator();
    }
   
}
