using UnityEngine;
using System.Collections;

namespace PotentialField
{
    public enum PF_TYPE
    {
        /// <summary>
        /// -1
        /// </summary>
        Attract = -1,
        /// <summary>
        /// 1
        /// </summary>
        Repell = 1
    }

     public class GlobalConfig
     {
         /// 战斗地图信息
         public static class MapConfig
         {
             public static Vector3 ORIGIN = Vector3.zero;


             //public static f

             ///格子跟体型相关
             ///体型跟射程相关
             /// 平行四边形底
             //public static float CELLSIZE_WIDTH = 1.7f;

             ///// 平行四边形左下角 角度
             ///// => 必须保证和军队内部初始地图小格子角度一致
             //public static float ANGLE = 120.0f;

             ///// 平行四边形斜边长
             //public static float CELLSIZE_BEVEL_HEIGHT = 1.7f;



             ///// 地图中间 大格 EMPTY 列数
             //public static int GRID_EMPTY_COLUMN_NUM = 2;

             ///// 地图 大格 SIDE 列数
             //public static int GRID_SIDE_COLUMN_NUM = 4;

             ///// 地图 大格 总列数
             //public static int GRID_COLUMN_NUM = (GRID_SIDE_COLUMN_NUM * 2 + GRID_EMPTY_COLUMN_NUM);

             ///// 地图 大格 总行数
             //public static int GRID_ROW_NUM = 3;

             /////每一大格子的宽 （= 多少个小格）
             /////必须 是 奇数
             //public static int GRID_WIDTH = 9;

             /////每一大格子的高 （= 多少个小格）
             /////必须 是 奇数
             //public static int GRID_HEIGHT = 9;

             ///// 中间 EMPTY 小格子列数
             //public static int EMPTY_COLUMN_NUM = GRID_EMPTY_COLUMN_NUM * GRID_WIDTH;

             ///// 中间 SIDE 小格子列数
             //public static int SIDE_COLUMN_NUM = GRID_SIDE_COLUMN_NUM * GRID_WIDTH;

             ///// 小格子总列数
             //public static int CELL_COLUMN_NUM = GRID_COLUMN_NUM * GRID_WIDTH;

             ///// 小格子总行数
             //public static int CELL_ROW_NUM = GRID_ROW_NUM * GRID_HEIGHT;
         }
     }
}
