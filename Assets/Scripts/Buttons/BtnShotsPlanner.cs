using UnityEngine;

public class BtnShotsPlanner : MonoBehaviour
{
    [SerializeField] public bool planStatic = false;
    [SerializeField] public bool planArc = false;
    [SerializeField] public ShotsPlanner shotsPlanner;
    [SerializeField] public RecorderMain recorderMain;
    [SerializeField] public GlobalVars globalVars;

    void OnValidate()
    {
        shotsPlanner.clusterCentersLists = globalVars.clusterCentersLists;
        if (planStatic) {
            shotsPlanner.CreateStaticPlans(); 
            recorderMain.DoStaticShots(); 
            Debug.Log("yes"); 
            planStatic = false;
        }
        if (planArc) {
            shotsPlanner.CreateArcPlans(); 
            recorderMain.DoArcShots(); 
            Debug.Log("yes"); 
            planArc = false;
        }
    }
}