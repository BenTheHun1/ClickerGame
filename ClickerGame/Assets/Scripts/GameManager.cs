using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float money;
    public int ClickAmount;
    public int ClickMult;
    public int AutoClick;
    public int AutoMult;
    public int desiredPosition;
    public int RAMSpeed;
    public float camSpeed;
    public Text displayPts;
    public Text displayMoney;
    public Camera cam;

    public Transform upgradeContainer;
    public Transform projectContainer;
    public List<GameObject> upgradeList;
    public List<GameObject> upgradesInScene;

    public Text displayClickAmount;
    public Text displayAutoClick;

    public double pts;
    public string resource;
    public int era;
    public EvolveManager em;
    public string moneyType;
    public ParticleSystem clickparticles;

    // Start is called before the first frame update
    void Start()
    {
        em = gameObject.GetComponent<EvolveManager>();
        era = 1;
        ClickAmount = 1;
        ClickMult = 1;
        AutoClick = 0;
        AutoMult = 1;
        RAMSpeed = 1;
        //pts = 10000; //debug
        //money = 10000; //debug
        foreach(GameObject up in upgradeList)
        {
            if (!up.GetComponent<Upgrade>().isProject)
            {
                upgradesInScene.Add(Instantiate(up, upgradeContainer));
            }
            else
            {
                upgradesInScene.Add(Instantiate(up, projectContainer));
            }
        }
        InvokeRepeating("Auto", 1.0f, 1.0f);
    }

    void Auto()
    {
        pts += AutoClick * AutoMult;
    }

    // Update is called once per frame
    void Update()
    {
        displayPts.text = pts.ToString("F0") + " " + resource;
        displayMoney.text = money.ToString("F2") + " " + moneyType;
        displayClickAmount.text = resource + "/Click: " + ClickAmount.ToString();
        if (ClickMult > 1)
        {
            displayClickAmount.text += " x " + ClickMult.ToString() + " = " + (ClickAmount * ClickMult).ToString();
        }
        displayAutoClick.text = resource + "/Sec: " + AutoClick.ToString();
        if (AutoMult > 1)
        {
            displayAutoClick.text += " x " + AutoMult.ToString() + " = " + (AutoClick * AutoMult).ToString();
        }
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(desiredPosition, cam.transform.position.y, cam.transform.position.z), Time.deltaTime * camSpeed);

        if (Input.GetKeyDown(KeyCode.P))
        {
            pts += 1000;
        }

    }

    public void Click()
    {
        pts += ClickAmount * ClickMult;
        displayClickAmount.gameObject.GetComponent<AudioSource>().Play();
        clickparticles.Play();
    }

    public void MoveCam(string dir)
    {
        if (dir == "l")
        {
            desiredPosition = -230;
        }
        else if (dir == "m")
        {
            desiredPosition = 0;
        }
        else if (dir == "r")
        {
            desiredPosition = 230;
        }
        else if (dir == "ll")
        {
            desiredPosition = -460;
        }
        else if (dir == "rr")
        {
            desiredPosition = 460;
        }
    }

}
