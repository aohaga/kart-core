using UnityEngine;

class DriftBrake : MonoBehaviour
{
    public bool keepSpeed;
    public float velAtDriftStart;
    public int directionAtDriftStart;
    public float driftDamping;

    public void beginDrift(float velocity, int wheelDirection)
    {
        velAtDriftStart = velocity - driftDamping;
        directionAtDriftStart = wheelDirection;
        keepSpeed = true;
    }

    public void releaseDrift()
    {
        keepSpeed = false;
        velAtDriftStart = 0;
        directionAtDriftStart = 0;
    }

    public Vector2 Get()
    {
        return new Vector3(velAtDriftStart, directionAtDriftStart);
    }
}
