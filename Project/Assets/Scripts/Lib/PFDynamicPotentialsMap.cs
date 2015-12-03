using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PotentialField
{
    public class PFDynamicPotentialsMap
    {
        public Vector2 origin;
        public int width;
        public int height;

        public List<PFPotentialField> fields;

        public PFDynamicPotentialsMap(Vector2 origin, int width, int height)
        {
            this.origin = origin;
            this.width = width;
            this.height = height;
            this.fields = new List<PFPotentialField>();
        }


        public void addPotentialField(PFPotentialField curField)
        {
            fields.Add(curField);
        }

        public void removePotentialField(PFPotentialField curField)
        {
            fields.Remove(curField);
        }

        public void removeAllPotentialFields()
        {
            fields.Clear();
        }


        public int getPotential(int mapX, int mapY)
        {
            int potential = 0;

            foreach (PFPotentialField curField in fields) {
                potential += curField.getLocalPotential(mapX - (int)curField.position.x, mapY - (int)curField.position.y);
            }
            
            return potential;
        }
    }
}
