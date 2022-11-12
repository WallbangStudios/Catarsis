using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SlopeController : MonoBehaviour {

    public bool displayGizmos;

    public LayerMask Layer;

    public DonovanController player;

    public Corners corners;

    BoxCollider2D coll;

    public float circleradius;

    public enum Corners { topLeft, topRight, btmLeft, btmRight }

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {

        Vector3 edge = CalculateSlopeEdge();

        RaycastHit2D hit = Physics2D.CircleCast(edge, circleradius, Vector2.zero, 0, Layer, -10f);
        if (hit) {
            bool OnEdge = hit;
            player = hit.transform.gameObject.GetComponent<DonovanController>();
            player.OnEdge = OnEdge;
        }
        if (!hit && player != null) {
            if (player.OnEdge) {
                player.OnEdge = false;
            }
            player = null;
        }
        
    }

    Vector2 CalculateSlopeEdge() {
        float top = coll.offset.y + (coll.size.y / 2f);
        float btm = coll.offset.y - (coll.size.y / 2f);
        float left = coll.offset.x - (coll.size.x / 2f);
        float right = coll.offset.x + (coll.size.x / 2f);

        Vector2 corner = Vector2.zero;

        if (corners == Corners.btmLeft)
            corner = transform.TransformPoint(new Vector3(left, btm, 0f));
        if(corners == Corners.btmRight)
            corner = transform.TransformPoint(new Vector3(right, btm, 0f));
        if (corners == Corners.topLeft)
            corner = transform.TransformPoint(new Vector3(left, top, 0f));
        if (corners == Corners.topRight)
            corner = transform.TransformPoint(new Vector3(right, top, 0f));

        return corner;
    }

    private void OnDrawGizmos()
    {
        if (!displayGizmos)
            return;
        coll = GetComponent<BoxCollider2D>();
        Vector3 dge = CalculateSlopeEdge();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(dge, circleradius);
    }
}
