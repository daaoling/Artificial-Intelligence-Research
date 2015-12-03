using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PotentialField
{
    public class Performance : MonoBehaviour
    {
        public static Performance current;

        public PFStaticPotentialsMap obstaclesPotentialsMap;
        public PFPotentialField[] obstacleFields;
   

        public PFPotentialField goalField;

        #region tmp Model
        public BoxCollider2D[] obstacle;
        
        public Transform goal;
        
        public List<BaseControl_3> agents = new List<BaseControl_3>();
        #endregion

        void Awake()
        {
            current = this;

            //create static map
            obstaclesPotentialsMap = new PFStaticPotentialsMap(MapConfig.origin, MapConfig.width, MapConfig.height);

            obstacleFields = new PFPotentialField[obstacle.Length];
            for (int i = 0; i < obstacleFields.Length; i++)
            {
                obstacleFields[i] = new PFRectangularPotentialField()
                {
                    type = PF_TYPE.Repell,
                    position = obstacle[i].center,
                    potential = 20,
                    gradation = 10,
                    halfWidth = (int)(obstacle[i].size.x * 0.5f),
                    halfHeight = (int)(obstacle[i].size.y * 0.5f)
                };

                Debug.Log((obstacleFields[i] as PFRectangularPotentialField).halfWidth
                    + " : "
                        + (obstacleFields[i] as PFRectangularPotentialField).halfHeight);

                obstaclesPotentialsMap.addPotentialField(obstacleFields[i]);
            }

            goalField = new PFRadialPotentialField()
            {
                type = PF_TYPE.Attract,
                position = goal.position,
                potential = MapConfig.width + MapConfig.height,
                gradation = 1
            };
        }

        void OnDrawGizmos()
        {
            DrawRect(MapConfig.origin, MapConfig.width, MapConfig.height);

            if (Application.isPlaying)
            {
                for (int i = 0; i < obstacle.Length; i++)
                {
                    DrawRect(
                        obstacle[i].center - Vector2.up * obstacle[i].size.y * 0.5f - Vector2.right * obstacle[i].size.x * 0.5f,
                            (int)obstacle[i].size.x, (int)obstacle[i].size.y);
                }
            }
        }

        void DrawRect(Vector3 origin,int width,int height)
        {
            //顺时针
            Vector3 start = origin;
            Vector3 end = start + Vector3.right * width;
            Debug.DrawLine(start, end, Color.red);

            start = end;
            end = start + Vector3.up * height;
            Debug.DrawLine(start, end, Color.red);

            start = end;
            end = start + Vector3.left * width;
            Debug.DrawLine(start, end, Color.red);

            start = end;
            end = start + Vector3.down * height;
            Debug.DrawLine(start, end, Color.red);
        }
        
    }
}

