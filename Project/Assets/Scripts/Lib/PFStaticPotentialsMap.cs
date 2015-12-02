using UnityEngine;
using System.Collections;


namespace PotentialField
{
    public class PFStaticPotentialsMap
    {
        public Vector2 origin;
        public int width;
        public int height;

        public int[][] map;

        public PFStaticPotentialsMap(Vector2 origin, int width, int height)
        {
            this.origin = origin;
            this.width = width;
            this.height = height;

            for (int i = 0; i < width; i++)
            {
                map[i] = new int[height];
            }
        }

        public void addPotentialField(PFPotentialField field)
        {
            addPotentail(field, 1);
        }
        public void removePotentialField(PFPotentialField field)
        {
            addPotentail(field, -1);
        }
        private void addPotentail(PFPotentialField field, int multiplier)
        {
            for (int i = (int) Mathf.Max(origin.x, (field.position.x - field.potentialBoundsHalfWidth)); 
                    i < Mathf.Min((origin.x + width), (field.position.x + field.potentialBoundsHalfWidth)); i++)
            {
                for (int j = (int)Mathf.Max(origin.y, (field.position.y - field.potentialBoundsHalfHeight));
                    j < Mathf.Min((origin.y + height), (field.position.y + field.potentialBoundsHalfHeight)); j++)
                {
                    map[i][j] = multiplier * field.getLocalPotential(i - (int)field.position.x, j - (int)field.position.y);
                }
            }
        }

        public int getPotential(int x, int y)
        {
            return map[x][y];
        }

        public bool IsInBound(Vector2 curPos)
        {
            return
                curPos.x <= (origin.x + width)
                    && curPos.x >= (origin.x)
                        && curPos.y <= (origin.y + height)
                            && curPos.y >= (origin.y);
        }
    }
}

