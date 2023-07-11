using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;

public class DraggableItem : MonoBehaviour
{
    public Vector3 tabScale;
    public Vector3 placedScale;
    private Vector3 originalPosition;
    private List<ItemHolder> collidedHolders = new List<ItemHolder>();
    
    // Start is called before the first frame update
    void Start()
    {
        tabScale = transform.localScale;
        originalPosition = transform.position;
    }

    private void OnEnable()
    {
        // GetComponent<PressGesture>().Pressed += PressedHandler;
        GetComponent<TransformGesture>().TransformStarted += TransformStartedHandler;
        GetComponent<TransformGesture>().TransformCompleted += TransformCompletedHandler;
    }

    private void OnDisable()
    {
        // GetComponent<PressGesture>().Pressed -= PressedHandler;
        GetComponent<TransformGesture>().TransformStarted -= TransformStartedHandler;
        GetComponent<TransformGesture>().TransformCompleted -= TransformCompletedHandler;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        ItemHolder holder = other.GetComponent<ItemHolder>();
        if (holder != null && holder.CanHoldItem())
        {
            collidedHolders.Add(holder);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ItemHolder holder = other.GetComponent<ItemHolder>();
        if (holder != null)
        {
            collidedHolders.Remove(holder);
        }
    }

    private void TransformStartedHandler(object sender, EventArgs e)
    {
        transform.localScale = placedScale;
    }
    
    private void TransformCompletedHandler(object sender, EventArgs e)
    {
        if (collidedHolders.Count > 0)
        {
            // Find the closest holder
            ItemHolder closestHolder = null;
            float minDistance = float.MaxValue;
            foreach (ItemHolder holder in collidedHolders)
            {
                float distance = Vector3.Distance(transform.position, holder.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestHolder = holder;
                }
            }

            // Add current item to the ItemHolder
            closestHolder.currentItems.Add(this);
            // Parent the item to the holder
            transform.parent = closestHolder.transform;
            // Increase placed count by 1
            GameManager.Instance.BlockPlaced();
            // Disable the `Transformer` and `TransformGesture` component so it can't be moved again
            Transformer transformer = GetComponent<Transformer>();
            TransformGesture transformGesture = GetComponent<TransformGesture>();
            if (transformer != null)
            {
                transformer.enabled = false;
            }
            if (transformGesture != null)
            {
                transformGesture.enabled = false;
            }
        }
        else
        {
            // No valid holder found, reset to original position and scale
            transform.position = originalPosition;
            transform.localScale = tabScale;
        }

        // Clear the list of collided holders
        collidedHolders.Clear();
    }
}
