using UnityEngine;
using TMPro;

public class MissionDataParser : MonoBehaviour
{
    public GameObject Rocket;

    public Quaternion quaternion;

    public TMP_Text timestampText;
    //public TMP_Text flightTime;

    public TMP_Text rollText;
    public TMP_Text pitchText;
    public TMP_Text yawText;

    public TMP_Text LatText;
    public TMP_Text LonText;
    public TMP_Text AltText;

    public TMP_Text speedText;


    public string latestData;

    void Start()
    {
        //// Example data strings for testing
        //string imuDataString = "14|13010000|0.278005|0.949908|0.137005|0.04021|0.000000|0.000000|0.000000|0.000000|0.000000|0.000000|0.000000|0.000000|";
        //string gpsDataString = "19|1200|200|500|54|2|12|15|20|15|12|15|20|27|45|3|4|5|";

        //ParseDataString(imuDataString);
        //ParseDataString(gpsDataString);

        //ParseDataString(imuDataString);
    }

    private void Update()
    {
        // Update the UI if new Data is detected
        if (SAI.SDK.UDP.Receiver.latestData != string.Empty && SAI.SDK.UDP.Receiver.latestData != latestData)
        {
            latestData = SAI.SDK.UDP.Receiver.latestData;
            ParseDataString(latestData);
        }

    }

    void ParseDataString(string dataString)
    {
        try
        {
            dataString = dataString.Trim().Replace('.', ',');
            string[] dataValues = dataString.Split('|');

            if (dataValues.Length > 0)
            {
                int dataType = int.Parse(dataValues[0]);

                switch (dataType)
                {
                    case 14: // IMU data
                        ParseIMUData(dataString);
                        break;
                    case 19: // GPS data
                        ParseGPSData(dataString);
                        break;
                    default:
                        Debug.Log("Unknown data type.");
                        break;
                }
            }
            else
            {
                Debug.Log("Invalid data format.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing data: {e.Message}");
        }
    }


    void ParseIMUData(string imuDataString)
    {
        try
        {
            imuDataString = imuDataString.Trim();
            string[] imuDataValues = imuDataString.Split('|');

            if (imuDataValues.Length == 15 && imuDataValues[0] == "14")
            {
                IMUData imuData = new IMUData
                {
                    Timestamp = imuDataValues[1],
                    QuaternionInstantRotation = new Quaternion(
                        float.Parse(imuDataValues[3]),
                        float.Parse(imuDataValues[4]),
                        float.Parse(imuDataValues[5]),
                        float.Parse(imuDataValues[2])
                    ),
                    ReferenceQuaternion = new Quaternion(
                        float.Parse(imuDataValues[9]),
                        float.Parse(imuDataValues[6]),
                        float.Parse(imuDataValues[7]),
                        float.Parse(imuDataValues[8])
                    ),
                    EulerDeviations = new Vector3(
                        float.Parse(imuDataValues[10]),
                        float.Parse(imuDataValues[11]),
                        float.Parse(imuDataValues[12])
                    ),
                    TimeOfFlight = imuDataValues[13]
                };

                rollText.text = imuData.EulerDeviations.x.ToString();
                pitchText.text = imuData.EulerDeviations.y.ToString();
                yawText.text = imuData.EulerDeviations.z.ToString();


                timestampText.text = imuData.Timestamp;
                //flightTime.text = imuData.TimeOfFlight;
                RotateGameObject(Rocket,imuData.QuaternionInstantRotation);
                Debug.Log($"IMU Data: {imuData}");
            }
            else
            {
                Debug.Log("Invalid IMU data format.");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log($"Error parsing IMU data: {e.Message}");
        }
    }

    void ParseGPSData(string gpsDataString)
    {
        try
        {
            gpsDataString = gpsDataString.Trim();
            string[] gpsDataValues = gpsDataString.Split('|');

            if (gpsDataValues.Length == 19 && gpsDataValues[0] == "19")
            {
                GPSData gpsData = new GPSData
                {
                    iTOW = int.Parse(gpsDataValues[1]),
                    fTOW = float.Parse(gpsDataValues[2]),
                    week = int.Parse(gpsDataValues[3]),
                    gpsFix = int.Parse(gpsDataValues[4]),
                    flags = int.Parse(gpsDataValues[5]),
                    ecefX = double.Parse(gpsDataValues[6]),
                    ecefY = double.Parse(gpsDataValues[7]),
                    ecefZ = double.Parse(gpsDataValues[8]),
                    pAcc = double.Parse(gpsDataValues[9]),
                    ecefVX = double.Parse(gpsDataValues[10]),
                    ecefVY = double.Parse(gpsDataValues[11]),
                    ecefVZ = double.Parse(gpsDataValues[12]),
                    sAcc = double.Parse(gpsDataValues[13]),
                    pDOP = double.Parse(gpsDataValues[14]),
                    reserved1 = int.Parse(gpsDataValues[15]),
                    numSV = int.Parse(gpsDataValues[16]),
                    reserved2 = int.Parse(gpsDataValues[17])
                };

                Vector3 ecefCoordinates = new Vector3((float)gpsData.ecefX, (float)gpsData.ecefY, (float)gpsData.ecefZ);
                Vector3 latLonAlt = EcefToLatLonAlt(ecefCoordinates);

                LatText.text = latLonAlt.x.ToString();
                LonText.text = latLonAlt.y.ToString();
                AltText.text = latLonAlt.z.ToString();

                float speedInMetersPerSecond = Mathf.Sqrt((float)(gpsData.ecefVX * gpsData.ecefVX + gpsData.ecefVY * gpsData.ecefVY + gpsData.ecefVZ * gpsData.ecefVZ));

                float speedInKmPerHour = speedInMetersPerSecond * 3.6f;

                speedText.text = speedInKmPerHour.ToString();

                Debug.Log("LATITUDE: " + latLonAlt.x + " LONGITUDE: " + latLonAlt.y + " ALTITUDE: " + latLonAlt.z);
                Debug.Log($"GPS Data: {gpsData}");

            }
            else
            {
                Debug.Log("Invalid GPS data format.");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log($"Error parsing GPS data: {e.Message}");
        }
    }

    public static Vector3 EcefToLatLonAlt(Vector3 ecefCoordinates)
    {
        // WGS84 Ellipsoid Constants
        double a = 6378137.0; // Semi-major axis
        double eSquared = 6.69437999014e-3; // Eccentricity squared

        double x = ecefCoordinates.x;
        double y = ecefCoordinates.y;
        double z = ecefCoordinates.z;

        double lon = Mathf.Atan2((float)y, (float)x);

        double p = Mathf.Sqrt((float)(x * x + y * y));
        double lat = Mathf.Atan2((float)z, (float)p);

        // Calculate altitude
        double N = a / Mathf.Sqrt(1 - (float)eSquared * Mathf.Sin((float)lat) * Mathf.Sin((float)lat));
        double alt = p / Mathf.Cos((float)lat) - N;

        // Convert latitude and longitude to degrees
        lat = lat * Mathf.Rad2Deg;
        lon = lon * Mathf.Rad2Deg;

        return new Vector3((float)lat, (float)lon, (float)alt);
    }

    void RotateGameObject(GameObject target, Quaternion rotation)
    {
        Debug.Log("ROCKET VALUES: " + target.transform.rotation);
        target.transform.rotation = rotation;
    }
}


public class IMUData
{
    public string Timestamp { get; set; }
    public Quaternion QuaternionInstantRotation { get; set; }
    public Quaternion ReferenceQuaternion { get; set; }
    public Vector3 EulerDeviations { get; set; }
    public string TimeOfFlight { get; set; }

    public override string ToString()
    {
        return $"Timestamp: {Timestamp}, QuaternionInstantRotation: {QuaternionInstantRotation}, ReferenceQuaternion: {ReferenceQuaternion}, EulerDeviations: {EulerDeviations}, TimeOfFlight: {TimeOfFlight}";
    }
}
public class GPSData
{
    public int iTOW { get; set; }
    public float fTOW { get; set; }
    public int week { get; set; }
    public int gpsFix { get; set; }
    public int flags { get; set; }
    public double ecefX { get; set; }
    public double ecefY { get; set; }
    public double ecefZ { get; set; }
    public double pAcc { get; set; }
    public double ecefVX { get; set; }
    public double ecefVY { get; set; }
    public double ecefVZ { get; set; }
    public double sAcc { get; set; }
    public double pDOP { get; set; }
    public int reserved1 { get; set; }
    public int numSV { get; set; }
    public int reserved2 { get; set; }

    public override string ToString()
    {
        return $"iTOW: {iTOW}, fTOW: {fTOW}, week: {week}, gpsFix: {gpsFix}, flags: {flags}, " +
            $"ecefX: {ecefX}, ecefY: {ecefY}, ecefZ: {ecefZ}, pAcc: {pAcc}, " +
            $"ecefVX: {ecefVX}, ecefVY: {ecefVY}, ecefVZ: {ecefVZ}, sAcc: {sAcc}, " +
            $"pDOP: {pDOP}, reserved1: {reserved1}, numSV: {numSV}, reserved2: {reserved2}";
    }
}
