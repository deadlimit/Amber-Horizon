using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ObjectRelocator : MonoBehaviour {

    [Serializable]
    public class RelocatorContext {
        public string Description;
        public Vector3 WorldPosition;
        public Transform TransformPosition;
        public bool UseGameObjectAsPosition;
    }

    public List<RelocatorContext> Positions = new List<RelocatorContext>();


    public void MoveToPosition(Vector3 position) {
        transform.position = position;
    }
}
