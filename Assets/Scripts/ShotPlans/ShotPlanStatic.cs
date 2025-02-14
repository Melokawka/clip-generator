using UnityEngine;
public class ShotPlanStatic
{
    public int id;
    public int environmentType;
    public int fps;
    public float duration;
    public int FOV;
    public ShotTypes shotType;
    public Vector3 observedPoint;
    public Vector3 cameraLocation;

    public ShotPlanStatic(int id, int environmentType, int fps, float duration, int FOV, ShotTypes shotType,
                            Vector3 observedPoint, Vector3 cameraLocation) {                 
        this.id = id;
        this.environmentType = environmentType;
        this.fps = fps;
        this.duration = duration;
        this.FOV = FOV;
        this.shotType = shotType;
        this.observedPoint = observedPoint;
        this.cameraLocation = cameraLocation;
    }
    
    public ShotPlanStatic(SerializableShotPlanStatic plan) {   
        id = plan.id;
        environmentType = plan.environmentType;
        fps = plan.fps;
        duration = plan.duration;
        FOV = plan.FOV;
        shotType = plan.shotType;
        observedPoint = plan.observedPoint.vector3;
        cameraLocation = plan.cameraLocation.vector3;
    }
}
