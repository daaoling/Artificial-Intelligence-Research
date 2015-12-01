using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class mMap : MonoBehaviour
{

    #region model
    #region legacy
    //public  Vector3 origin = Vector3.zero;
    //public  float width  = 153;
    //public  float height = 50;
    //public  float angle = 120;
    #endregion
    public Transform left_bottom;
    public Transform left_up;
    public Transform right_bottom;
    public Transform right_up;
    #endregion

    public static mMap current;

    void Awake()
    {
        current = this;
    }
    void Start()
    {
        //Init();
    }
    #region util
    public void Init()
    {

        //List<Vector2> points = new List<Vector2>();
        //for (int i = 0; i <= 1; i++)
        //{
        //    for (int j = 0; j <= 1; j++)
        //    {
        //        Vector2 curPoint
        //            = origin
        //                + j * Vector3.right * width
        //                    //+ j * Vector3.left * Mathf.Cos(angle * Mathf.Deg2Rad) * height
        //                             + i * Vector3.up * Mathf.Sin(angle * Mathf.Deg2Rad) * height;
                                
        //        points.Add(curPoint);
        //    }
        //}

        //PolygonCollider2D curPolygon = gameObject.AddComponent<PolygonCollider2D>();
        //curPolygon.points = points.ToArray();
    }
    #endregion

    public bool IsInBounds(Vector2 pos)
    {
        Debug.Log("isLeft " + isLeft(left_bottom.transform.position, left_up.transform.position, pos));
        Debug.Log("isLeft " + isLeft(right_bottom.transform.position, right_up.transform.position, pos));
        Debug.Log("isAbove " + isAbove(left_bottom.transform.position, right_bottom.transform.position, pos));
        Debug.Log("isAbove " + isAbove(left_up.transform.position, right_up.transform.position, pos));

        ///是否在左边的右边 && 右边的左边 && 下边的上边 && 上边的下边 　
        return !isLeft(left_bottom.transform.position, left_up.transform.position, pos)
                    && isLeft(right_bottom.transform.position, right_up.transform.position, pos)
                        && isAbove(left_bottom.transform.position, right_bottom.transform.position, pos)
                            && !isAbove(left_up.transform.position, right_up.transform.position, pos);
    }


    public static bool isLeft(Vector2 start, Vector2 end, Vector2 c)
    {
        return ((end.x - start.x) * (c.y - start.y) - (end.y - start.y) * (c.x - start.x)) > 0;
    }

    public static bool isAbove(Vector2 start, Vector2 end, Vector2 c)
    {
        return ((end.x - start.x) * (c.y - start.y) - (end.y - start.y) * (c.x - start.x)) > 0;
    }
}
