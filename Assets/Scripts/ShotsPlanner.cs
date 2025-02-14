using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ShotsPlanner : MonoBehaviour
{
    [SerializeField] public GlobalVars globalVars;

    [Header("General")]
    [SerializeField] public int fps = 5;  
    [SerializeField] public float duration = 5f;  
    [SerializeField] public string savePath = "ShotPlans";  // Folder to save plans
    [SerializeField] public int minFOV = 20;
    [SerializeField] public int maxFOV = 150;
    [SerializeField] public float minCameraHeight = 5f;

    [Space(20)]
    [Header("Static Shots")]
    [SerializeField] public int staticShotsNr;
    [SerializeField] public float staticShotsMinDist = 5f;
    [SerializeField] public float staticShotsMaxDist = 20f;
    [Space(20)]
    [Header("Arc Shots")]
    [SerializeField] public int arcShotsNr;
    [SerializeField] public float arcShotsMinDist = 5f;
    [SerializeField] public float arcShotsMaxDist = 20f;
    [SerializeField] public int arcShotsMinDegrees = 30;
    [SerializeField] public int arcShotsMaxDegrees = 240;

    public List<ShotPlanStatic> shotPlansStatic = new();
    public List<ShotPlanArc> shotPlansArc = new();
    public List<List<Vector3>> clusterCentersLists;

    void Start() {  // these shouldnt be called in start() because those vars might not be initialized yet (changing script execution order is bad) - they should be called manually via GUI or external class
        //clusterCentersLists = globalVars.clusterCentersLists;  // i will do it manually in btnshotsplanner
    }

    // for now lets assume that given coordinates are in global space
    public void CreateStaticPlans() {
        List<ShotPlanStatic> plans = PlanStaticShots();

        SaveStaticPlans(plans);
    }
    public List<ShotPlanStatic> PlanStaticShots() {
        List<ShotPlanStatic> plans = new();
        List<int> environmentShotsNrs = CalculateEnvironmentShotsNrs(staticShotsNr);
        int planNr = 0;

        int environmentsNr = environmentShotsNrs.Count;
        for (int environmentNr = 0; environmentNr < environmentsNr; environmentNr++) {  // for each environment
            int shotsNr = environmentShotsNrs[environmentNr];
            List<Vector3> clusterCenters = clusterCentersLists[environmentNr];
            GameObject tempEnvironment = Instantiate(globalVars.environments[environmentNr], Vector3.zero, Quaternion.identity);  // necessary for raycasting

            for (int i = 0; i < shotsNr; i++) {                                         // make all shots in 1 environment
                Vector3 targetPoint = clusterCenters[Random.Range(0, clusterCenters.Count)];
                Vector3 cameraLocation = SpherePoint(targetPoint, staticShotsMinDist, staticShotsMaxDist, minCameraHeight);
                int FOV = Random.Range(minFOV, maxFOV);

                ShotPlanStatic shotPlan = new(planNr, environmentNr, fps, duration, FOV, ShotTypes.Static,
                                            targetPoint, cameraLocation);
                plans.Add(shotPlan);
                planNr++;
            }

            Destroy(tempEnvironment);
        }

        return plans;
    }

    public List<int> CalculateEnvironmentShotsNrs(int shotsNr) {  // for one shot type, how many shots in each environment
        List<int> environmentShotsNrs = new();

        List<int> environmentWeights = globalVars.environmentWeights;
        float weightSum = environmentWeights.Sum();
        float avgShotNr = shotsNr / weightSum;  // how many shots for 1 environment with weight 1

        int shotNrDiff = shotsNr;  // how many shots we are missing
        for (int i = 0; i < environmentWeights.Count; i++) {
            int nr = (int) System.Math.Floor(avgShotNr * environmentWeights[i]);
            environmentShotsNrs.Add(nr);

            shotNrDiff -= nr;
        }
        
        for (int i = 0; i < shotNrDiff; i++) {  // we make up for the difference between number of shots from weights and the actual one, by incrementing first few environment shot frequencies
            environmentShotsNrs[i]++;
        }

        return environmentShotsNrs;
    }

    public void SaveStaticPlans(List<ShotPlanStatic> plans) {
        List<SerializableShotPlanStatic> serializablePlans = new();
        foreach (ShotPlanStatic plan in plans) {
            serializablePlans.Add(new SerializableShotPlanStatic(plan));
        }

        string json = JsonUtility.ToJson(new SerializableList<SerializableShotPlanStatic>(serializablePlans), true);
        File.WriteAllText("static.txt", json);
    }

    // jak tworze nowy obiekt shotplan to fajnie jakbym nie musial podawac wszystkich parametrow od razu
    // najlepiej zainicjowac pusty obiekty, a parametry recznie dodawac (parametry opcjonalne?)
    public void CreateArcPlans() {
        List<ShotPlanArc> plans = PlanArcShots();

        SaveArcPlans(plans);
    }
    public List<ShotPlanArc> PlanArcShots() {  // right now we take startingPoints randomly - TODO - make it, so that we take each possible starting point the same number of times (or even remove randomness)
        List<ShotPlanArc> plans = new();
        List<int> environmentShotsNrs = CalculateEnvironmentShotsNrs(arcShotsNr);
        int planNr = 0;

        int environmentsNr = environmentShotsNrs.Count;
        for (int environmentNr = 0; environmentNr < environmentsNr; environmentNr++) {  // for each environment
            int shotsNr = environmentShotsNrs[environmentNr];
            List<Vector3> clusterCenters = clusterCentersLists[environmentNr];
            GameObject tempEnvironment = Instantiate(globalVars.environments[environmentNr], Vector3.zero, Quaternion.identity);  // necessary for raycasting

            for (int i = 0; i < shotsNr; i++) {
                Vector3 startPoint = clusterCenters[Random.Range(0, clusterCenters.Count)];
                Vector3 cameraLocation = SpherePoint(startPoint, arcShotsMinDist, arcShotsMaxDist, minCameraHeight);
                int FOV = Random.Range(minFOV, maxFOV);
                int arcDegrees = Random.Range(arcShotsMinDegrees, arcShotsMaxDegrees);

                ShotPlanArc shotPlan = new(planNr, environmentNr, fps, duration, FOV, ShotTypes.Arc,
                                            startPoint, cameraLocation, arcDegrees);
                plans.Add(shotPlan);
                planNr++;
            }

            Destroy(tempEnvironment);
        }
        
        return plans;
    }

    public void SaveArcPlans(List<ShotPlanArc> plans) {
        List<SerializableShotPlanArc> serializablePlans = new();
        foreach (ShotPlanArc plan in plans) {
            serializablePlans.Add(new SerializableShotPlanArc(plan));
        }

        string json = JsonUtility.ToJson(new SerializableList<SerializableShotPlanArc>(serializablePlans), true);
        File.WriteAllText("arc.txt", json);
    }

    Vector3 SpherePoint(Vector3 start, float minDist, float maxDist, float minY = 0f) { // works by discarding values outside desired range
        int tries = 0;

        Vector3 randPoint;
        float dist;
        do
        {
            float x = Random.Range(-maxDist, maxDist);
            float y = Random.Range(-maxDist, maxDist);
            float z = Random.Range(-maxDist, maxDist);

            randPoint = new(start.x + x, start.y + y, start.z + z);

            dist = Vector3.Distance(start, randPoint);

            tries++;
            if (tries > 100) break;

            Vector3 rayOrigin = new(start.x, 50f, start.z);
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 100f)) 
            {
                if (start.y < hit.point.y) continue;
            }
        } while (dist > maxDist || dist < minDist || randPoint.y < minY);

        //if (randPoint.y < 0) randPoint.y = -randPoint.y;  // if below ground then take negative

        return randPoint;
    }

    // Vector3 SpherePoint(Vector3 origin, float minDist, float maxDist)
    // {
    //     float distance = Random.Range(minDist, maxDist);
    //     return origin + Random.onUnitSphere * distance;
    // }
}
