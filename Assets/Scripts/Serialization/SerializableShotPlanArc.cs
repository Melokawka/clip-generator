[System.Serializable]
public class SerializableShotPlanArc
{
    public int id;
    public int environmentType;
    public int fps;
    public float duration;
    public int FOV;
    public ShotTypes shotType;
    public SerializableVector3 observedPoint;
    public SerializableVector3 cameraLocation;
    public int arcDegrees;

    public SerializableShotPlanArc(ShotPlanArc plan) {
        id = plan.id;
        environmentType = plan.environmentType;
        fps = plan.fps;
        duration = plan.duration;
        FOV = plan.FOV;
        shotType = plan.shotType;
        observedPoint = new SerializableVector3(plan.observedPoint);
        cameraLocation = new SerializableVector3(plan.cameraLocation);
        arcDegrees = plan.arcDegrees;
    }
}
