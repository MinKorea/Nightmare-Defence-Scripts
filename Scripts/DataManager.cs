using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dm;

    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

    // Start is called before the first frame update
    void Start()
    {
        dm = this;
        LoadTable();
    }

    void LoadTable()
    {
        data = CSVReader.Read("Stat");

        for(int i = 0; i < data.Count; i++)
        {
            Debug.Log("DMG : " + data[i]["DMG"] + " " + "ASPD : " + data[i]["ASPD"] + " " + "SPD : " + data[i]["SPD"]);
        }
    }    

    public int GetDataDamage(int level)
    {
        return (int)data[level]["DMG"];
    }
    public int GetDataSpeed(int level)
    {
        return (int)data[level]["SPD"];
    }
    public float GetDataAttackSpeed(int level)
    {
        return (float)data[level]["ASPD"];
    }



}
