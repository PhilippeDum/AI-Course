using System.Collections.Generic;
using UnityEngine;

public class BoxFormation : FormationBase
{
    //SerializeField][Range(0, 10)] private int _unitWidth = 5;
    //[SerializeField][Range(0, 10)] private int _unitDepth = 5;
    [SerializeField] private bool _hollow = false;
    [SerializeField] private float _nthOffset = 0;

    public override IEnumerable<Vector3> EvaluatePoints(int _unitWidth, int _unitDepth)
    {
        var middleOffset = new Vector3(_unitWidth * 0.5f, 0, _unitDepth * 0.5f);

        for (int x = 0; x < _unitWidth; x++)
        {
            for (int z = 0; z < _unitDepth; z++)
            {
                if (_hollow && x != 0 && x != _unitWidth - 1 && z != 0 && z != _unitDepth - 1) continue;
                var pos = new Vector3(x + (z % 2 == 0 ? 0 : _nthOffset), 0, z);

                pos -= middleOffset;

                pos += GetNoise(pos);

                pos *= spread;

                yield return pos;
            }
        }
    }

    public override Vector3 EvaluatePointsV2(int _unitWidth, int _unitDepth)
    {
        var middleOffset = new Vector3(_unitWidth * 0.5f, 0, _unitDepth * 0.5f);

        for (int x = 0; x < _unitWidth; x++)
        {
            for (int z = 0; z < _unitDepth; z++)
            {
                if (_hollow && x != 0 && x != _unitWidth - 1 && z != 0 && z != _unitDepth - 1) continue;
                var pos = new Vector3(x + (z % 2 == 0 ? 0 : _nthOffset), 0, z);

                pos -= middleOffset;

                pos += GetNoise(pos);

                pos *= spread;

                return pos;
            }
        }

        return Vector3.zero;
    }
}