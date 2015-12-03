using UnityEngine;
using System.Collections;


namespace PotentialField
{
    public class BaseControl_3 : MonoBehaviour
    {
        public PFAgent curAgent;
        public PFDynamicPotentialsMap agentsMap;

        void Awake()
        {
            curAgent = new PFAgent()
            {
                type = PF_TYPE.Repell,
                position = transform.position,
                potential = 4,
                gradation = 1
            };
        }

        void Start()
        {
            agentsMap = new PFDynamicPotentialsMap(MapConfig.origin, MapConfig.width, MapConfig.height);
            agentsMap.addPotentialField(Performance.current.goalField);
            for (int i = 0; i < Performance.current.agents.Count; i++)
            {
                if (Performance.current.agents[i].curAgent != this.curAgent)
                {
                    agentsMap.addPotentialField(Performance.current.agents[i].curAgent);
                }
            }
            

            curAgent.addStaticPotentialsMap(Performance.current.obstaclesPotentialsMap);
            curAgent.addDynamicPotentialsMap(agentsMap);
        }
        
        void Update()
        {
            curAgent.moveToPositionPoint(curAgent.nextPosition());
            transform.position = curAgent.position;
        }

        void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                for (int i = 0; i < MapConfig.width; i++)
                {
                    for (int j = 0; j < MapConfig.height; j++)
                    {
                        Vector3 center = new Vector3(i + 0.5f, j + 0.5f, 0);

                        int curValue = curAgent.GetAllPotential(center);

                        if (curValue > 0)
                            Gizmos.color = Color.red;
                        else if (curValue == 0)
                            Gizmos.color = Color.white;
                        else
                            Gizmos.color = Color.green;

                        if (Mathf.Abs(center.x - curAgent.position.x) < 0.5f &&
                                Mathf.Abs(center.y - curAgent.position.y) < 0.5f) Gizmos.color = Color.black;

                        Gizmos.DrawCube(
                            new Vector3(i, j, curValue),
                                Vector3.one);
                    }
                }
            }
        }
    }
}

