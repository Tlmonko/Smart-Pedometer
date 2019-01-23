using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Koleno : MonoBehaviour
{
    public GameObject BedroJoint;
    public GameObject Bedro;
    public GameObject KolenoJoint;
    public GameObject Golen;
    public GameObject DownPoint;
    public GameObject DownPoint1;
    public GameObject BedroJoint1;
    public GameObject Bedro1;
    public GameObject KolenoJoint1;
    public GameObject Golen1;
    public Text dataText;
    private Text dataText2;
    private Text hzText;
    private Text modeText;
    public Transform LoadingBar;
    public Transform Rast;
    public Text Text;
    public Text textDistance;

    private int countSteps = 0;
    private bool IsStable = false;
    private bool State;
    private int countPass = 89;
    private bool FileMode = true;
    private double HipPositionTemp = 90;
    private double TibiaPositionTemp = 90;
    private bool isFirst = true;
    private double HipPositionNow;
    private double TibiaPositionNow;
    private double HipPositionTemp1;
    private double TibiaPositionTemp1;
    private double HipPositionNow1;
    private double TibiaPositionNow1;
    private int[] PreviousNumbers = new int [50];
    private DateTime LastArrived;
    private int counter;
    private DateTime LastExec;
    private double BedroDegree;
    private double KolenoDegree;
    private double BedroDegree1;
    private double KolenoDegree1;
    private int Iterator = 0;
    private int nowSymbol;
    private int RotateBorder = 90;
    private string ReadFile;
    private List<(double, double)> Numbers = new List<(double, double)>();
    private string Data;
    private bool isEnd = false;
    [SerializeField] private string fileName = "prised2.csv";
    [SerializeField] private float currentAmount = 0;
    [SerializeField] private float speed = 30;
    [SerializeField] private float goal = 100;
    private Text HipText;
    private Image HipImage;
    private Image HipImage1;
    private Image HipImage2;
    private Image HipImage3;
    private Text HipText1;


    private void Start()
    {
        HipText1 = GameObject.FindObjectOfType<Text>();
        HipImage3 = Rast.GetComponent<Image>();
        HipImage2 = LoadingBar.GetComponent<Image>();
        HipImage1 = Rast.GetComponent<UnityEngine.UI.Image>();
        HipImage = LoadingBar.GetComponent<UnityEngine.UI.Image>();
        HipText = GameObject.FindObjectOfType<Text>();

        Debug.Log("Process started");
        dataText.text = $"{HipPositionNow}, {TibiaPositionNow}";
        if (FileMode)
        {
            var Temp = File.ReadAllText($@"/storage/emulated/0/temp/myFolder/{fileName}").Replace(" ", "")
                .Split('\n').Select(t => t.Split(',')).ToArray();
            var len = Temp.Count();

            for (var i = 0; i < Temp.Count(); i++)
            {
                try
                {
                    Numbers.Add((double.Parse(Temp[i][0], CultureInfo.InvariantCulture),
                        double.Parse(Temp[i][1], CultureInfo.InvariantCulture)));
                    Debug.Log("Added to numbers");
                }
                catch (Exception e)
                {
                    Debug.Log("Not added to numbers");
                    Debug.Log(e.Message);
                }
            }

            HipPositionTemp1 = Numbers[0].Item1;
            TibiaPositionTemp1 = Numbers[0].Item2;
        }

        Debug.Log($"List 0: {string.Join(", ", Numbers)}");

        //LastArrived = DateTime.Now;
        Debug.Log("Started by USSR");
        ServerThread.DataArrived += arr =>
        {
            Debug.Log("New Data");
//            var now = DateTime.Now;
//            if (now - LastExec < new TimeSpan(0, 0, 0, 0, 10)) return;
//            LastExec = now;
            HipPositionNow = 90 - arr[0];
            TibiaPositionNow = 90 - arr[1];
            if (isFirst)
            {
                TibiaPositionTemp = TibiaPositionNow;
                HipPositionTemp = HipPositionNow;
                isFirst = false;
            }

            BedroDegree = HipPositionNow - HipPositionTemp;
            KolenoDegree = TibiaPositionNow - TibiaPositionTemp;
            Debug.Log("Point 0");
//            try
//            {
//                counter++;
//
//                if (now - LastArrived >= new TimeSpan(0, 0, 1))
//                {
//                    LastArrived = now;
//                    counter = 0;
//                }
//            }
//            catch (Exception e)
//            {
//                Debug.Log(e);
//            }

            if (IsStep(HipPositionNow, TibiaPositionNow, ref countPass))
            {
                countSteps++;
                countPass = 0;
                Debug.Log($"Log on Step, CountStep = {countSteps}");
            }

            Debug.Log("Point 1");

            State = true;
        };
        ServerThread.Start();
    }

    private void FixedUpdate()
    {
        //Debug.Log($"countSteps in Update = {countSteps}");
        if (currentAmount <= 143 * countSteps / goal)
        {
            currentAmount += speed * Time.deltaTime;
            //textDistance = HipText1;
            textDistance.text = (countSteps * 0.7f).ToString("0.0");
            Text.text = countSteps.ToString();
        }

        HipImage2.fillAmount = currentAmount * 0.7f / 100;
        HipImage3.fillAmount = currentAmount / 100;

        if (FileMode && !isEnd && State)
        {
            Rotate(Golen, Bedro, DownPoint, KolenoJoint, KolenoDegree, KolenoDegree - BedroDegree);
            State = false;

            HipPositionNow1 = Numbers[nowSymbol].Item1;
            TibiaPositionNow1 = Numbers[nowSymbol].Item2;
            BedroDegree1 = HipPositionNow1 - HipPositionTemp1;
            KolenoDegree1 = TibiaPositionNow1 - TibiaPositionTemp1;

            Rotate(Golen1, Bedro1, DownPoint1, KolenoJoint1, KolenoDegree1, KolenoDegree1 - BedroDegree1);
            nowSymbol++;
            HipPositionTemp1 = HipPositionNow1;
            TibiaPositionTemp1 = TibiaPositionNow1;
            if (nowSymbol == Numbers.Count)
                isEnd = true;
            Debug.Log(nowSymbol);
        }
        else if (State)
        {
            Rotate(Golen, Bedro, DownPoint, KolenoJoint, KolenoDegree, KolenoDegree - BedroDegree);
            State = false;
        }

        HipPositionTemp = HipPositionNow;
        TibiaPositionTemp = TibiaPositionNow;
    }

    public void SetMode()
    {
        Debug.Log("Mode changed");
        FileMode = !FileMode;
        if (FileMode)
            modeText.text = "Mode: File";
        if (!FileMode)
            modeText.text = "Mode: Network";
    }

    private static void Rotate(GameObject rotating1, GameObject rotating2, GameObject rotating1Around,
        GameObject rotating2Around, double rotatting1Degree, double rotating2Degree)
    {
        rotating1.transform.RotateAround(rotating1Around.transform.position, new Vector3(0, 0, 1),
            float.Parse(Convert.ToString(rotatting1Degree)));
        rotating2.transform.RotateAround(rotating2Around.transform.position, new Vector3(0, 0, 1),
            float.Parse(Convert.ToString(rotating2Degree)));
    }

    private static bool IsStep(double position1, double position2, ref int counterLink)
    {
        counterLink++;
        return counterLink == 90 && position1 <= 90 && position2 >= 137;
    }
}