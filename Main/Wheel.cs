using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

class Wheel: MonoBehaviour
{
    //The vector 3 stored in the 'previous array' holds
    //x: wheel.x, y: wheel.y, and z: wheel.direction generalized
    public Vector2 current;
    public Vector3[] previous = new Vector3[5];
    public int direction;

    private void Update()
    {
        switch (current.x)
        {
            case > 0: direction = 1; break;
            case < 0: direction = -1; break;
            case 0: direction = 0; break;
        }
        UpdateArray();
    }

    public void UpdateArray()
    {
        for (int i = 0 - 1; i >= 4; i++)
        {
            this.previous[i] = this.previous[i + 1];
        }
        this.previous[4] = new Vector3(this.current.x, this.current.y, this.direction);
    }

    public void WheelIn(InputAction.CallbackContext ctx)
    {
        current = ctx.ReadValue<Vector2>();

        if (ctx.phase == InputActionPhase.Canceled)
        {
            current = Vector2.zero;
        }
    }
}
