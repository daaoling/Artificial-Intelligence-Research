using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PotentialField
{
    public class PFDynamicPotentialsMap
    {
        public List<PFPotentialField> _fields = new List<PFPotentialField>();

        public void addPotentialField(PFPotentialField curField)
        {
            _fields.Add(curField);
        }

        public void removePotentialField(PFPotentialField curField)
        {
            _fields.Remove(curField);
        }

        public void removeAllPotentialFields()
        {
            _fields.Clear();
        }


        public int getPotential(int mapX, int mapY)
        {
            int potential = 0;

            foreach (PFPotentialField curField in _fields) {
                potential += curField.getLocalPotential(mapX - (int)curField.point.x, mapY - (int)curField.point.y);
            }
            
            return potential;
        }
    }
}
