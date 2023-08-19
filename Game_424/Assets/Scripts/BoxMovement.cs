using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    public GameObject[] cubes;
    public GameObject[] door;
    public GameObject[] trueOrderCubes;

    private void IsCubesTrueOrder()
    {
        if (cubes[0].transform.position == trueOrderCubes[0].transform.position &&
                       cubes[1].transform.position == trueOrderCubes[1].transform.position &&
                                  cubes[2].transform.position == trueOrderCubes[2].transform.position &&
                                             cubes[3].transform.position == trueOrderCubes[3].transform.position)
        {
            Debug.Log("true order");
            //delete door[0] and door[1]
            Destroy(door[0]);
            Destroy(door[1]);
        }
    }

    public void OnButtonPressed(int index)
    {
        if (index == 0)
        {
            Vector3 temp = cubes[0].transform.position;
            cubes[0].transform.position = cubes[1].transform.position;
            cubes[1].transform.position = temp;
            GameObject temp2 = cubes[0];
            cubes[0] = cubes[1];
            cubes[1] = temp2;
        }
        else if (index == 1)
        {
            Vector3 temp = cubes[1].transform.position;
            cubes[1].transform.position = cubes[2].transform.position;
            cubes[2].transform.position = temp;
            GameObject temp2 = cubes[1];
            cubes[1] = cubes[2];
            cubes[2] = temp2;
        }
        else if (index == 2)
        {
            Vector3 temp = cubes[2].transform.position;
            cubes[2].transform.position = cubes[3].transform.position;
            cubes[3].transform.position = temp;
            GameObject temp2 = cubes[2];
            cubes[2] = cubes[3];
            cubes[3] = temp2;
        }
        
        IsCubesTrueOrder();
    }

}
