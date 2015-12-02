using UnityEngine;
using System;
using System.Collections;

namespace PotentialField
{
    public class PFRadialPotentialField : PFPotentialField
    {
        private int _potential;
        private int _gradation;
        public int potential { get { return _potential; } set { _potential = value; } }
        public int gradation { get { return _gradation; } set { _gradation = value; } }
        public int radius { get { return Mathf.FloorToInt(_potential / _gradation); } }


        public PFRadialPotentialField()
        {

        }

        public PFRadialPotentialField
            (PF_TYPE type, Vector3 point,
                int _potential, int  _gradation)
                    : base(type, point)
        {
            this._potential = _potential;
            this._gradation = _gradation;
        }


        public override int potentialBoundsHalfHeight
        {
            get { return radius; }
        }
        public override int potentialBoundsHalfWidth
        {
            get { return radius; }
        }



        public override int getLocalPotential(int localX_offset, int localY_offset)
        {
            int manhattan_distance = Mathf.Abs(localX_offset) + Mathf.Abs(localY_offset);
            //int manhattan_distance = (int)new Vector2(Mathf.Abs(localX_offset), Mathf.Abs(localY_offset)).magnitude;

            if (manhattan_distance > radius) return 0;

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

