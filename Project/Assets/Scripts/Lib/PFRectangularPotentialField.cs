using UnityEngine;
using System.Collections;

namespace PotentialField
{
    public class PFRectangularPotentialField : PFPotentialField
    {
        public int potential;
        public int gradation;
        public int halfWidth;
        public int halfHeight;

        public PFRectangularPotentialField()
        {

        }
        public PFRectangularPotentialField(
            PF_TYPE type, Vector3 point, 
                int potential, int gradation, int halfWidth, int halfHeight)
                    : base(type, point)
        {
            this.potential = potential;
            this.gradation = gradation;
            this.halfWidth = halfWidth;
            this.halfHeight = halfHeight;
        }

        public int boundsHalfWidth { get { return this.halfWidth + Mathf.FloorToInt(this.potential / this.gradation); } }
        public int boundsHalfHeight { get { return this.halfHeight + Mathf.FloorToInt(this.potential / this.gradation); } }

        public override int potentialBoundsHalfWidth
        {
            get { return this.boundsHalfWidth; }
        }

        public override int potentialBoundsHalfHeight
        {
            get { return this.boundsHalfHeight; }
        }


        public override int getLocalPotential(int localX, int localY)
        {
            //if (Mathff.Abs(localX) > halfWidth || Mathff.Abs(localY) > halfHeight) return 0;
            ////    return (int)this.type * potential;
            ////else
            ////    return 0;

            //switch (this.type)
            //{
            //    case PF_TYPE.Attract:
            //        return Mathff.Min(0, -1 * potential);
            //    case PF_TYPE.Repell:
            //        return Mathff.Max(0, 1 * potential);
            //    default:
            //        throw new System.Exception("this ? " + this.type);
            //}

            if (Mathf.Abs(localX) < this.halfWidth && Mathf.Abs(localY) < this.halfHeight) return (int)type * potential;

            if (Mathf.Abs(localX) < potentialBoundsHalfWidth && Mathf.Abs(localY) < potentialBoundsHalfHeight)
            {
                int distance = 0;

                if (Mathf.Abs(localX) > halfWidth && Mathf.Abs(localY) > halfHeight)
                {
                    distance = Mathf.Abs(localX) - halfWidth + Mathf.Abs(localY) - halfHeight;
                }
                else if (Mathf.Abs(localX) > halfWidth)
                {
                    distance = Mathf.Abs(localX) - halfWidth;
                }
                else if (Mathf.Abs(localY) > halfHeight)
                {
                    distance = Mathf.Abs(localY) - halfHeight;
                }


                switch (this.type)
                {
                    case PF_TYPE.Attract:
                        return Mathf.Min(0, -1 * (potential - gradation * distance));
                    case PF_TYPE.Repell:
                        return Mathf.Max(0, 1 * (potential - gradation * distance));
                    default:
                        throw new System.Exception("this ? " + this.type);
                }
            }
                

            return 0;
        }
    }
}

