using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    public static GameManage Instance;

    public PostProcessVolume postProcessVolume;
    private AutoExposure autoExposure;

    private int totalMoney;
    private int money;
    public float timeValue = 90;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI moneyText;

    public GameObject winPanel;
    public GameObject overPanel;

    private PlayerController playerController;

    public bool isOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void OnEnable()
    {
        TotalMoneyCount();
        playerController = FindObjectOfType<PlayerController>();
    }

    IEnumerator Start()
    {
        DisplayMoney(0);

        //set fixed position position player at start for camera
        playerController.transform.position = new Vector2(-16.83999f, -6.945016f);
        yield return 0.2f;
        postProcessVolume.profile.TryGetSettings<AutoExposure>(out autoExposure);
        SetAutoExposure(1f);
    }

    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
            DisplayTime(timeValue);
        }
        else
        {
            timeValue = 0;

            SetAutoExposure(0f);
            Invoke(nameof(GameOver),0.35f);
        }

        if(playerController.transform.position.y < -12.5f)
        {
            SetAutoExposure(0f);
            Invoke(nameof(GameOver), 0.35f);
        }

        if(isOver == true && Input.GetKeyDown(KeyCode.Return))
        {
            RePlay();
        }

        if (isOver)
        {
            playerController.gameObject.SetActive(false);
        }
    }

    private void RePlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetAutoExposure(float value)
    {
        autoExposure.keyValue.value = value;
    }

    private void TotalMoneyCount()
    {
        var _count = FindObjectsOfType<Collectibles>().Length;
        totalMoney = _count;
    }

    public void CollectMoney()
    {
        money++;
        DisplayMoney(money);

        if(money == totalMoney)
        {
            SetAutoExposure(0f);
            Invoke(nameof(GameWin), 0.35f);
        }
    }

    public void GameOver()
    {
        overPanel.SetActive(true);
        isOver = true;
    }

    public void GameWin()
    {
        winPanel.SetActive(true);
        isOver = true;
    }

    private void DisplayMoney(int value)
    {
        moneyText.text = value.ToString() + "/" + totalMoney;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
