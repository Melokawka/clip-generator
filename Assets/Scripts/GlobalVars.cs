using UnityEngine;
using System.Collections.Generic;

public class GlobalVars : MonoBehaviour
{

    //[SerializeField] public List<Vector3> clusterCenters = new();
    [SerializeField] public List<GameObject> environments = new();
    [SerializeField] public List<int> pointsPerEnvironment = new();
    [SerializeField] public List<int> environmentWeights = new();  // weights // how many shots in each environment // default: 1,1,1,1 /// if weight set to 2 - this environment will have twice as many shots
    public List<List<Vector3>> clusterCentersLists = new();

    void Start()
    {
        //clusterCentersLists = CalculateClusterCenters();  // i will call it in randomizer in gui
    }

    public List<List<Vector3>> CalculateClusterCenters() {
        List<List<Vector3>> clusterCentersLists = new();
        
        for (int i = 0; i < environments.Count; i++) {
            List<Vector3> clusterCenters = GetEnvironmentClusterCenters(i);

            clusterCentersLists.Add(clusterCenters);  // cluster centers in 1 list belong to 1 environment
        }

        return clusterCentersLists;
    }

    public List<Vector3> GetEnvironmentClusterCenters(int i) {
        GameObject environment = environments[i];
        int pointsNr = pointsPerEnvironment[i];

        List<Vector3> clusterCenters = CalculateEnvironmentClusterCenters(environment, pointsNr);

        return clusterCenters;
    }

    public List<Vector3> CalculateEnvironmentClusterCenters(GameObject environment, int pointsNr) {
        List<Vector3> clusterCenters = new();

        Bounds bounds = GetEnvironmentBounds(environment);
        
        for (int i = 0; i < pointsNr; i++)  // Generate random points within the bounds
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);
            clusterCenters.Add(new Vector3(randomX, 0f, randomZ));
        }

        return clusterCenters;
    }

    private Bounds GetEnvironmentBounds(GameObject environment)
    {
        GameObject tempEnvironment = Instantiate(environment, Vector3.zero, Quaternion.identity);
        tempEnvironment.SetActive(false); // disable to keep it invisible

        Renderer[] renderers = tempEnvironment.GetComponentsInChildren<Renderer>();
        Destroy(tempEnvironment);

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }
}