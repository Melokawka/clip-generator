using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

// public class ClusterManager : MonoBehaviour
// {
//     [SerializeField]
//     GameObject ClusterPrefab;

//     [SerializeField]
//     public GlobalVars globalVars;

//     [SerializeField]
//     public GameObject treePrefab;

//     [SerializeField]
//     public int treesNr = 0;
//     private List<GameObject> spawnedClusters = new List<GameObject>();
    
//     void Start()
//     {
//         SpawnClusters();

//         RandomizeClusters();

//         //List<GameObject> spawnedTrees = SpawnTrees(spawnedClusters);
//     }
    
//     void SpawnClusters() {
//         GameObject obj;
//         List<Vector3> clusterCenters = globalVars.clusterCenters;

//         foreach (Vector3 spawn in clusterCenters) {
//             Vector3 randSpawn = spawn;
//             randSpawn.x += Random.Range(-5.0f, 5.0f);
//             randSpawn.z += Random.Range(-5.0f, 5.0f);

//             obj = Instantiate(ClusterPrefab, randSpawn, Quaternion.identity);
//             spawnedClusters.Add(obj);
//         }
//     }

//     public void RandomizeClusters() {
//         foreach (GameObject cluster in spawnedClusters) {
//             foreach (Transform child in cluster.GetComponent<Transform>()) {
//                 if (child.name == "Ground") continue;

//                 Vector3 pos = child.position;
//                 child.position = new Vector3(pos.x + Random.Range(-1f,1f), pos.y, pos.z + Random.Range(-1f,1f));
//             }
//         }
//     }

//     // List<GameObject> SpawnTrees(List<GameObject> spawnedClusters) {
//     //     List<GameObject> spawnedTrees = new List<GameObject>();
//     //     List<Bounds> clusterBounds = new List<Bounds>();

//     //     foreach (GameObject cluster in spawnedClusters) {
//     //         Transform transform = cluster.GetComponent<Transform>();
//     //         Transform ground = transform.Find("Ground");
//     //         MeshRenderer meshRenderer = ground.GetComponent<MeshRenderer>();
            
//     //         clusterBounds.Add(meshRenderer.bounds);
//     //         Debug.Log(meshRenderer.bounds);
//     //     }

//     //     for (int i = 0; i < treesNr; i++) {
//     //         Bounds bound = clusterBounds[Random.Range(0, clusterBounds.Count)];
//     //         Vector3 randPoint = new Vector3(bound.)
//     //         GameObject obj = Instantiate(ClusterPrefab, randSpawn, Quaternion.identity);
//     //         spawnedTrees.Add(obj);
//     //     }
        
//     //     return spawnedTrees;
//     // }
// }
