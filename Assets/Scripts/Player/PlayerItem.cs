using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public string Name;
    public int Damage;
    [Space]
    public Type None;
    public GameObject worldObject;

    private bool equipt = false;

    public void equiptFunction(bool isTrue)
    {
        if(isTrue == true)
        {
            equipt = true;
            worldObject.SetActive(true);
        }else{
            equipt = false;
            worldObject.SetActive(false);
        }
    }

    public virtual void Trigger()
    {
        if(equipt == false)return;
    }

    public enum Type
    {
        Weapon,
        HealingItem,
        None
    }
}
