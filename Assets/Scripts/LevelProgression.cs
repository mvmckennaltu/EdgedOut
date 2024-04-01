using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    public static bool level1Complete = false;
    public static bool level2Complete = false;
    public static bool level3Complete = false;
    public static bool allLevelsComplete = false;
    public static int currentLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(level1Complete && level2Complete && level3Complete)
        {
            allLevelsComplete = true;
        }
    }
}
