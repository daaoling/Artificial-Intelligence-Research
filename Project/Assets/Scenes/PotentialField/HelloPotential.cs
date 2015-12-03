using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PotentialField;

public class HelloPotential : MonoBehaviour
{
    //public static HelloPotential current;

    //public static int width = 150;
    //public static int height = 43;
    //public static int diaonal = (int)Mathf.Sqrt(150 * 150 + 43 * 43);

    public PFDynamicPotentialsMap curMap;

    public PFAgent agent;

    public PFRadialPotentialField followTargetPotential;
    public PFRadialPotentialField obstaclePotential;
    public PFRadialPotentialField obstaclePotential_1;
    public PFRadialPotentialField obstaclePotential_2;
    #region Model
    //tmp reference view

    public Transform bot;

    public Transform goal;
    public Transform obstacle;
    public Transform obstacle_1;
    public Transform obstacle_2;
    #endregion

    void Awake()
    {

    }
    
    void Start()
    {
        //Debug.Log(" width :" + width + " height:" + height + " diaonal:" + diaonal);

        //add attractive field
        followTargetPotential = new PFRadialPotentialField()
        {
             type = PF_TYPE.Attract,
             position = goal.transform.position,
             potential = MapConfig.diaonal,
             gradation = 1
        };

        //add obstacle field
        obstaclePotential = new PFRadialPotentialField()
        {
            type = PF_TYPE.Repell,
            position = obstacle.transform.position,
            potential = 100,
            gradation = 10
        };
        obstaclePotential_1 = new PFRadialPotentialField()
        {
            type = PF_TYPE.Repell,
            position = obstacle_1.transform.position,
            potential = 100,
            gradation = 10
        };
        obstaclePotential_2 = new PFRadialPotentialField()
        {
            type = PF_TYPE.Repell,
            position = obstacle_2.transform.position,
            potential = 100,
            gradation = 10
        };


        curMap = new PFDynamicPotentialsMap(MapConfig.origin, MapConfig.width, MapConfig.height);
        curMap.addPotentialField(followTargetPotential);
        curMap.addPotentialField(obstaclePotential);
        curMap.addPotentialField(obstaclePotential_1);
        curMap.addPotentialField(obstaclePotential_2);

        //set agent and register maps
        agent = new PFAgent(){
            position = bot.transform.position
        };
        agent.addDynamicPotentialsMap(curMap);
    }
    

    void Update()
    {
        ///move to next position
        agent.moveToPositionPoint(agent.nextPosition());

        bot.transform.position = agent.position;
    }
    bool First = true;
    
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            //List<Vector3> points = new List<Vector3>();

            for (int i = 0; i < MapConfig.width; i++)
            {
                for (int j = 0; j < MapConfig.height; j++)
                {
                    Vector3 center = new Vector3(i + 0.5f, j + 0.5f, 0);

                    int curValue = agent.GetAllPotential(center);

                    //points.Add(new Vector3(center.x, center.y, curValue));
                    if (curValue > 0) 
                        Gizmos.color = Color.red;
                    else if (curValue == 0) 
                        Gizmos.color = Color.white;
                    else 
                        Gizmos.color = Color.green;

                    if (Mathf.Abs(center.x - agent.position.x) < 0.5f && 
                            Mathf.Abs(center.y - agent.position.y) < 0.5f) Gizmos.color = Color.black;

                    Gizmos.DrawCube(
                        new Vector3(i,j, curValue),
                            Vector3.one);
                }
            }
        }
    }

}


