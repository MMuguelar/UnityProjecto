using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class EarthMap : MonoBehaviour
{
    public GameObject earth;
    public GameObject nodeTemplate;
    public float earthRadius;
    public NodeControl nodeControl;
    private List<GameObject> instantiatedNodes = new List<GameObject>();

    public float lat;
    public float lon;

    GameObject g;

    private void Start()
    {
        //Generate3DNodes();
        Vector3 positionNode = LatLongToXYZ(lat, lon, 0);
        //g = Instantiate(nodeTemplate, positionNode, new Quaternion(0, 0, 0, 0));
    }

    //public void Update()
    //{
    //    genOne();
    //}

    //public void genOne()
    //{
    //    Vector3 positionNode = LatLongToXYZ(lat, lon, 0);
    //    g.transform.position = positionNode;
    //}

    public void InstantiateNodePrefab(Vector3 position)
    {
        GameObject newNode = Instantiate(nodeTemplate, position, Quaternion.identity);
        instantiatedNodes.Add(newNode);
    }

    public void DestroyAllInstantiatedNodes()
    {
        foreach (var node in instantiatedNodes)
        {
            Destroy(node);
        }
        instantiatedNodes.Clear();
    }

    //public void Generate3DNodes()
    //{
    //    Debug.Log("nodes refreshed");

    //    GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Node3D");

    //    foreach (GameObject obj in allObjects)
    //    {
    //        Destroy(obj);
    //    }

    //    List<Node> nodeList = nodeControl.GetNodeList("s", "s");

    //    for (int i = 0; i < nodeList.Count; i++)
    //    {
    //        Node thisnode = nodeControl.getNodeInfo(nodeList[i].adress, "");

    //        if (thisnode.position.lat == null)
    //        {
    //            thisnode.position.lat = "0";
    //        }
    //        if (thisnode.position.@long == null)
    //        {
    //            thisnode.position.@long = "0";
    //        }
    //        if (thisnode.position.altitude == null)
    //        {
    //            thisnode.position.altitude = "0";
    //        }

    //        //Debug.Log("LAT: " + thisnode.position.lat);
    //        //Debug.Log("LON: " + thisnode.position.lon);
    //        //Debug.Log("ALTITUDE: " + thisnode.position.altitude);

    //        Vector3 positionNode = LatLongToXYZ(float.Parse(thisnode.position.lat, CultureInfo.InvariantCulture), float.Parse(thisnode.position.@long, CultureInfo.InvariantCulture), float.Parse(thisnode.position.altitude));
    //        Instantiate(nodeTemplate, positionNode, new Quaternion(0, 0, 0, 0));
    //        nodeTemplate.transform.position = transform.position + positionNode;
    //    }
    //}

    public Vector3 LatLongToXYZ(float latitude, float longitude, float altitude)
    {

        float radius = earthRadius; // Radius of the earth (can be adjusted to your liking)
        float latitudeRadians = latitude * Mathf.Deg2Rad;
        float longitudeRadians = -longitude * Mathf.Deg2Rad;
        float x = (radius ) * Mathf.Cos(latitudeRadians) * Mathf.Sin(longitudeRadians);
        float y = (radius ) * Mathf.Sin(latitudeRadians);
        float z = (radius ) * Mathf.Cos(latitudeRadians) * Mathf.Cos(longitudeRadians);



        return new Vector3(x, y, z);
    }
}
