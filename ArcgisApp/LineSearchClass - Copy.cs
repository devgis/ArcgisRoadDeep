using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace ArcgisApp
{
    public class LineSearchClass
    {
        //功能：分析两个变电站之间的所有路径信息
        //参数：bdzStar-起点变电站对象，bdzEnd-终点变电站对象
        //参数：nLevel-遍历检索的层次深度
        //返回：路径经由的变电站对象列表
        public ArrayList SearchAllWay(string iStartNodeId, string iEndNodeID,List<string> listNode,DataTable dtLines, int iLevel=10)
        {
            ArrayList ar = new ArrayList();
            try
            {
                int r = 0;
                float da;   //临时两个点之间的距离

                //首先获取除了起点变电站以外的所有变电站信息备用
                ////////string strSql = "select id from gis_road_node where id<>"+iStartNodeId.ToString()+" order by id";//by yafei排除边缘结点
                ////////DataTable dt = OracleHelper.GetDataTable(strSql);

                //Table table = Session.Current.Catalog.GetTable("LineNode");
                //ITableFeatureCollection fNodeCollection = table as MapInfo.Data.ITableFeatureCollection;

                //取得图元数量
                int iNodeCount = listNode.Count;
                //foreach (Feature f in fNodeCollection)
                //{
                //    iNodeCount++;
                //}

                if (iNodeCount <= 0)
                {
                    return ar;
                }

                r = iNodeCount + 1;

                int[] aBdzId = new int[r];   //存放所有变电站ID
                string[] aBdzName = new string[r]; //存放所有变电站名称

                aBdzId[0] = listNode.IndexOf(iStartNodeId);
                aBdzName[0] = iStartNodeId;

                //for (int i = 1;i < r; i++)
                //{
                //    Feature fNode=fNodeCollection[i - 1] as Feature;
                //    aBdzId[i] = Convert.ToInt32(fNode["ID"]);
                //    aBdzName[i] = Convert.ToInt32(fNode["ID"]);
                //}
                int i = 1;
                foreach (string nodeID in listNode)
                {
                    aBdzId[i] = listNode.IndexOf(nodeID);
                    aBdzName[i] = nodeID;
                    i++;
                }

                Hashtable hashAllMap = new Hashtable();
                foreach (DataRow dr in dtLines.Rows)
                {
                    if (dr["linid"] != null)
                    {
                        string lineid = dr["linid"].ToString();
                        double length = double.MaxValue;
                        try
                        {
                            length = Convert.ToDouble(dr["value"]);
                        }
                        catch
                        { }
                        string[] arrTemp = lineid.Split('-');
                        if (arrTemp.Length == 2)
                        {
                            int FNODE = listNode.IndexOf(arrTemp[0]);
                            int TNODE = listNode.IndexOf(arrTemp[1]);
                            if (FNODE >= 0 && TNODE >= 0)
                            {
                                //如果表中不包含起点
                                if (!hashAllMap.ContainsKey(FNODE))
                                {
                                    Hashtable hashAdj = new Hashtable();
                                    hashAdj.Add(TNODE, length);
                                    hashAllMap.Add(FNODE, hashAdj);
                                }
                                //表中包含起点
                                else
                                {
                                    Hashtable hashAdj = (Hashtable)hashAllMap[FNODE];
                                    if (!hashAdj.ContainsKey(TNODE))
                                    {
                                        hashAdj.Add(TNODE, length);
                                    }
                                }

                                ////如果表中不包含起点
                                //if (!hashAllMap.ContainsKey(FNODE))
                                //{
                                //    Hashtable hashAdj = new Hashtable();
                                //    hashAdj.Add(TNODE, length);
                                //    hashAllMap.Add(FNODE, hashAdj);
                                //}
                                ////表中包含起点
                                //else
                                //{
                                //    Hashtable hashAdj = (Hashtable)hashAllMap[FNODE];
                                //    if (!hashAdj.ContainsKey(TNODE))
                                //    {
                                //        hashAdj.Add(TNODE, length);
                                //    }
                                //}

                                //如果表中不包含起点
                                if (!hashAllMap.ContainsKey(TNODE))
                                {
                                    Hashtable hashAdj = new Hashtable();
                                    hashAdj.Add(FNODE, length);
                                    hashAllMap.Add(TNODE, hashAdj);
                                }

                                //表中包含起点
                                else
                                {
                                    Hashtable hashAdj = (Hashtable)hashAllMap[TNODE];
                                    if (!hashAdj.ContainsKey(FNODE))
                                    {
                                        hashAdj.Add(FNODE, length);
                                    }
                                }
                            }
                        }
                    }
                }

                ////找出终点索引号
                //int nIndex = 0;

                //for (int i = 1; i < r; i++)
                //{
                //    if (aBdzId[i] ==iEndNodeID)
                //    {
                //        nIndex = i;
                //        break;
                //    }
                //}

                MapPath mp = new MapPath(r, iLevel);

                ArrayList arPath = mp.GetAllWays(hashAllMap, listNode.IndexOf(iStartNodeId), listNode.IndexOf(iEndNodeID));
                foreach (ArrayList childPath in arPath)
                {
                    ArrayList aBdz = new ArrayList();
                    foreach (int num in childPath)
                    {
                        aBdz.Add(aBdzName[num]);
                    }
                    ar.Add(aBdz);   //将每一条路径中相关变电站信息全部追加到整个路径链表中
                }

                return ar;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("发生异常："+ex.Message);
                return ar;
            }
        }
    }
}
