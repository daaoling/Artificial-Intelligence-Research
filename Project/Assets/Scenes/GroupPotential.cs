using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PotentialField;

public class GroupPotential : MonoBehaviour 
{
    public PFPotentialField[] obstacleFields;

    public PFPotentialField goalField;

    public PFAgent agent1;
    public PFAgent agent2;

    public PFStaticPotentialsMap obstaclesPotentialsMap;
    public PFDynamicPotentialsMap agent1Mp;
    public PFDynamicPotentialsMap agent2Mp;
    #region tmp Reference
    public Transform[] obstacles;

    public Transform goal;

    public Transform agent1_tran;
    public Transform agent2_tran;
    #endregion

    // Use this for initialization
	void Start () {
	    //create static map
        obstaclesPotentialsMap = new PFStaticPotentialsMap(MapConfig.origin, MapConfig.width, MapConfig.height);

        obstacleFields = new PFPotentialField[obstacles.Length];
        for (int i = 0; i < obstacleFields.Length; i++)
        {
            obstacleFields[i] = new PFRadialPotentialField()
            {
                type = PF_TYPE.Repell,
                position = obstacles[i].position,
                potential = 100,
                gradation = 10
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

        agent1 = new PFAgent() {
            type = PF_TYPE.Repell,
            position = agent1_tran.position,
            potential = 4,
            gradation = 1
        };
        

        agent2 = new PFAgent()
        {
            type = PF_TYPE.Repell,
            position = agent2_tran.position,
            potential = 4,
            gradation = 1
        };
        agent1_tran.gameObject.GetComponent<BaseControl_2>().agent = agent1;
        agent1_tran.gameObject.GetComponent<BaseControl_2>().neighbour_agent = agent2;

        agent2_tran.gameObject.GetComponent<BaseControl_2>().agent = agent2;
        agent2_tran.gameObject.GetComponent<BaseControl_2>().neighbour_agent = agent1;

        agent1Mp = new PFDynamicPotentialsMap(MapConfig.origin, MapConfig.width, MapConfig.height);
        agent1Mp.addPotentialField(goalField);
        agent1Mp.addPotentialField(agent2);


        agent2Mp = new PFDynamicPotentialsMap(MapConfig.origin, MapConfig.width, MapConfig.height);
        agent2Mp.addPotentialField(goalField);
        agent2Mp.addPotentialField(agent1);

        agent1.addStaticPotentialsMap(obstaclesPotentialsMap);
        agent1.addDynamicPotentialsMap(agent1Mp);


        agent2.addStaticPotentialsMap(obstaclesPotentialsMap);
        agent2.addDynamicPotentialsMap(agent2Mp);
       
    }


    void Update()
    {
        ///move to next position
        agent1.moveToPositionPoint(agent1.nextPosition());

        agent1_tran.transform.position = agent1.position;

        agent2.moveToPositionPoint(agent2.nextPosition());

        agent2_tran.transform.position = agent2.position;
    }
	    
}
