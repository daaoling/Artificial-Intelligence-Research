﻿using UnityEngine;
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
        public CircleCollider2D[] obstacle;
        
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
                obstacleFields[i] = new PFRadialPotentialField()
                {
                    type = PF_TYPE.Repell,
                    position = obstacle[i].transform.position,
                    potential = 60,
                    gradation = 10,
                };
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
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(obstacle[i].transform.position, obstacle[i].radius);
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

