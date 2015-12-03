using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PotentialField
{
    public class PFAgentTrail
    {
        public int worldX;
        public int worldY;
        public int potential;

        public PFAgentTrail(int worldX, int worldY, int potential)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.potential = potential;
        }
    }
    public class PFAgent : PFRadialPotentialField
    {
        private List<PFStaticPotentialsMap> staticPotentialMaps = new List<PFStaticPotentialsMap>();

        private List<PFDynamicPotentialsMap> dynamicPotentialsMaps = new List<PFDynamicPotentialsMap>();

        public List<PFAgentTrail> trails = new List<PFAgentTrail>();

        public PFAgent()
        {

        }

        public PFAgent(
            PF_TYPE type, Vector3 position, int potential, int gradation)
            : base(type, position, potential, gradation)
        {

        }

        public void addStaticPotentialsMap(PFStaticPotentialsMap curMap)
        {
            staticPotentialMaps.Add(curMap);
        }
        public void addDynamicPotentialsMap(PFDynamicPotentialsMap curMap)
        {
            dynamicPotentialsMaps.Add(curMap);
        }

      


        /// <summary>
        /// may be bigger than biggest obstacle
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        public Vector2 nextPosition(int mutiply = 10)
        {
            Vector2 finalResult = position;
            int bestPotential = GetAllPotential(position);

            #region ready improve
            for (int z = 1; z <= mutiply; z++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        Vector2 newPos = new Vector2(position.x + i * mutiply, position.y + j * mutiply);

                        if (!MapConfig.isInBound(newPos)) continue;

                        int tmpPotential = GetAllPotential(newPos);

                        if (tmpPotential < bestPotential)
                        {
                            finalResult = newPos;
                            bestPotential = tmpPotential;
                        }
                    }
                }
                if ((Vector2)position != finalResult) break;
            }
            #endregion

            //if (finalResult == (Vector2)position)
            //{
            //    if (trails.Count > 4)
            //    {
            //        trails.RemoveAt(0);
            //    }

            //    trails.Add(new PFAgentTrail((int)this.position.x, (int)this.position.y, 1 * potential));
            //}

            return finalResult;
        }

        public void moveToPositionPoint(float x, float y)
        {
            moveToPositionPoint(new Vector2(x,y));
        }
        public void moveToPositionPoint(Vector2 newPos)
        {
           this.position = newPos;
        }

        #region Util
       public int GetAllPotential(Vector2 curPos)
       {
           int staticPotential = staticPotentialsSum((int)curPos.x, (int)curPos.y);
           int agentsPotential = dynamicPotentialsSum((int)curPos.x, (int)curPos.y);
           //int trailPotential = getTrailPotential((int)curPos.x, (int)curPos.y);

           int curAllPotential
               = staticPotential
                    + agentsPotential;
                        //+ trailPotential;
           
           return curAllPotential;
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
       private int staticPotentialsSum(int map_x, int map_y)
       {
           int sum = 0;
           for (int i = 0; staticPotentialMaps != null && i < staticPotentialMaps.Count; i++)
           {
               sum += staticPotentialMaps[i].getPotential(map_x, map_y);
           }
           return sum;
       }

       private int getTrailPotential(int map_x, int map_y)
       {
           int potential = 0;
           for (int i = 0; trails!= null && i < trails.Count; i++)
           {
               if(trails[i].worldX == map_x && trails[i].worldY == map_y)
               {
                   potential += trails[i].potential;
               }
           }
           return potential;
       }
       #endregion
    }
}

