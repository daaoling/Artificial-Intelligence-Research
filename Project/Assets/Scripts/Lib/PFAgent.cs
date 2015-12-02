using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PotentialField
{
    public class PFAgent : PFRadialPotentialField
    {
        private List<PFDynamicPotentialsMap> dynamicPotentialsMaps = new List<PFDynamicPotentialsMap>();

        public Vector2 position;


        public PFAgent()
        {

        }

        public PFAgent(
            PF_TYPE type, Vector3 point, int _potential, int _gradation, Vector3 _position)
            : base(type, point, _potential, _gradation)
        {
            this.position = _position;
        }

        public void addDynamicPotentialsMap(PFDynamicPotentialsMap curMap)
        {
            dynamicPotentialsMaps.Add(curMap);
        }

      
        /// <summary>
        /// 越小越好
        /// </summary>
        /// <param name="map_x"></param>
        /// <param name="map_y"></param>
        /// <returns></returns>
        private int dynamicPotentialsSum(int map_x, int map_y)
        {
            int sum = 0;
            for (int i = 0; dynamicPotentialsMaps != null && i < dynamicPotentialsMaps.Count; i++)
            {
                sum += dynamicPotentialsMaps[i].getPotential(map_x, map_y);
            }
            return sum;
        }

        public Vector2 nextPosition(int xOffset = 10, int yOffset = 10)
        {
            Vector2 finalResult = position;
            int bestPotential = GetAllPotential(position);

            for (int z = 1; z <= xOffset; z++)
            {
                for (int z1 = 1; z1 <= yOffset; z1++)
                {
                    #region find best position
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (i == 0 && j == 0) continue;

                            Vector2 newPos = new Vector2(position.x + i * z, position.y + j * z1);

                            if (!dynamicPotentialsMaps[0].IsInBound(newPos)) continue;

                            int tmpPotential = GetAllPotential(newPos);

                            if (tmpPotential < bestPotential)
                            {
                                finalResult = newPos;
                                bestPotential = tmpPotential;
                            }
                        }
                    }
                    #endregion

                    if (position != finalResult) break;
                }
            }

            return finalResult;
        }

        public void moveToPositionPoint(float x, float y)
        {
            moveToPositionPoint(new Vector2(x,y));
        }
        public void moveToPositionPoint(Vector2 newPos)
        {
            position = newPos;
        }

        #region Util
       public int GetAllPotential(Vector2 curPos)
       {
           int agentsPotential = dynamicPotentialsSum((int)curPos.x, (int)curPos.y);
           
           int curAllPotential = agentsPotential;
           
           return curAllPotential;
       }
        #endregion
    }
}

