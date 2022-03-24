using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public DNA dna;
    /* vars */
    Quaternion targetRotation;
    LineRenderer line;
    //bools
    bool isInit = false; //false by default, changed up being initialised
    public bool isFinished = false; //false by default, changed up being finished
    //v2
    Vector2 target;
    Vector2 nextPoint;
    List<Vector2> currentPath = new List<Vector2>();
    //float
    public float agentSpeed;
    public float vectorMult;
    public float rotationSpeed;
    //int
    int pathIndex = 0;
    public void initAgent(DNA newDNA, Vector2 EndTarget) {
        currentPath.Add(transform.position);
        isInit = true;
        dna = newDNA;
        target = EndTarget;
        nextPoint = transform.position;
        currentPath.Add(nextPoint);
        line = GetComponent<LineRenderer>();
    }


    private void Update()
    {
        /* if the agent has been initialised */
        if (isInit && !isFinished) {
            /* || the agent is < 0.5f away from target(end point)*/
            if (pathIndex == dna.genes.Count || Vector2.Distance(transform.position,target)<0.5f) { isFinished = true; }
            if ((Vector2)transform.position == nextPoint)
            {
                nextPoint = (Vector2)transform.position + dna.genes[pathIndex];
                currentPath.Add(nextPoint);
                targetRotation = LookAt2D(nextPoint);
                pathIndex++;
                
            }
            else {
                transform.position = Vector2.MoveTowards(transform.position, nextPoint, (agentSpeed * Time.deltaTime));
            }
            if (transform.rotation != targetRotation) {
               transform.rotation= Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        RenderLine();
    }

    public void RenderLine() {
        List<Vector3> linePoints = new List<Vector3>();
        /* needs to exceed certain length */
        if (currentPath.Count > 1)
        {
            
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                linePoints.Add(currentPath[i]);
            }
            linePoints.Add(transform.position);
            
        }
        else {
            linePoints.Add(currentPath[0]);
            linePoints.Add(transform.position);
        }
        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
        

    }

    /* returns fitness value of current agent */
    public float fitness
    {
        get
        {
            /* the closer the agent is to the target destination, the higher the fitness value */
            float distance = Vector2.Distance(transform.position, target);
            /* set the distance if the final distance of the agent is 0, to avoid a division error */
            if (distance == 0)
            {
                distance = 0.0001f;
            }
            return (60 / distance);
        }
    }
    /* to rotate the agent towards its target destination */
    public Quaternion LookAt2D(Vector2 target, float angleOffset = 90)
    {
        Vector2 fromTo = (target - (Vector2)transform.position).normalized;
        float zRot = Mathf.Atan2(fromTo.y, fromTo.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, zRot + angleOffset);
    }
void End() { isFinished = true; }
       
}
