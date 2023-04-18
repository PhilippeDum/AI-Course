using System.Collections.Generic;
using UnityEngine;

public abstract class FormationBase : MonoBehaviour
{
    [SerializeField][Range(0, 10)] protected float _noise = 0;
    [SerializeField][Range(0, 10)] protected float spread = 1;

    public abstract List<Vector3> EvaluatePoints(int _length, Vector3 center);

    public Vector3 GetNoise(Vector3 position)
    {
        var noise = Mathf.PerlinNoise(position.x * _noise, position.z * _noise);

        return new Vector3(noise, 0, noise);
    }
}