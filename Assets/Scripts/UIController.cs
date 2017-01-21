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
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentWave.text = "0";
        slimesKilled.text = "0";
        grubText.text = "0";
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

    public void SetFoodPercentage(int percentage)
    {
        grubText.text = "" + percentage;
    }
}
