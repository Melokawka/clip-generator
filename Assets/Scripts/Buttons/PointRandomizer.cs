using UnityEngine;

public class PointRandomizer : MonoBehaviour
{
    private bool wasChanged = false;
    [SerializeField] public bool randomize = false;
    public GlobalVars globalVars;

    void OnValidate()
    {
        if (wasChanged == false) {wasChanged = true; return;} 
        globalVars.clusterCentersLists = globalVars.CalculateClusterCenters();
        randomize = false;
    }

    // private bool wasChanged = false;
    // [SerializeField]
    // public bool randomize = false;
    // public ClusterManager clusterManager;

    // void OnValidate()
    // {
    //     if (wasChanged == false) {wasChanged = true; return;} 
    //     clusterManager.RandomizeClusters();
    //     randomize = false;
    // }
}
