using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarControl : CarController {

    public override void Control()
    {
        //float[] input = new float[];

        move = 1;
        rotation = 0;
    }
}
