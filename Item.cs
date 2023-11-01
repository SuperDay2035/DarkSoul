using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SG
{

    public class Item : ScriptableObject
    {

        [Header("Item Information")]
        // Item uchun Icon E'lon Qilamiz
        public Sprite itemIcon;
        // Item Uchun Nom E'lon Qilamiz
        public string itemName;

    }
}