using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemHolder : MonoBehaviour
{
    public int itemCapacity = 1;  // The maximum number of items this block can hold
    public List<DraggableItem> currentItems = new List<DraggableItem>();  // The items currently on this block

    // Return true if this block can hold another item
    public bool CanHoldItem()
    {
        return currentItems.Count < itemCapacity;
    }
    
}
