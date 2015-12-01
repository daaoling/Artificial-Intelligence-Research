using System;
using UnityEngine;
using System.Collections;

namespace PotentialField
{
    public class PFHorizontalPotentialField : PFPotentialField
    {
        private int _potential;
        private int _gradation;
        private int _halfWidth;

        public int potential { get { return _potential; } set { _potential = value; } }
        public int gradation { get { return _gradation; } set { _gradation = value; } }
        public int halfHeight { get { return Mathf.FloorToInt(_potential / _gradation); } }

        public PFHorizontalPotentialField
            (PF_TYPE type, Vector3 point,
                int _potential, int _gradation, int _halfWidth)
                : base(type, point)
        {
            this._potential = _potential;
            this._gradation = _gradation;
            this._halfWidth = _halfWidth;
        }


        public override int potentialBoundsHalfHeight{ get { return halfHeight; } }

        public override int potentialBoundsHalfWidth { get { return _halfWidth; } }

        public override int getLocalPotential(int localX, int localY)
        {
            if (Mathf.Abs(localX) > _halfWidth) return 0;

            switch (type)
            {
                case PF_TYPE.Attract:
                    return Mathf.Min(0, -1 * (_potential - _gradation * Math.Abs(localY)));
                case PF_TYPE.Repell:
                    return Mathf.Max(0,  1 * (_potential - _gradation * Math.Abs(localY)));
                default:
                    throw new Exception(" this " + type);
            }
        }
    }
}

