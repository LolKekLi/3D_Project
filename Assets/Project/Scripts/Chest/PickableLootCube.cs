using UnityEngine;

public class PickableLootCube : AbstarctPickableLootObject
{
    protected override void Collect()
    {
        Debug.LogError("1234567890");
    }
}