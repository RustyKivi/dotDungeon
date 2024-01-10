using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoLoader : MonoBehaviour
{
    private void Start() {
        GameManager.instance.selectedDungeon.Load();
    }
}
