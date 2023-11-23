using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClusterDropdown : MonoBehaviour
{
    int intervalCheck = 1;
    float count;
    bool isActive = true;

    public int currentClusterID;
    public string currentClusterName;
    public string currentClusterNodes;

    private List<Cluster> myClusters;

    public TMP_Dropdown myDD;



    private void OnEnable()
    {
        myClusters = SAI.SDK.Clusters.GetClusterList();

        myDD.ClearOptions();

        List<TMP_Dropdown.OptionData> dddata = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < myClusters.Count; i++)
        {
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
            data.text = myClusters[i].cluster_name;
            dddata.Add(data);
        }
        myDD.AddOptions(dddata);
    }




    public void checkClusters()
    {
        myClusters = SAI.SDK.Clusters.GetClusterList();
        if (myClusters.Count == 0)
        {
            isActive = true;
            print("NO CLUSTER FOUND");
            
        }
        else
        {
            myDD.ClearOptions();
            List<TMP_Dropdown.OptionData> dddata = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < myClusters.Count; i++)
            {
                TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
                data.text = myClusters[i].cluster_name;
                dddata.Add(data);               
            }
            myDD.AddOptions(dddata);
            isActive = false;

        }
    }

    
}
