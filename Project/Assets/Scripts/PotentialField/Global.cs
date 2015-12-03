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
    public static class MapConfig
    {
        public static Vector3 origin = Vector2.zero;
        public static int width = 150;
        public static int height = 43;
        public static int diaonal = (int)Mathf.Sqrt(150 * 150 + 43 * 43);

        public static bool isInBound(Vector2 curPos)
        {
            return
                curPos.x <= (origin.x + width)
                    && curPos.x >= (origin.x)
                        && curPos.y <= (origin.y + height)
                            && curPos.y >= (origin.y);
        }
    }
}
