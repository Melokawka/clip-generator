using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RecorderMain : MonoBehaviour
{
    [Header("General")]
    [SerializeField] public string savePath = "CapturedFrames";  // Folder to save images

    [Space(20)]
    [SerializeField] public int resolution = 224;  // constant and cannot be changed
    [SerializeField] public int batchSize = 5;  // TODO
    [SerializeField] public GlobalVars globalVars;


    public List<ShotPlanStatic> ReadShotPlansStatic() {
        string json = File.ReadAllText("static.txt");
        SerializableList<SerializableShotPlanStatic> serializablePlans = JsonUtility.FromJson<SerializableList<SerializableShotPlanStatic>>(json);
        List<SerializableShotPlanStatic> list = serializablePlans.list;

        List<ShotPlanStatic> plans = new();
        foreach(SerializableShotPlanStatic plan in list) {
            plans.Add(new ShotPlanStatic(plan));
        }
        return plans;
    }

    public List<ShotPlanArc> ReadShotPlansArc() {
        string json = File.ReadAllText("arc.txt");
        SerializableList<SerializableShotPlanArc> serializablePlans = JsonUtility.FromJson<SerializableList<SerializableShotPlanArc>>(json);
        List<SerializableShotPlanArc> list = serializablePlans.list;
        
        List<ShotPlanArc> plans = new();
        foreach(SerializableShotPlanArc plan in list) {
            plans.Add(new ShotPlanArc(plan));
        }
        return plans;
    }

    public List<List<ShotPlanStatic>> MakeBatchesStatic(List<ShotPlanStatic> shotPlansStatic) {
        int index = 0;
        List<List<ShotPlanStatic>> batches = new();
        List<ShotPlanStatic> batch;

        while (index < shotPlansStatic.Count) {
            batch = new();
            int environmentNr = shotPlansStatic[index].environmentType;
            int limit = index + batchSize;  // yes its necessary - loop updates the value in each iteration
            for (; index < limit; index++) {
                if (shotPlansStatic[index].environmentType != environmentNr) break;
                batch.Add(shotPlansStatic[index]);
            }
            batches.Add(batch);
        }

        return batches;
    }

    public List<List<ShotPlanArc>> MakeBatchesArc(List<ShotPlanArc> shotPlansArc) {
        int index = 0;
        List<List<ShotPlanArc>> batches = new();
        List<ShotPlanArc> batch;

        while (index < shotPlansArc.Count) {
            batch = new();
            int environmentNr = shotPlansArc[index].environmentType;
            int limit = index + batchSize;  // yes its necessary - loop updates the value in each iteration
            for (; index < limit; index++) {
                if (shotPlansArc[index].environmentType != environmentNr) break;
                batch.Add(shotPlansArc[index]);
            }
            batches.Add(batch);
        }

        return batches;
    }

    public async void DoStaticShots() {  // at this moment if one shotPlan is bad and needs fixing, we need to redo the whole recording - how to fix this?
        List<ShotPlanStatic> shotPlansStatic = ReadShotPlansStatic();
        List<List<ShotPlanStatic>> batches = MakeBatchesStatic(shotPlansStatic);

        for (int j = 0; j < batches.Count; j++) {
            List<ShotPlanStatic> batch = batches[j];
            
            GameObject environment = Instantiate(globalVars.environments[batch[0].environmentType], Vector3.zero, Quaternion.identity);

            for (int i = 0; i < batch.Count; i++) {
                ShotPlanStatic shotPlan = batch[i];

                GameObject cameraObject = new GameObject($"StaticCamera_{shotPlan.id}");
                Camera camera = cameraObject.AddComponent<Camera>();
                camera.transform.position = shotPlan.cameraLocation;

                camera.transform.LookAt(shotPlan.observedPoint);

                camera.fieldOfView = shotPlan.FOV;

                RecorderStatic recorder = cameraObject.AddComponent<RecorderStatic>();
                if (i < batch.Count-1) {
                    recorder.Record(shotPlan.fps, resolution, shotPlan.duration,
                                    Path.Combine(savePath, $"StaticCamera_{shotPlan.id}"));
                }
                else await recorder.Record(shotPlan.fps, resolution, shotPlan.duration,
                                           Path.Combine(savePath, $"StaticCamera_{shotPlan.id}"));
            }

            Destroy(environment);
        }
    }

    public async void DoArcShots() {
        List<ShotPlanArc> shotPlansArc = ReadShotPlansArc();
        List<List<ShotPlanArc>> batches = MakeBatchesArc(shotPlansArc);
        
        for (int j = 0; j < batches.Count; j++) {
            List<ShotPlanArc> batch = batches[j];

            GameObject environment = Instantiate(globalVars.environments[batch[0].environmentType], Vector3.zero, Quaternion.identity);

            for (int i = 0; i < batch.Count; i++) {
                ShotPlanArc shotPlan = batch[i];

                GameObject cameraObject = new GameObject($"ArcCamera_{shotPlan.id}");
                Camera camera = cameraObject.AddComponent<Camera>();
                camera.transform.position = shotPlan.cameraLocation;

                camera.transform.LookAt(shotPlan.observedPoint);
                
                camera.fieldOfView = shotPlan.FOV;

                GameObject pivot = new GameObject($"ArcPivot_{shotPlan.id}");
                pivot.transform.position = shotPlan.observedPoint;

                cameraObject.transform.parent = pivot.transform;  // Parent camera to pivot
                
                RecorderArc recorder = cameraObject.AddComponent<RecorderArc>();

                float rotationSpeed = 360f / shotPlan.duration * ((float)shotPlan.arcDegrees / 360f);

                if (i < batch.Count-1) {    
                    recorder.Record(shotPlan.fps, resolution, shotPlan.duration,
                                    Path.Combine(savePath, $"ArcCamera_{shotPlan.id}"), rotationSpeed);
                }
                else {
                    await recorder.Record(shotPlan.fps, resolution, shotPlan.duration,
                                    Path.Combine(savePath, $"ArcCamera_{shotPlan.id}"), rotationSpeed);
                }
            }

        }
    }

    void Start()
    {
    }
}
