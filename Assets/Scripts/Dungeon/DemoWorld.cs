using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWorld : Dungeon
{
    public override void Load()
    {
        base.Load();
        Debug.Log("Demo world...");
    }
}
