using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // 用于显示倒计时的UI Text
    public TextMeshProUGUI currentStaminaText;
    public int staminaIncreaseInterval = 300; // 体力增加的间隔时间（秒），默认5分钟
    public static int currentStamina = 0; // 当前体力值
    public GameObject AddStaminaPanel;
    private float timeRemaining; // 剩余的倒计时时间
    public void OpenAddPanel()
    {
        AddStaminaPanel.SetActive(true);
    }
    void Start()
    {
        currentStamina = PlayerPrefs.GetInt("Stamina");
        if (!PlayerPrefs.HasKey("First"))
        {
            // 执行首次启动时的操作
            Debug.Log("领取10初始体力");
            currentStamina = 10;
            PlayerPrefs.SetInt("Stamina", currentStamina);
            // 在这里添加你需要在首次启动时执行的代码

            // 设置键"FirstStart"，表示不是首次启动了
            PlayerPrefs.SetInt("First", 1);
            PlayerPrefs.Save(); // 确保保存设置
        }
        else
        {
            Debug.Log("已经领取过初始体力");
        }
        timeRemaining = staminaIncreaseInterval; // 初始化倒计时时间
    }
    public void ShowAdd()
    {
        AddStaminaPanel.SetActive(true);
    }
    public static void subtract(int number)
    {
        currentStamina -= number;
        PlayerPrefs.SetInt("Stamina", currentStamina);
    }
    public static void Add(int number)
    {
        currentStamina += number;
        PlayerPrefs.SetInt("Stamina", currentStamina);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentStamina -= 1;
        }
        PlayerPrefs.SetInt("Stamina", currentStamina);
        currentStaminaText.text = currentStamina.ToString();
        if (currentStamina >= 10)
        {
            timerText.gameObject.SetActive(false);
            return;
        }
        else
        {
            timerText.gameObject.SetActive(true);
        }
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // 减少倒计时时间
            UpdateTimerDisplay(); // 更新UI显示
        }
        else
        {
            // 倒计时结束，增加体力
            currentStamina++;
            Debug.Log("体力增加！当前体力: " + currentStamina);

            // 重置倒计时
            timeRemaining = staminaIncreaseInterval;
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // 计算分钟
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // 计算秒数

        // 更新UI Text显示
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
