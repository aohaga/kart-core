using UnityEngine;
using UnityEngine.InputSystem;

public class Kart : MonoBehaviour
{
    //To Do : !!!
    //General Cleanup. 
    //Get the update loop concise and easy to read
    //Move drift input to its component?

    [SerializeField] Tunings myTunings = new Tunings();
    DriftBrake driftbrake;
    Wheel wheel;

    float gas, brake;
    float spedometer;
    bool drifting;

    float speed, steering;
    Vector3 momentum; //The vector to which translation will be applied
    Vector3 momentumMemory; 

    private void Start()
    {
        myTunings.StartEngine();
        wheel = GetComponent<Wheel>();
        driftbrake = GetComponent<DriftBrake>();
    }

    //Input Read (wheel reading happens in its own monobehaviour)
    public void GasIn(InputAction.CallbackContext ctx)
    {
        gas = ctx.ReadValue<float>();
    }

    public void BrakeIn(InputAction.CallbackContext ctx)
    {
        brake = ctx.ReadValue<float>();
    }

    public void DiftBrake(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && drifting == false && speed > 2)
        {
            drifting = true;
            driftbrake.beginDrift(speed, wheel.direction);
        }
        if (ctx.phase == InputActionPhase.Performed && drifting == true) {
            driftbrake.keepSpeed = false;
        }
        if (ctx.phase == InputActionPhase.Canceled && drifting == true)
        {
            drifting = false;
            driftbrake.releaseDrift();
        }
    }

    private void Update()
    {
        speed = engineValue();
        steering = steeringValue();

        if (speed > 0.1)
        {
            //Rotate kart
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + steering, 0), Time.deltaTime);

            //Process momentum
            float p = Mathf.InverseLerp(0, myTunings.top_speed / 2, speed);
            float slide = Mathf.Lerp(1, myTunings.slidyness, p);
            momentumMemory = momentum;
            momentum = Vector3.Lerp(momentum, transform.forward, Time.deltaTime / slide);

            //Check for something in the path of motion
            RaycastHit outFront;
            if (Physics.Raycast(transform.position, momentum, out outFront, speed * Time.deltaTime))
            {
                //Something is there
            }
            else
            {
                //Nothing is there
                transform.Translate(momentum * speed * Time.deltaTime, Space.World);
            }
        }
    }

    //Retrieves value from engine curve
    float engineValue()
    {
        if (gas > 0)
        {
            spedometer += gas;
            spedometer -= brake * 1.5f;
            spedometer = Mathf.Clamp(spedometer, 0, myTunings.acceleration);
            return myTunings.engine.Evaluate(spedometer);
        }
        else
        {
            if (speed > 0)
            {
                float t = speed;
                t = Mathf.Lerp(t, 0, Time.deltaTime);
                t -= brake * 0.7f;
                if (t < myTunings.stop_tolerance) { t = 0; }

                float j = Mathf.InverseLerp(myTunings.top_speed, 0, t);
                spedometer = Mathf.Lerp(myTunings.acceleration, 0, j);

                return t;
            }
            return 0;
        }
    }

    //Retrieves value from wheel and drift
    float steeringValue()
    {
        float s;
        if (drifting)
        {
            Vector2 data = driftbrake.Get();
            if (driftbrake.keepSpeed && gas > 0)
            {
                speed = data.x;
            }
            float control = ((int)data.y == 1) ? ExtensionMethods.Remap(wheel.current.x, -1, 1, 0, 2) : ExtensionMethods.Remap(wheel.current.x, -1, 1, 2, 0);
            s = (int)data.y * control;
        }
        else
        {
            s = wheel.direction * Mathf.Abs(wheel.current.x);
        }
        return s * myTunings.handling;
    }

}
