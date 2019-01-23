using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;


public class test : MonoBehaviour
{
    public int ch;
    public Transform LoadingBar;
    public Transform Rast;
    public Text Text;
    public Text Textrast;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed=30;
    [SerializeField] private float goal = 50;
    // Start is called before the first frame update
    void Start()
    {

        ch = 25;

    }

    void Update()
    {
        if(currentAmount < 143 * ch / goal)
        {
            int ch1 = ch;
            currentAmount += speed * Time.deltaTime;
            Textrast = GameObject.FindObjectOfType<Text>();
            Textrast.text = (currentAmount * 0.7f).ToString("0.0");
            Text.text = (currentAmount).ToString("0");
        }
        LoadingBar.GetComponent<Image>().fillAmount = currentAmount * 0.7f / 100;
        
    }
}
