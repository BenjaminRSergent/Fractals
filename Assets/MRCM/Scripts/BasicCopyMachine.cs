using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCopyMachine : MonoBehaviour {
    [System.Serializable]
    public struct Lens
    {
        public float scale;
        public Vector3 offset;
    }
    
    public Lens[] lens;
    public int maxDepth;
    public int currentDepth;
    public Vector3 volume;

    private Vector3 lowerLeft;

    // Use this for initialization
    void Start()
    {
        Destroy(this);
        if (currentDepth >= maxDepth)
        {
            return;
        }
        if (maxDepth == 0)
        {
            lowerLeft = transform.position - volume/2;
        }


        var newParent = new GameObject();
        newParent.transform.position = transform.position;
        newParent.transform.rotation = transform.rotation;

        foreach (Lens currLen  in lens)
        {
            var newObj = Instantiate(gameObject);
            newObj.transform.localScale = newObj.transform.localScale * currLen.scale;
            newObj.transform.position = lowerLeft + new Vector3(volume.x * currLen.offset.x, volume.y * currLen.offset.y, volume.z * currLen.offset.z);
            newObj.transform.parent = newParent.transform;
        }

        var parentScript = newParent.AddComponent<BasicCopyMachine>();
        parentScript.lens = lens;
        parentScript.currentDepth = currentDepth+1;
        parentScript.maxDepth = maxDepth;
        parentScript.volume = volume;
        parentScript.lowerLeft = lowerLeft;
        Destroy(this.gameObject);
    }
}
