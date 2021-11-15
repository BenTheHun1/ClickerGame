using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float pts;
    public float money;
    public int ClickAmount;
    public int AutoClick;
    public int desiredPosition;
    public float camSpeed;
    public Text displayPts;
    public Text displayMoney;
    public Camera cam;

    public Transform upgradeContainer;
    public Transform projectContainer;
    public List<GameObject> upgradeList;

    public Text displayClickAmount;
    public Text displayAutoClick;

    // Start is called before the first frame update
    void Start()
    {
        ClickAmount = 1;
        foreach(GameObject up in upgradeList)
        {
            if (!up.GetComponent<Upgrade>().isProject)
            {
                Instantiate(up, upgradeContainer);
            }
            else
            {
                Instantiate(up, projectContainer);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        pts += AutoClick * Time.deltaTime;
        displayPts.text = pts.ToString("F0");
        displayMoney.text = "$" + money.ToString("F2");
        displayClickAmount.text = "Click: " + ClickAmount.ToString();
        displayAutoClick.text = "Auto: "+ AutoClick.ToString();
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(desiredPosition, cam.transform.position.y, cam.transform.position.z), Time.deltaTime * camSpeed);

    }

    public void Click()
    {
        pts += ClickAmount;
        displayClickAmount.gameObject.GetComponent<AudioSource>().Play();
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
    }

}
