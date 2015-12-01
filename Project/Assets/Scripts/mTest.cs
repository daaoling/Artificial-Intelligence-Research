using UnityEngine;
using System.Collections;

public class mTest : MonoBehaviour {

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button")) Example();
            

    }

    void Example()
    {
        print(Mathf.Pow(3,2));
    }
}
