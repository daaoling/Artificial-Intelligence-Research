using UnityEngine;
using System.Collections;


namespace PotentialField
{
    public class HelloPotential : MonoBehaviour
    {
        public PFRadialPotentialField attract_field;

        public PFDynamicPotentialsMap map;
        
        public PFAgent agent;
        void Start() 
        {
            Init();
        }



        public void Init()
        {
            ///radius must cover all map
            attract_field = new PFRadialPotentialField() {  
                type = PF_TYPE.Attract, 
                point = Vector3.zero,
                potential = 250,
                gradation = 5  
            };

            map = new PFDynamicPotentialsMap();
            map.addPotentialField(attract_field);
        }

    }
}

