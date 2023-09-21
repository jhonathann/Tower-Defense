using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controlls the main directional light rotation
/// </summary>
public class LightController : MonoBehaviour
{
    private const float ROTATION_SPEED = 1.0f;
    void FixedUpdate()
    {
        ApplyRotation();
    }

    /// <summary>
    /// Aplies the rotation (this is what causes the slight wiggli shadow movement, having the shadow off center (non zero values for z rotation and x,y,z position) helps. It seems like an engine limitation)
    /// </summary>
    private void ApplyRotation()
    {
        this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + ROTATION_SPEED * Time.deltaTime, transform.rotation.eulerAngles.z);
    }
}
