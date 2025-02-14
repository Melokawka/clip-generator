using UnityEngine;
public class ShotPlanArc  // add inheritance?
{
    public int id;
    public int environmentType;
    public int fps;
    public float duration;
    public int FOV;
    public ShotTypes shotType;
    public Vector3 observedPoint;
    public Vector3 cameraLocation;
    public int arcDegrees;

    public ShotPlanArc(int id, int environmentType, int fps, float duration, int FOV, ShotTypes shotType,
                            Vector3 observedPoint, Vector3 cameraLocation, int arcDegrees) {
        this.id = id;
        this.environmentType = environmentType;
        this.fps = fps;
        this.duration = duration;
        this.FOV = FOV;
        this.shotType = shotType;
        this.observedPoint = observedPoint;
        this.cameraLocation = cameraLocation;
        this.arcDegrees = arcDegrees;
    }

    public ShotPlanArc(SerializableShotPlanArc plan) {   // converter serializable->normal
        id = plan.id;
        environmentType = plan.environmentType;
        fps = plan.fps;
        duration = plan.duration;
        FOV = plan.FOV;
        shotType = plan.shotType;
        observedPoint = plan.observedPoint.vector3;
        cameraLocation = plan.cameraLocation.vector3;
        arcDegrees = plan.arcDegrees;
    }
}
