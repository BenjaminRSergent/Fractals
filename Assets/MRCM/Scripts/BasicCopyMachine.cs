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

    private Vector3 _lowerLeft;
    
    void Start()
    {
        Destroy(this);
        if (currentDepth >= maxDepth)
        {
            return;
        }
        if (currentDepth == 0)
        {
            // We're the first copy. Set the lower left reference point for all future copies.
            _lowerLeft = transform.position - volume/2.0f;
        }

        // The parents for all the copies we're about to make
        var newParent = new GameObject();
        newParent.transform.position = transform.position;
        newParent.transform.rotation = transform.rotation;

        // Copy scale and translate the current object based on the machine lens
        foreach (Lens currLen  in lens)
        {
            var newObj = Instantiate(gameObject);
            newObj.transform.localScale = newObj.transform.localScale * currLen.scale;
            newObj.transform.position = _lowerLeft + new Vector3(volume.x * currLen.offset.x, volume.y * currLen.offset.y, volume.z * currLen.offset.z);
            newObj.transform.parent = newParent.transform;
        }

        // Have the parent continue copying recursively
        var parentScript = newParent.AddComponent<BasicCopyMachine>();
        parentScript.lens = lens;
        parentScript.currentDepth = currentDepth+1;
        parentScript.maxDepth = maxDepth;
        parentScript.volume = volume;
        parentScript._lowerLeft = _lowerLeft;
        Destroy(this.gameObject);
    }
}
