using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulGames.EasyGridBuilderPro;

namespace SoulGames.Utilities
{
    public class SpawnEffects : MonoBehaviour
    {
        [Space]
        [Tooltip("Enable Use Scale Effect")]
        [SerializeField]public bool useScalingEffect;

        [SerializeField]public bool showObjectScallingData = false;

        [Space]
        [Tooltip("Enable useScaleEffectX. \n(Which scale object in it's local X axis, when it's buid and ghost object selected.)")]
        [SerializeField]private bool useScaleEffectX;
        [Tooltip("Enable useScaleEffectY. \n(Which scale object in it's local Y axis, when it's buid and ghost object selected.)")]
        [SerializeField]private bool useScaleEffectY;
        [Tooltip("Enable useScaleEffectZ. \n(Which scale object in it's local Z axis, when it's buid and ghost object selected.)")]
        [SerializeField]private bool useScaleEffectZ;
        [Tooltip("Curve Time represent scale duration. \nCurve Value represent scale size in Y axis.")]
        [SerializeField]private AnimationCurve ScaleEffectAnimationCurve = null;
        [Space]

        [SerializeField]public bool showObjectSpawnData = false;

        [Space]
        [Tooltip("Enable Use Spawn Game Objects")]
        [SerializeField]public bool useSpawnGameObjects;
        [Tooltip("List of game objects that needs to be spawned")]
        [SerializeField]private List<GameObject> spawnGameObjects;
        [Tooltip("If this is enabled only one of game object picked randomly and will be spawned")]
        [SerializeField]private bool spawnRandomOneFromList;
        [Tooltip("Provide a delay wich game objects will be spawned")]
        [SerializeField]private float spawnStartDelay = 0f;
        [Tooltip("Offset local position which game objects will be spawned")]
        [SerializeField]private Vector3 spawnLocalPosition = Vector3.zero;
        [Tooltip("Override local rotation of spawning game object")]
        [SerializeField]private Vector3 spawnLocalRotation = Vector3.zero;
        [Tooltip("Override local scale of spawning game object")]
        [SerializeField]private Vector3 spawnLocalScale = Vector3.one;
        [Tooltip("If enabled spawnned objects will be destroyed after a certain amount of time")]
        [SerializeField]public bool destroySpawnnedAfterDelay;
        [Tooltip("Time delay before the spawnned objects are destroyed")]
        [SerializeField]private float destroyDelay = 1f;

        private BuildableGridObject buildableGridObject;
        private BuildableFreeObject buildableFreeObject;

        private float time;

        private void Start()
        {
            if (GetComponent<BuildableGridObject>()) buildableGridObject = GetComponent<BuildableGridObject>();
            if (GetComponent<BuildableFreeObject>()) buildableFreeObject = GetComponent<BuildableFreeObject>();
            if (useSpawnGameObjects) Invoke("CallSpawnObjects", spawnStartDelay);
        }

        private void Update()
        {
            if (useScalingEffect)
            {
                if (useScaleEffectX) ScaleEffectX();
                if (useScaleEffectY) ScaleEffectY();
                if (useScaleEffectZ) ScaleEffectZ();
            }
        }

        private void ScaleEffectX()
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(ScaleEffectAnimationCurve.Evaluate(time), 1, 1);
        }

        private void ScaleEffectY()
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(1, ScaleEffectAnimationCurve.Evaluate(time), 1);
        }

        private void ScaleEffectZ()
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(1, 1, ScaleEffectAnimationCurve.Evaluate(time));
        }
        
        private void CallSpawnObjects()
        {
            if (buildableGridObject)
            {
                if (buildableGridObject.GetIsObjectBuilt()) SpawnObjects();
            }
            else if (buildableFreeObject)
            {
                if (buildableFreeObject.GetIsObjectBuilt()) SpawnObjects();
            }
        }

        public void SpawnObjects()
        {
            if (spawnRandomOneFromList)
            {
                GameObject randomObject = spawnGameObjects[UnityEngine.Random.Range(0, spawnGameObjects.Count)];
                GameObject[] spawnnedObject = new GameObject[1];
                spawnnedObject[0] = Instantiate(randomObject, transform.localPosition + spawnLocalPosition, Quaternion.identity);
                spawnnedObject[0].transform.eulerAngles = spawnLocalRotation;
                spawnnedObject[0].transform.localScale = spawnLocalScale;
                if (destroySpawnnedAfterDelay)
                {
                    StartCoroutine(DestroySpawnnedObjects(spawnnedObject));
                }
            }
            else
            {
                GameObject[] spawnnedObject = new GameObject[spawnGameObjects.Count];
                for (int i = 0; i < spawnGameObjects.Count; i++)
                {
                    spawnnedObject[i] = Instantiate(spawnGameObjects[i], transform.localPosition + spawnLocalPosition, Quaternion.identity);
                    spawnnedObject[i].transform.eulerAngles = spawnLocalRotation;
                    spawnnedObject[i].transform.localScale = spawnLocalScale;
                }
                if (destroySpawnnedAfterDelay)
                {
                    StartCoroutine(DestroySpawnnedObjects(spawnnedObject));
                }
            }
        }

        IEnumerator DestroySpawnnedObjects(GameObject[] spawnnedObject)
        {
            yield return new WaitForSeconds(destroyDelay);
            foreach (GameObject item in spawnnedObject)
            {
                Destroy(item);
            }
        }
    }
}