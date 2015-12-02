using UnityEngine;
using System.Collections;

namespace PotentialField
{
    public class PFRectangularPotentialField : PFPotentialField
    {
        public PFRectangularPotentialField(
            PF_TYPE type, Vector3 point, 
                int _potential, int _gradation, int _halfWidth, int _halfHeight)
                    : base(type, point)
        {

        }

        public override int potentialBoundsHalfHeight
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int potentialBoundsHalfWidth
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int getLocalPotential(int localX, int localY)
        {
            throw new System.NotImplementedException();
        }
    }
}

