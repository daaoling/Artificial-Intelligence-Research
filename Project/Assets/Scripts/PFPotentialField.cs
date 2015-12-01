using UnityEngine;
using System.Collections;

namespace PotentialField
{
    public abstract class PFPotentialField 
    {
        public PF_TYPE type;
        public Vector3 point;

        public PFPotentialField()
        {

        }

        public PFPotentialField(PF_TYPE type, Vector3 point)
        {
            this.type = type;
            this.point = point;
        }


        public abstract int potentialBoundsHalfWidth { get; }
        public abstract int potentialBoundsHalfHeight { get; }
        public abstract int getLocalPotential(int localX, int localY);
    }
}

