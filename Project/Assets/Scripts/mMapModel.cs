using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;




#region Fight Map
public enum MarkSign
{
    none,
    red,
    green,
    yellow
}

public enum PathFindDir
{
    toUp_toRight,//从下到上 从左到右
    toUp_toLeft,//从下到上 从右到左
    toDown_toRight,//从上到下 从左到右
    toDown_toLeft//从上到下 从右到左
}

[System.Serializable]
public class mNode
{
    public int row;
    public int col;

    public Vector3 position;

    ///进入战斗状态
    ///战斗状态的移动
    ///退出战斗状态
    ///死亡
    ///都会影响到当前格子障碍

    public mNode(int row, int col)
    {
        this.row = row;
        this.col = col;

        //this.isObstacle = false;

        //this.position = mMapModel.Current.GetGridCell_Center_Position(row, col);
    }

    public mNode(Vector3 pos)
    {
        //int index = mMapModel.Current.GetGridIndex(pos);
        //Debug.Log("index" + index);

        //this.row = mMapModel.Current.GetRow(index);
        //Debug.Log("row" + row);

        //this.col = mMapModel.Current.GetColumn(index);
        //Debug.Log("col" + col);

        //this.isObstacle = false;

        this.position = pos;
    }

    public override bool Equals(object obj)
    {
        mNode other = (mNode)obj;
        return this.row == other.row && this.col == other.col;
    }

    public override string ToString()
    {
        return " row : " + row + " col : " + col + " position : " + position;
    }
}

public class mMapModel : MonoBehaviour
{

    #region Controller
    public bool showGrid = true;
    public bool showObstacleBlocks = true;
    #endregion

    #region Mode
    public static mMapModel Current;

    public mNode[,] nodes;

    private PathFindDir pathFindDir = PathFindDir.toDown_toRight;

    int tmpindex;
    int tmpRow;
    int tmpCol;
    mNode tmpNode;

    mNode enemyNode;
    mNode selfNode;
    mNode moveNode;

    List<mNode> sameRowNodes;
    List<mNode> sameColNodes;
    List<mNode> attackNodes;
    List<mNode> attackNodesWithOutObstacle;
    #endregion

    void Awake()
    {
        Current = this;

        //Init();
    }

    //#region 节点索引
    /////从给定位置返回格子中的格子索引
    //public int GetGridIndex(Vector3 pos)
    //{
    //    if (!IsInBounds(pos))
    //    {
    //        Debug.LogError("!IsInBounds");
    //        return -1;
    //    }

    //    pos -= GlobalConfig.MapConfig.ORIGIN;

    //    float bevelHeight = pos.y / Mathf.Sin(GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad);
    //    int row = (int)(bevelHeight / GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT);
    //    //Debug.Log(" row " + row);

    //    float width = bevelHeight * Mathf.Cos(GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad);
    //    int col = (int)((pos.x - width) / GlobalConfig.MapConfig.CELLSIZE_WIDTH);
    //    //Debug.Log(" col " + col);
            
    //    return (row * GlobalConfig.MapConfig.CELL_COLUMN_NUM + col);
    //}

    //public int GetRow(int index)
    //{
    //    int row = index / GlobalConfig.MapConfig.CELL_COLUMN_NUM;
    //    return row;
    //}

    /////GetColumn 从给定索引 返回格子的列数
    //public int GetColumn(int index)
    //{
    //    int col = index % GlobalConfig.MapConfig.CELL_COLUMN_NUM;
    //    return col;
    //}

    //public mNode GetNode(Vector3 pos)
    //{
    //    if (!IsInBounds(pos))
    //    {
    //        Debug.LogError("!IsInBounds");
    //        return null;
    //    }

    //    tmpindex = GetGridIndex(pos);
    //    tmpRow = GetRow(tmpindex);
    //    tmpCol = GetColumn(tmpindex);
    //    return nodes[tmpRow, tmpCol];
    //}

    ///// checked
    //public Vector3 GetGridCell_Center_Position(int index)
    //{
    //    Vector3 curPos = GetGridCell_LeftBottom_Position(index);

    //    //curPos.y += (GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);
    //    //curPos.x += (GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);

    //    float angle = GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad;

    //    curPos.y += (GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT / 2.0f) * Mathf.Sin(angle);

    //    curPos.x += (GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT / 2.0f) * Mathf.Cos(angle);

    //    curPos.x += (GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);

    //    return curPos;
    //}

    //public Vector3 GetGridCell_LeftBottomMiddle(int row, int col)
    //{
    //    Vector3 curPos = GetGridCell_LeftBottom_Position(row, col);

    //    curPos.x += (GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);

    //    return curPos;
    //}

    ///// checked
    //public Vector3 GetGridCell_Center_Position(int row, int col)
    //{
    //    Vector3 curPos = GetGridCell_LeftBottom_Position(row, col);

    //    //curPos.y += (GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);
    //    //curPos.x += (GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);

    //    float angle = GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad;
            
    //    curPos.y += (GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT / 2.0f) * Mathf.Sin(angle);
            
    //    curPos.x += (GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT / 2.0f) * Mathf.Cos(angle);
            
    //    curPos.x += (GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);

    //    return curPos;
    //}

    ///// checked
    //public Vector3 GetGridCell_LeftBottom_Position(int index)
    //{
    //    int row = GetRow(index);

    //    int col = GetColumn(index);

    //    return GetGridCell_LeftBottom_Position(row, col);
    //}

    ///// checked
    //private Vector3 GetGridCell_LeftBottom_Position(int row, int col)
    //{
    //    //float yPosInGrid = row * GlobalConfig.MapConfig.CELLSIZE_WIDTH;

    //    //float xPosInGrid = col * GlobalConfig.MapConfig.CELLSIZE_WIDTH;

    //    //return GlobalConfig.MapConfig.ORIGIN + new Vector3(xPosInGrid, yPosInGrid, 0);

    //    float angle = GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad;

    //    return GlobalConfig.MapConfig.ORIGIN
    //        + col * GlobalConfig.MapConfig.CELLSIZE_WIDTH * Vector3.right
    //            + row * GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT * Mathf.Cos(angle) * Vector3.right
    //                + row * GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT * Mathf.Sin(angle) * Vector3.up;
    //}

    /////是否在面积内
    //public bool IsInBounds(Vector3 pos)
    //{
    //    //float width = GlobalConfig.MapConfig.CELL_COLUMN_NUM * GlobalConfig.MapConfig.CELLSIZE_WIDTH;
    //    //float height = GlobalConfig.MapConfig.CELL_ROW_NUM * GlobalConfig.MapConfig.CELLSIZE_WIDTH;

    //    //return (pos.x >= GlobalConfig.MapConfig.ORIGIN.x && pos.x <= GlobalConfig.MapConfig.ORIGIN.x + width &&
    //    //        pos.y >= GlobalConfig.MapConfig.ORIGIN.y && pos.y <= GlobalConfig.MapConfig.ORIGIN.y + height);

    //    float angle = GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad;
    //    Vector2 up_offset
    //        = Vector2.right * GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT * GlobalConfig.MapConfig.CELL_ROW_NUM * Mathf.Cos(angle)
    //            + Vector2.up * GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT * GlobalConfig.MapConfig.CELL_ROW_NUM * Mathf.Sin(angle);

    //    ///得到四个点
    //    Vector2 origin_LeftBottom
    //        = GlobalConfig.MapConfig.ORIGIN;
    //    Vector2 origin_LeftUp
    //        = origin_LeftBottom + up_offset;
    //    Vector2 origin_RightBottom
    //        = origin_LeftBottom
    //            + Vector2.right * GlobalConfig.MapConfig.CELLSIZE_WIDTH * GlobalConfig.MapConfig.CELL_COLUMN_NUM;
    //    Vector2 origin_RightUp
    //        = origin_LeftBottom
    //            + Vector2.right * GlobalConfig.MapConfig.CELLSIZE_WIDTH * GlobalConfig.MapConfig.CELL_COLUMN_NUM
    //                + up_offset;

    //    //Debug.Log("isLeft " + Util.isLeft(origin_LeftBottom, origin_LeftUp, pos));
    //    //Debug.Log("isLeft " + Util.isLeft(origin_RightBottom, origin_RightUp, pos));
    //    //Debug.Log("isAbove " + Util.isAbove(origin_LeftBottom, origin_RightBottom, pos));
    //    //Debug.Log("isAbove " + Util.isAbove(origin_LeftUp, origin_RightUp, pos));

    //    ///是否在左边的右边 && 右边的左边 && 下边的上边 && 上边的下边 　
    //    return !Util.isLeft(origin_LeftBottom, origin_LeftUp, pos)
    //                && Util.isLeft(origin_RightBottom, origin_RightUp, pos)
    //                    && Util.isAbove(origin_LeftBottom, origin_RightBottom, pos)
    //                        && !Util.isAbove(origin_LeftUp, origin_RightUp, pos);
    //}

    //public bool IsInBounds(int row, int col)
    //{
    //    return row >= 0 && col >= 0 && row < GlobalConfig.MapConfig.CELL_ROW_NUM && col < GlobalConfig.MapConfig.CELL_COLUMN_NUM;
    //}



    /////得到从出发点 到 结束点 一列
    //public List<mNode> GetSameColNodes_FromStartNode(int startRow, int startCol, bool toDown, int radius, bool includeSelf)
    //{
    //    sameColNodes = new List<mNode>();

    //    int i = includeSelf ? 0 : 1;

    //    while (i <= radius)
    //    {
    //        tmpRow = startRow + (toDown ? (-1 * i) : (1 * i));
    //        tmpCol = startCol;

    //        if (IsInBounds(tmpRow, tmpCol))
    //        {
    //            sameColNodes.Add(nodes[tmpRow, tmpCol]);
    //        }

    //        i++;
    //    }

    //    return sameColNodes;
    //}
    //#endregion

    //#region 外部 接口

    //public void Init()
    //{
    //    nodes = new mNode[GlobalConfig.MapConfig.CELL_ROW_NUM, GlobalConfig.MapConfig.CELL_COLUMN_NUM];

    //    for (int i = 0; i < GlobalConfig.MapConfig.CELL_ROW_NUM; i++)
    //    {
    //        for (int j = 0; j < GlobalConfig.MapConfig.CELL_COLUMN_NUM; j++)
    //        {
    //            nodes[i, j] = new mNode(i, j);
    //        }
    //    }
    //}

    ///// 得到Grid中心mNode
    //public mNode GetNode_FromGird(int girdRow, int gridCol)
    //{
    //    ///GridRow,GridCol => nodeRow nodeCol
    //    //tmpRow = girdRow * 5 + 2;
    //    tmpRow = girdRow * GlobalConfig.MapConfig.GRID_HEIGHT + (GlobalConfig.MapConfig.GRID_HEIGHT / 2);
    //    tmpCol = gridCol * GlobalConfig.MapConfig.GRID_WIDTH + (GlobalConfig.MapConfig.GRID_WIDTH / 2);

    //    Debug.Log(" girdRow " + girdRow + " gridCol " + gridCol);

    //    return nodes[tmpRow, tmpCol];
    //}

    //private float TotalCost(mNode self, mNode tmpSelect, mNode goal)
    //{
    //    float a_totalCost = HeuristicEstimateCost(selfNode, tmpSelect);

    //    float a_neighbourNodeEstCost = HeuristicEstimateCost(tmpSelect, goal);

    //    float a_estimatedCost = a_totalCost + a_neighbourNodeEstCost;

    //    return a_estimatedCost;
    //}

    //private float HeuristicEstimateCost(mNode curNode, mNode goalNode)
    //{
    //    //Vector3 vecCost = curNode.position - goalNode.position;
    //    //return vecCost.magnitude;

    //    return Vector3.Distance(goalNode.position, curNode.position);
    //}
    //#endregion

    #region Draw Map
    //Vector3 obCellSize = new Vector3(GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f, GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f, 0.0f);
    //void OnDrawGizmos()
    //{


    //    if (showGrid)
    //    {
    //        DebugDrawCell(
    //            GlobalConfig.MapConfig.ORIGIN,
    //                GlobalConfig.MapConfig.CELL_ROW_NUM,
    //                    GlobalConfig.MapConfig.CELL_COLUMN_NUM,
    //                        GlobalConfig.MapConfig.CELLSIZE_WIDTH,
    //                            GlobalConfig.MapConfig.CELLSIZE_BEVEL_HEIGHT,
    //                                GlobalConfig.MapConfig.ANGLE);
    //    }

    //    if (showObstacleBlocks && Application.isPlaying)
    //    {
    //        int bound0 = nodes.GetUpperBound(0);
    //        int bound1 = nodes.GetUpperBound(1);

    //        for (int i = 0; i <= bound0; i++)
    //        {
    //            for (int x = 0; x <= bound1; x++)
    //            {
    //                if (nodes[i, x].owners.Count > 0)
    //                {
    //                    Gizmos.DrawCube(nodes[i, x].position, obCellSize);
    //                }

    //                #region testMark
    //                switch (nodes[i, x].testMark)
    //                {
    //                    case MarkSign.none:
    //                        break;
    //                    case MarkSign.red:
    //                        Gizmos.color = Color.red;
    //                        Gizmos.DrawWireSphere(nodes[i, x].position, GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);
    //                        break;
    //                    case MarkSign.green:
    //                        Gizmos.color = Color.green;
    //                        Gizmos.DrawWireSphere(nodes[i, x].position, GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);
    //                        break;
    //                    case MarkSign.yellow:
    //                        Gizmos.color = Color.yellow;
    //                        Gizmos.DrawWireSphere(nodes[i, x].position, GlobalConfig.MapConfig.CELLSIZE_WIDTH / 2.0f);
    //                        break;
    //                    default:
    //                        break;
    //                }
    //                #endregion

    //            }
    //        }
    //    }
    //}

    //Color curColor;
    //public void DebugDrawCell(
    //    Vector3 origin,
    //        int numRows, int numCols,
    //            float cell_width, float cell_bevelheight,
    //                float angle)
    //{
    //    float width = (numCols * cell_width);
    //    float bevel_height = (numRows * cell_bevelheight);

    //    // Draw the horizontal grid lines 
    //    for (int i = 0; i < numRows + 1; i++)
    //    {
    //        Vector3 startPos 
    //            = origin
    //                + Vector3.up * i * cell_bevelheight * Mathf.Sin(GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad)
    //                + Vector3.right * i * cell_bevelheight * Mathf.Cos(GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad);

    //        curColor = (i % GlobalConfig.MapConfig.GRID_HEIGHT) == 0 ? Color.red : Color.blue;

    //        Vector3 endPos = startPos + width * Vector3.right;

    //        Debug.DrawLine(startPos, endPos, curColor);
    //    }

    //    // Draw the vertial grid lines
    //    for (int i = 0; i < numCols + 1; i++)
    //    {
    //        Vector3 startPos = origin + i * cell_width * Vector3.right;
    //        Vector3 endPos 
    //            = startPos
    //                + Vector3.up * bevel_height * Mathf.Sin(GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad)
    //                + Vector3.right * bevel_height * Mathf.Cos(GlobalConfig.MapConfig.ANGLE * Mathf.Deg2Rad);

    //        curColor = (i % GlobalConfig.MapConfig.GRID_WIDTH) == 0 ? Color.red : Color.blue;

    //        Debug.DrawLine(startPos, endPos, curColor);
    //    }
    //}
    #endregion
}
#endregion


