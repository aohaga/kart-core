using UnityEngine;

[System.Serializable]
class Tunings
{
    public float acceleration; // how many "seconds" to top speed; Multiplied by 50 (or,fixedUpdate fps) when engine starts
    public float top_speed; // how fast it can go
    public float kick_percentage; //the percent of the top speed kart will begin at when gas is applied

    public float handling;
    public float slidyness;
    public float stop_tolerance;

    public AnimationCurve engine;

    public void StartEngine()
    {
        //Finalize values
        acceleration *= 50;

        //Make engine slope: weight mode settings force animation curve to be linear.
        engine.AddKey(new Keyframe(-2, -2) { weightedMode = WeightedMode.Both, inWeight = 0, outWeight = 0 });
        engine.AddKey(new Keyframe(0, 0) { weightedMode = WeightedMode.Both, inWeight = 0, outWeight = 0 });
        engine.AddKey(new Keyframe(1, top_speed * kick_percentage) { weightedMode = WeightedMode.Both, inWeight = 0, outWeight = 0 }); ;
        engine.AddKey(new Keyframe(acceleration, top_speed) { weightedMode = WeightedMode.Both, inWeight = 0, outWeight = 0 }); ;
        //engine.AddKey(new Keyframe(limit, top_speed) { weightedMode = WeightedMode.Both, inWeight = 0, outWeight = 0 }); ;
    }

}
