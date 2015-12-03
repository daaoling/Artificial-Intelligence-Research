using UnityEngine;
using System.Collections;

namespace PotentialField
{
    public class BaseControl_2 : MonoBehaviour
    {
        public PFAgent agent;

        public PFAgent neighbour_agent;

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

                        if (curValue > 0)
                            Gizmos.color = Color.red;
                        else if (curValue == 0)
                            Gizmos.color = Color.white;
                        else
                            Gizmos.color = Color.green;

                        if (Mathf.Abs(center.x - agent.position.x) < 0.5f &&
                                Mathf.Abs(center.y - agent.position.y) < 0.5f) Gizmos.color = Color.black;

                        if (Mathf.Abs(center.x - neighbour_agent.position.x) < 0.5f &&
                                Mathf.Abs(center.y - neighbour_agent.position.y) < 0.5f) Gizmos.color = Color.white;

                        Gizmos.DrawCube(
                            new Vector3(i, j, curValue),
                                Vector3.one);
                    }
                }
            }
        }
    }
}

