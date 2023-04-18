using System.Collections.Generic;
using UnityEngine;

public class BoxFormation : FormationBase
{
    [SerializeField] private bool _hollow = false;
    [SerializeField] private float _nthOffset = 0;

    public override List<Vector3> EvaluatePoints(int _length, Vector3 center)
    {
        List<Vector3> positions = new List<Vector3>(_length);

        int rows = Mathf.RoundToInt(Mathf.Sqrt(_length));
        int cols = Mathf.CeilToInt((float)_length / (float)rows);

        var middleOffset = new Vector3(rows * 0.5f, 0, cols * 0.5f);

        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < cols; z++)
            {
                if (_hollow && x != 0 && x != rows - 1 && z != 0 && z != cols - 1) continue;
                var pos = new Vector3(x, 0, z);

                pos.x += (z % 2 == 0 ? 0 : _nthOffset);

                pos -= middleOffset;

                pos += GetNoise(pos);

                pos *= spread;

                pos += center;

                positions.Add(pos);
            }
        }

        return positions;
    }
}