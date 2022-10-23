using System;
using UnityEngine;

[Serializable]
public struct ObjectProperties
{
    public ObjectProperties(Vector3 position, Vector3 eulers) {

        Position = new float[3] { position.x, position.y, position.z };
        Eulers = new float[3] { eulers.x, eulers.y, eulers.z };
    }

    public float[] Position { get; }
    public float[] Eulers { get; }

    
}
