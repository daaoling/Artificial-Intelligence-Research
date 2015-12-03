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

            map = new int[width][];
            for (int i = 0; i < width; i++)
            {
                map[i] = new int[height];
            }
        }

        public void addPotentialField(PFPotentialField curfield)
        {
            addPotentail(curfield, 1);
        }
        public void removePotentialField(PFPotentialField curfield)
        {
            addPotentail(curfield, -1);
        }
        private void addPotentail(PFPotentialField curfield, int multiplier)
        {
            for (int i = (int)Mathf.Max(origin.x, (curfield.position.x - curfield.potentialBoundsHalfWidth));
                    i < Mathf.Min((origin.x + width), (curfield.position.x + curfield.potentialBoundsHalfWidth)); i++)
            {
                for (int j = (int)Mathf.Max(origin.y, (curfield.position.y - curfield.potentialBoundsHalfHeight));
                    j < Mathf.Min((origin.y + height), (curfield.position.y + curfield.potentialBoundsHalfHeight)); j++)
                {
                    map[i][j] += multiplier * curfield.getLocalPotential(i - (int)curfield.position.x, j - (int)curfield.position.y);
                }
            }


        }

        public int getPotential(int x, int y)
        {
            return map[x][y];
        }
    }
}

