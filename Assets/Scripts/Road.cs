using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Road should be horizontal
public class Road : MonoBehaviour
{
    [SerializeField] private GameObject[] edgeObjects;

    private RoadJoint[] connectedJoints = new RoadJoint[2];

    private void Start()
    {
        OptimizeArrangement();
    }

    private void OptimizeArrangement()
    {
        GetConnectedJoints();

        Vector3 originalPath = edgeObjects[0].transform.position - edgeObjects[1].transform.position;
        Vector3 optimizedPath = connectedJoints[0].transform.position - connectedJoints[1].transform.position;

        //move
        transform.position = (connectedJoints[0].transform.position + connectedJoints[1].transform.position) / 2;

        //extend
        float extensionCoef = optimizedPath.magnitude / originalPath.magnitude;
        transform.localScale = new Vector3(extensionCoef * transform.localScale.x, transform.localScale.y, transform.localScale.z);

        //rotate
        Quaternion rotation = Quaternion.FromToRotation(originalPath, optimizedPath);
        transform.rotation *= rotation;
    }

    private void GetConnectedJoints()
    {
        for(int cnt = 0; cnt < 2; cnt++)
        {
            Vector3 edgePosition = edgeObjects[cnt].transform.position;

            connectedJoints[cnt] = GetNearestJoint(edgePosition);
        }
    }

    private RoadJoint GetNearestJoint(Vector3 searchingPoint)
    {
        RoadJoint[] allJoints = FindObjectsOfType<RoadJoint>();

        RoadJoint nearestJoint = null;

        float minDistance = float.MaxValue;
        foreach(RoadJoint joint in allJoints)
        {
            Vector3 difference = searchingPoint - joint.transform.position;
            float distance = difference.magnitude;

            if (distance < minDistance)
            {
                nearestJoint = joint;
                minDistance = distance;
            }
        }

        Debug.Assert(nearestJoint != null);

        return nearestJoint;
    }
}
