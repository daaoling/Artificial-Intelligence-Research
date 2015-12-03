using System;
using UnityEngine;
using System.Collections;

namespace PotentialField
{
    public class PFHorizontalPotentialField : PFPotentialField
    {
        public int potential;
        public int gradation;
        public int halfWidth;


        public int halfHeight { get { return Mathf.FloorToInt(potential / gradation); } }

        public PFHorizontalPotentialField()
        {

        }
        public PFHorizontalPotentialField
            (PF_TYPE type, Vector3 point,
                int potential, int gradation, int halfWidth)
                : base(type, point)
        {
            this.potential = potential;
            this.gradation = gradation;
            this.halfWidth = halfWidth;
        }


        public override int potentialBoundsHalfHeight{ get { return halfHeight; } }

        public override int potentialBoundsHalfWidth { get { return halfWidth; } }

        public override int getLocalPotential(int localX, int localY)
        {
            if (Mathf.Abs(localX) > halfWidth) return 0;

            switch (type)
            {
                case PF_TYPE.Attract:
                    return Mathf.Min(0, -1 * (potential - gradation * Math.Abs(localY)));
                case PF_TYPE.Repell:
                    return Mathf.Max(0, 1 * (potential - gradation * Math.Abs(localY)));
                default:
                    throw new Exception(" this " + type);
            }
        }
    }
}

