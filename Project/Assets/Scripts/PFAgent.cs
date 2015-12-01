using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PotentialField
{
    public class PFAgent : PFRadialPotentialField
    {
        private List<PFDynamicPotentialsMap> dynamicPotentialsMaps = new List<PFDynamicPotentialsMap>();

        public Transform _trans;
        public Vector3 position { get { return _trans.position; } set { _trans.position = value; } }

        public PFAgent(
            PF_TYPE type, Vector3 point, int _potential, int _gradation, Transform _trans)
            : base(type, point, _potential, _gradation)
        {
            this._trans = _trans;
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

       public Vector3 nextPosition(int xOffset = 1, int yOffset = 1)
       {
            //int agentsPotential = dynamicPotentialsSum((int)position.x, (int)position.y);

            //int bestAttractPotential = agentsPotential;

            //return bestAttractPotential;

           Vector3 finalResult = position;

           for (int i = -1; i <= 1; i++)
           {
               for (int j = -1; j <= 1; j++)
               {
                   //Vector2 
                   var curMapX = (int)position.x + i * xOffset;
                   var curMapY = (int)position.y + j * yOffset;

                   //if(mMap.current.IsInBounds(new Vector2()))
               }
           }

           return finalResult;
       }


        #region Util
       public int GetAllPotential(int mapX, int mapY)
       {
           int agentsPotential = dynamicPotentialsSum(mapX, mapY);
           int curAllPotential = agentsPotential;
           return curAllPotential;
       }
        #endregion
    }
}

