using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameManager))]
public class SaveScript : MonoBehaviour
{
    private GameManager gm;
    private string savePath;
    public Text disAutoSave;
    private float AutoSaveTime;
    private float AutoSaveTimeCurrent;

    public GameObject deleteSaveConfirm;

    public GameObject loading;

    // Start is called before the first frame update
    void Start()
    {
        loading.SetActive(true);
        deleteSaveConfirm.SetActive(false);
        AutoSaveTime = 30f;
        AutoSaveTimeCurrent = AutoSaveTime;
        gm = GetComponent<GameManager>();
        savePath = Application.persistentDataPath + "/gamesave.sav";
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        LoadData();
        yield return new WaitForSeconds(0.25f);
        gm.em.ReloadEra();
        loading.SetActive(false);
    }

    private void Update()
    {
        AutoSaveTimeCurrent -= Time.deltaTime;
        disAutoSave.text = "AutoSave in " + AutoSaveTimeCurrent.ToString("F0");
        if (AutoSaveTimeCurrent <= 0)
        {
            SaveData();
            AutoSaveTimeCurrent = AutoSaveTime;
        }
    }

    public void OpenDeleteMenu()
    {
        deleteSaveConfirm.SetActive(true);
    }

    public void CloseDeleteMenu()
    {
        deleteSaveConfirm.SetActive(false);
    }


    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save Deleted");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.LogWarning("No Save Found");
            CloseDeleteMenu();
        }
    }

    // Update is called once per frame
    public void SaveData()
    {
        var save = new Save()
        {
            ptsSaved = gm.pts,
            ClickAmountSaved = gm.ClickAmount,
            ClickMultSaved = gm.ClickMult,
            AutoClickSaved = gm.AutoClick,
            AutoMultSaved = gm.AutoMult,
            eraSaved = gm.era,
            upgradesTimesPurchased = new List<int>()
        };
        foreach (GameObject upgrade in gm.upgradesInScene)
        {
            int disTimesPurchased = upgrade.GetComponent<Upgrade>().timesPurchased;
            save.upgradesTimesPurchased.Add(disTimesPurchased);
        }

        var bf = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            bf.Serialize(fileStream, save);
        }

        Debug.Log("Data Saved");
    }

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            Save save;

            var bf = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)bf.Deserialize(fileStream);
            }

            gm.pts = save.ptsSaved;
            gm.ClickAmount = save.ClickAmountSaved;
            gm.ClickMult = save.ClickMultSaved;
            gm.AutoClick = save.AutoClickSaved;
            gm.AutoMult = save.AutoMultSaved;
            gm.era = save.eraSaved;
            for (int i = 0; i < gm.upgradeList.Count; i++) 
            {
                gm.upgradesInScene[i].GetComponent<Upgrade>().timesPurchased = save.upgradesTimesPurchased[i];
                gm.upgradesInScene[i].GetComponent<Upgrade>().Reload();
            }
            Debug.Log("Data Loaded");
        }
        else
        {
            Debug.LogWarning("Save File Doesn't Exist");
        }
    }
}
