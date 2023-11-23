using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAI : MonoBehaviour
{
  
    public static SAI SDK;




    // Subsystem/Ramas

    
    public Wallet Wallet;
    public API API;
    public UtilHQ Util;   
    public Devices Devices;
    public Clusters Clusters;
    public Nodes Nodes;
    public LoginSystem Login;
    public UDP UDP;
    public ErrorHandler errorHandler;
    public LocationManager locationManager;
    public Storage Storage;
    
  

    //

    public BlockchainHQ blockchain;
    
    public DevicesHQ register_devices;
    public NodesHQ register_nodes;    
    public DebugHQ debuger;

    
    
    public Logger logger;
    
    public ClusterTransactionPG415 clusterTransaction;
    public IaRequest iarequest;

    private void Awake()
    {
        if (SDK == null) SDK = this; else Destroy(this.gameObject);  
        DontDestroyOnLoad(SDK);
        

    }
}
