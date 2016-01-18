using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utilities;

public class SliceViewer : MonoBehaviour {
    private const float SLICE_NUM = 4;

    // A list of slices that we can toggle on and off.
    private readonly List<Slice> slices = new List<Slice>();

    // This is our position on the looping manifold. (-inf, inf)
    private float manifold_translation = 0;

    private Vector3 previous_forward;

    private struct Slice {
        public GameObject g_object;
        public float start_angle;
        public float end_angle;

        public float Midpoint() {
            return (start_angle + end_angle) / 2.0f;
        }
    }

	protected void Start () {
	    previous_forward = transform.forward;

        // Add all of the slices
	    for (int i = 0; i < SLICE_NUM; i++) {
            slices.Add(new Slice {
                g_object = GameQuery.FindOrThrow("Half_"+i, find_non_active:true),
                start_angle = i * 180.0f,
                end_angle = (i+1) * 180.0f,
            });
	    }
	}
	
	protected void Update () {
	    float rotation_change = Math.AngleSigned(previous_forward, transform.forward, Vector3.up);
	    previous_forward = transform.forward;

        // Move us along the manifold
	    manifold_translation += rotation_change;

        // Show any slice with a midpoint that is within 180 degrees of our 
        // look direction / position in the manifold.
	    foreach (Slice s in slices) {
            bool active = Mathf.Abs(s.Midpoint() - manifold_translation) < 180.0f; 
            s.g_object.SetActive(active);
	    }
	}
}
