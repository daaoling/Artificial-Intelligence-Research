using UnityEngine;
using System;
using System.Collections;

namespace PotentialField
{
    public class PFRadialPotentialField : PFPotentialField
    {

        public int potential;
        public int gradation;
        public int radius { get { return Mathf.FloorToInt(potential / gradation); } }


        public PFRadialPotentialField()
        {

        }

        public PFRadialPotentialField
            (PF_TYPE type, Vector3 point,
                int potential, int gradation)
                    : base(type, point)
        {
            this.potential = potential;
            this.gradation = gradation;
        }


        public override int potentialBoundsHalfHeight
        {
            get { return this.radius; }
        }
        public override int potentialBoundsHalfWidth
        {
            get { return this.radius; }
        }



        public override int getLocalPotential(int localX_offset, int localY_offset)
        {
            //int manhattan_distance = Mathf.Abs(localX_offset) + Mathf.Abs(localY_offset);
            int manhattan_distance = (int)new Vector2(Mathf.Abs(localX_offset), Mathf.Abs(localY_offset)).magnitude;

            if (manhattan_distance >= radius) return 0;

            switch (type)
            {
                case PF_TYPE.Attract: 
                    return Mathf.Min(0, -1 * (potential - gradation * manhattan_distance));
                case PF_TYPE.Repell:
                    return Mathf.Max(0,  1 * (potential - gradation * manhattan_distance));
                default:  
                    throw new Exception("this ? "+ type);
            }
        }
    }
}

