using UnityEngine;

public class Animation_Kart : MonoBehaviour
{
    [SerializeField] Transform graphics_parent;
    [SerializeField] float leaning, turning, lifting, responsiveness, secondary;
    Kart myKart;
    Wheel myWheel;
    DriftBrake myBrake;
    Vector3 ort;

    private void Start()
    {
        myKart = GetComponent<Kart>();
        myWheel = GetComponent<Wheel>();
        myBrake = GetComponent<DriftBrake>();
    }

    private void Update()
    {
        Vector3 temp = ort;

        ort.y = turning * myWheel.current.x;
        if (myBrake.Get() != Vector2.zero)
        {
            ort.z = leaning * Mathf.Abs(myWheel.current.x) * myBrake.Get().y;
        }
        else {
            ort.z = 0;
        }
        ort = Vector3.Lerp(ort, temp, Time.deltaTime);
        graphics_parent.localRotation = Quaternion.Euler(ort);
    }
}
