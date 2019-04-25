/************************************************************************************************
 * 文档说明：求解无向图中任意两点之间的所有路径。
 * *********************************************************************************************/
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace ArcgisApp
{
    /// <summary>
    /// 求解无向图中任意两点之间的所有路径
    /// </summary>
    public class MapPath
    {
        #region 私有字段
        private int _vertexNumber = 0;
        #endregion

        #region 辅助变量
        private bool[] visted = null;
        private int[] path = null;  //编号路径
        private List<List<int>> allPath = new List<List<int>>();    //以字符串链表形式保存所有路径，每一个串为一条路径
        private int level = 6; //nLevel-搜索最大深度
        #endregion

        #region 构造函数
        public MapPath()
            : this(0)
        {

        }
        public MapPath(int vexNum)
        {
            this.VertexNumber = vexNum;
            this.visted = new bool[VertexNumber];
            this.path = new int[VertexNumber];
        }

        public MapPath(int vexNum, int nLevel)
        {
            this.VertexNumber = vexNum;
            this.visted = new bool[VertexNumber];
            this.path = new int[VertexNumber];
            this.level = nLevel;
        }
        #endregion

        #region 内部方法，基于广度遍历算法和递归，检索出所有路径
        //参数：graph-图中任意两个顶点间的距离矩阵，值=0-表示无联通路径；值=1-表示有联通路径
        //sourceVertex-起点编号
        //targetVertex-目标前编号
        private void SearchAll(Dictionary<int, Dictionary<int, double>> graph, int sourceVertex, int targetVertex, int step)
        {if (step > level)
                    return;
            if (visted[sourceVertex])
            {
                return;
            }
            path[step - 1] = sourceVertex;
            if (sourceVertex == targetVertex)
            {
                List<int> aPath = new List<int>();
                for (int i = 0; i < step; i++)
                {
                    aPath.Add(path[i]);
                }

                allPath.Add(aPath);
            }
            else
            {
                visted[sourceVertex] = true;
                for (int v = 0; v < VertexNumber; v++)
                {
                    //if (graph[sourceVertex, v] != 0 && !visted[v])
                    //try
                    //{
                    //    if (graph.Contains(sourceVertex)) ;
                    //}
                    //catch (Exception ex)
                    //{ }
                    if (graph.ContainsKey(sourceVertex))
                    {
                        if (graph[sourceVertex].ContainsKey(v))
                        {
                            if (graph[sourceVertex][v] > 0)
                            {
                                if (!visted[v])
                                {
                                    SearchAll(graph, v, targetVertex, step + 1);
                                }
                            }
                        }
                    }
                        
                }
                visted[sourceVertex] = false;
            }
        }

        //获取所有遍历到的可用路径信息，以字符串链表形式保存
        public List<List<int>> GetAllWays(Dictionary<int, Dictionary<int, double>> graph, int sourceVertex, int targetVertex)
        {
            SearchAll(graph, sourceVertex, targetVertex, 1);
            return allPath;
        }
        #endregion

        #region 属性
        public int VertexNumber
        {
            get { return _vertexNumber; }
            set { _vertexNumber = value; }
        }
        #endregion
    }
}