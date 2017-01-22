using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    private static UIController instance;
    public static UIController Instance
    {
        get
        {
            return instance;
        }
    }

    public Slider playerHealth;

    public int slimeKills = 0;

    public Text currentWave;
    public Text slimesKilled;

    public Text grubText;

    // --Game over screen--

    bool youDied;
    public Text youDiedText;
    public float youDiedFromScale;
    public float youDiedToScale;
    public float youDiedScaleSpeed;

    public GameObject retry;
    public GameObject returnToMenu;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentWave.text = "0";
        slimesKilled.text = "0";
        grubText.text = "0";
        youDied = false;
        youDiedText.enabled = false;
        retry.SetActive(false);
        returnToMenu.SetActive(false);
    }

    void Update ()
    {
        if (youDied)
        {
            youDiedText.rectTransform.localScale = Vector3.Lerp(youDiedText.rectTransform.localScale, Vector3.one * youDiedToScale, youDiedScaleSpeed);
        }
    }

    public void SetPlayerHealth(float fraction)
    {
        playerHealth.value = Mathf.Clamp01(fraction);

        Color color = Color.green;

        if (fraction <= 0)
        {
            color = Color.black;
        }
        else if (fraction < 0.3f)
        {
            color = Color.red;
        }
        else if (fraction < 0.5f)
        {
            color = new Color(1, 165/255f, 0);
        }
        else if (fraction < 0.8f)
        {
            color = Color.yellow;
        }
        
        ColorBlock colors = playerHealth.colors;

        colors.normalColor = color;

        playerHealth.colors = colors;
    }

    public void SetCurrentWave(int wave)
    {
        currentWave.text = "" + wave;
    }

    public void AddSlimeKill()
    {
        slimeKills++;

        slimesKilled.text = "" + slimeKills;
    }

    public void Win()
    {

    }

    public void SetFoodPercentage(int percentage)
    {
        grubText.text = "" + percentage;
    }

    public void GameOverUI()
    {
        youDiedText.enabled = true;
        youDiedText.rectTransform.localScale = Vector3.one * youDiedFromScale;
        youDied = true;
        retry.SetActive(true);
        returnToMenu.SetActive(true);
    }

    public void OnRetry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
