using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    public GameObject[] cubes;
    public GameObject[] door;
    public GameObject[] trueOrderCubes;

    public void Start()
    {
        trueOrderCubes = new GameObject[cubes.Length+1];
        trueOrderCubes[0] = cubes[3];
        trueOrderCubes[1] = cubes[2];
        trueOrderCubes[2] = cubes[1];
        trueOrderCubes[3] = cubes[0];
    }

    private void IsCubesTrueOrder()
    {
        //if boxes have true order, door[0]'s z index will be -3 and door[1]'s z index will be +3
        if (cubes[0].transform.position == trueOrderCubes[0].transform.position &&
                       cubes[1].transform.position == trueOrderCubes[1].transform.position &&
                                  cubes[2].transform.position == trueOrderCubes[2].transform.position &&
                                             cubes[3].transform.position == trueOrderCubes[3].transform.position)
        {
            Debug.Log("true order");
            door[0].transform.position = new Vector3(door[0].transform.position.x, door[0].transform.position.y, door[0].transform.position.z - 3);
            door[1].transform.position = new Vector3(door[1].transform.position.x, door[1].transform.position.y, door[0].transform.position.z + 3);
        }
    }

    //there are 4 cubes and 3 capsules. Capsules are in the middle of the cubes. If capsules[i] is pressed "E", cubes[i] and cubes[i+1] will be swapped
    public void OnButtonPressed(int index)
    {
        Debug.Log("xx " + index + " pressed");
        if (index == 0)
        {
            Debug.Log("aaa " + index + " pressed");
            //if capsules[0] is pressed "E", cubes[0] and cubes[1] will be swapped
            Vector3 temp = cubes[0].transform.position;
            cubes[0].transform.position = cubes[1].transform.position;
            cubes[1].transform.position = temp;
            GameObject temp2 = cubes[0];
            cubes[0] = cubes[1];
            cubes[1] = temp2;
        }
        else if (index == 1)
        {
            //if capsules[1] is pressed "E", cubes[1] and cubes[2] will be swapped
            Vector3 temp = cubes[1].transform.position;
            cubes[1].transform.position = cubes[2].transform.position;
            cubes[2].transform.position = temp;
            GameObject temp2 = cubes[1];
            cubes[1] = cubes[2];
            cubes[2] = temp2;
        }
        else if (index == 2)
        {
            //if capsules[2] is pressed "E", cubes[2] and cubes[3] will be swapped
            Vector3 temp = cubes[2].transform.position;
            cubes[2].transform.position = cubes[3].transform.position;
            cubes[3].transform.position = temp;
            GameObject temp2 = cubes[2];
            cubes[2] = cubes[3];
            cubes[3] = temp2;
        }
        
        IsCubesTrueOrder();
    }

    //how to understand which phere is pressed "E"? 
    //1. Create a sphere and add a script to it.
    //2. Add a tag to the sphere.
    //3. Add a collider to the sphere.
    //4. Add a rigidbody to the sphere.
    //5. Add a script to the sphere.
    // what script do I add to the sphere?
    // public class ButtonOnClick : MonoBehaviour
    // {
    //     //If sphere pressed "E", call boxMovement.OnButtonPressed(i)

}
