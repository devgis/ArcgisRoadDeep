using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using System.Text;
using System.Collections.Generic;
using System.Data.OleDb;

namespace ArcgisApp
{
    public sealed partial class MainForm : Form
    {
        #region class private members
        private IMapControl3 m_mapControl = null;
        private string m_mapDocumentName = string.Empty;
        #endregion

        #region class constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion


        IFeatureLayer pointLayer = null;
        IFeatureLayer lineLayer = null;
        private void MainForm_Load(object sender, EventArgs e)
        {
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;
            string mxdFile = System.IO.Path.Combine(Application.StartupPath, "LineMap\\LineMap.mxd");
            axMapControl1.LoadMxFile(mxdFile);

            /*
            pointLayer = CreateFeatureLayerInmemeory("PLayer", "点图层", axMapControl1.Map.SpatialReference, esriGeometryType.esriGeometryPoint, null);
            lineLayer = CreateFeatureLayerInmemeory("LLayer", "线图层", axMapControl1.Map.SpatialReference, esriGeometryType.esriGeometryPolyline, null);

            axMapControl1.AddLayer(pointLayer);
            axMapControl1.AddLayer(lineLayer);

            InitGPS();//初始化gps
             * */

            List<string> listStartNode = GetNodeInfo();
            cbStart.DataSource = listStartNode;

            List<string> listEndNode = GetNodeInfo();
            cbEnd.DataSource = listEndNode;

            DataTable dtLineDate = OledbHelper.GetDataTable("select * from lineinfo");
        }

        private List<string> GetNodeInfo()
        {
            List<string> list = new List<string>();
            //获取图层
            ILayer pLayer = axMapControl1.get_Layer(0);
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;  //转为要素图层
            IFeatureClass pFeaterClass = pFeatureLayer.FeatureClass; 
            string where = ""; 
            IQueryFilter filter = new QueryFilterClass(); 
            filter.WhereClause = where; 
            IFeatureCursor pFeatcursor = pFeaterClass.Search(filter, false);
            IFeature pFeature = pFeatcursor.NextFeature(); 
            while (pFeature!= null) {
                string nodeid = GetFeatureField(pFeature, "NODEID");
                if (!string.IsNullOrEmpty(nodeid))
                {
                    list.Add(nodeid);
                }
                pFeature = pFeatcursor.NextFeature();
            } 
            return list;
        }

        private string GetFeatureField(IFeature feature,string fieldName)
        {
            if (feature == null || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }
            else
            {
                try
                {
                    return feature.get_Value(feature.Fields.FindField(fieldName)).ToString();
                }
                catch
                {
                    return null;
                }
            }
        }

        #region gps相关参数
        string strReceivedNoComplete;//串口单次接收数据不完整时保存本次接收结果，用于下次拼接

        bool bGPGGA_DataComplete = true;//true 数据完整，false数据不完整,GPGGA数据
        bool bGPGSA_DataComplete = true;//true 数据完整，false数据不完整,GPGSA
        bool bGPRMC_DataComplete = true;//true 数据完整，false数据不完整,GPRMC
        bool bGPGSV_DataComplete = true;//true 数据完整，false数据不完整,GPGSV

        //委托,将从串口接收到的数据显示到接收框里面
        delegate void handleinterfaceupdatedelegate(System.Object textbox, string text);

        private bool bRecordToDb = false;//自动入库
        //用于画星座图中的卫星
        Rectangle[] myRectangle = new Rectangle[24];
        Rectangle rectLargeCircle = new Rectangle();
        Rectangle rectSmall = new Rectangle();
        //纬度30度线
        Rectangle myRectangle30 = new Rectangle();
        //纬度60度线
        Rectangle myRectangle60 = new Rectangle();
        Rectangle rectCN;

        System.Drawing.Point myPoint = new System.Drawing.Point();
        System.Drawing.Point myPoint2 = new System.Drawing.Point();
        System.Drawing.Point pt1 = new System.Drawing.Point();
        System.Drawing.Point pt2 = new System.Drawing.Point();
        System.Drawing.Point pt3 = new System.Drawing.Point();
        System.Drawing.Point pt4 = new System.Drawing.Point();
        int btnFlag = 0; //gps标记 0 gps未启用 1 第一个点 2 第2个点 其他未启用
        #endregion

        /// <summary>
        /// 在内存中创建图层
        /// </summary>
        /// <param name="DataSetName">数据集名称</param>
        /// <param name="AliaseName">别名</param>
        /// <param name="SpatialRef">空间参考</param>
        /// <param name="GeometryType">几何类型</param>
        /// <param name="PropertyFields">属性字段集合</param>
        /// <returns>IfeatureLayer</returns>
        public static IFeatureLayer CreateFeatureLayerInmemeory(string DataSetName, string AliaseName, ISpatialReference SpatialRef, esriGeometryType GeometryType, IFields PropertyFields)
        {
            IWorkspaceFactory workspaceFactory = new InMemoryWorkspaceFactoryClass();
            ESRI.ArcGIS.Geodatabase.IWorkspaceName workspaceName = workspaceFactory.Create("", "MyWorkspace", null, 0);
            ESRI.ArcGIS.esriSystem.IName name = (IName)workspaceName;
            ESRI.ArcGIS.Geodatabase.IWorkspace inmemWor = (IWorkspace)name.Open();
            IField oField = new FieldClass();
            IFields oFields = new FieldsClass();
            IFieldsEdit oFieldsEdit = null;
            IFieldEdit oFieldEdit = null;
            IFeatureClass oFeatureClass = null;
            IFeatureLayer oFeatureLayer = null;
            try
            {
                oFieldsEdit = oFields as IFieldsEdit;
                oFieldEdit = oField as IFieldEdit;
                if (PropertyFields != null && PropertyFields.FieldCount > 0)
                {
                    for (int i = 0; i < PropertyFields.FieldCount; i++)
                    {
                        oFieldsEdit.AddField(PropertyFields.get_Field(i));
                    }
                }
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.AvgNumPoints_2 = 5;
                geometryDefEdit.GeometryType_2 = GeometryType;
                geometryDefEdit.GridCount_2 = 1;
                geometryDefEdit.HasM_2 = false;
                geometryDefEdit.HasZ_2 = false;
                geometryDefEdit.SpatialReference_2 = SpatialRef;
                oFieldEdit.Name_2 = "SHAPE";
                oFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                oFieldEdit.GeometryDef_2 = geometryDef;
                oFieldEdit.IsNullable_2 = true;
                oFieldEdit.Required_2 = true;
                oFieldsEdit.AddField(oField);
                oFeatureClass = (inmemWor as IFeatureWorkspace).CreateFeatureClass(DataSetName, oFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                (oFeatureClass as IDataset).BrowseName = DataSetName;
                oFeatureLayer = new FeatureLayerClass();
                oFeatureLayer.Name = AliaseName;
                oFeatureLayer.FeatureClass = oFeatureClass;

                ISimpleRenderer pSimpleRenderer = new SimpleRendererClass();
                switch (GeometryType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        pSimpleRenderer.Symbol = GetPointStyle() as ISymbol;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        pSimpleRenderer.Symbol = GetLineStyle() as ISymbol;
                        break;
                }
                IGeoFeatureLayer m_pGeoFeatureLayer = oFeatureLayer as IGeoFeatureLayer;
                m_pGeoFeatureLayer.Renderer = pSimpleRenderer as IFeatureRenderer;
            }
            catch
            {
            }
            finally
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oField);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFields);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFieldsEdit);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFieldEdit);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(name);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceFactory);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceName);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(inmemWor);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFeatureClass);
                }
                catch { }

                GC.Collect();
            }
            return oFeatureLayer;
        }

        #region Main Menu event handlers
        private void menuNewDoc_Click(object sender, EventArgs e)
        {
            //execute New Document command
            ICommand command = new CreateNewDocument();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuOpenDoc_Click(object sender, EventArgs e)
        {
            //execute Open Document command
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuSaveDoc_Click(object sender, EventArgs e)
        {
            //execute Save Document command
            if (m_mapControl.CheckMxFile(m_mapDocumentName))
            {
                //create a new instance of a MapDocument
                IMapDocument mapDoc = new MapDocumentClass();
                mapDoc.Open(m_mapDocumentName, string.Empty);

                //Make sure that the MapDocument is not readonly
                if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                {
                    MessageBox.Show("Map document is read only!");
                    mapDoc.Close();
                    return;
                }

                //Replace its contents with the current map
                mapDoc.ReplaceContents((IMxdContents)m_mapControl.Map);

                //save the MapDocument in order to persist it
                mapDoc.Save(mapDoc.UsesRelativePaths, false);

                //close the MapDocument
                mapDoc.Close();
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            //execute SaveAs Document command
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuExitApp_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }
        #endregion

        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            m_mapDocumentName = m_mapControl.DocumentFilename;
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        private void AddPoint(double x, double y)
        {
            IFeatureClass pFeatCls = pointLayer.FeatureClass;//定义一个要素集合，并获取图层的要素集合  
            IFeatureClassWrite fr = (IFeatureClassWrite)pFeatCls;//定义一个实现新增要素的接口实例，并该实例作用于当前图层的要素集  
            IWorkspaceEdit w = (pFeatCls as IDataset).Workspace as IWorkspaceEdit;//定义一个工作编辑工作空间，用于开启前图层的编辑状态  
            IFeature f;//定义一个IFeature实例，用于添加到当前图层上  
            w.StartEditing(true);//开启编辑状态  
            w.StartEditOperation();//开启编辑操作  
            IPoint p;//定义一个点，用来作为IFeature实例的形状属性，即shape属性  
            //下面是设置点的坐标和参考系  
            p = new PointClass();
            p.SpatialReference = this.axMapControl1.SpatialReference;
            p.X = x;
            p.Y = y;

            //将IPoint设置为IFeature的shape属性时，需要通过中间接口IGeometry转换  
            IGeometry peo;
            peo = p;
            f = pFeatCls.CreateFeature();//实例化IFeature对象， 这样IFeature对象就具有当前图层上要素的字段信息  
            f.Shape = peo;//设置IFeature对象的形状属性  
            
            //f.set_Value(3, "house1");//设置IFeature对象的索引是3的字段值  
            f.Store();//保存IFeature对象  
            fr.WriteFeature(f);//将IFeature对象，添加到当前图层上  
            w.StopEditOperation();//停止编辑操作  
            w.StopEditing(true);//关闭编辑状态，并保存修改  
            //this.axMapControl1.Refresh();//刷新地图  
        }

        private void AddLine(double x1, double y1, double x2, double y2)
        {
            IFeatureClass pFeatCls = lineLayer.FeatureClass;//定义一个要素集合，并获取图层的要素集合  
            IFeatureClassWrite fr = (IFeatureClassWrite)pFeatCls;//定义一个实现新增要素的接口实例，并该实例作用于当前图层的要素集  
            IWorkspaceEdit w = (pFeatCls as IDataset).Workspace as IWorkspaceEdit;//定义一个工作编辑工作空间，用于开启前图层的编辑状态  
            IFeature f;//定义一个IFeature实例，用于添加到当前图层上  
            w.StartEditing(true);//开启编辑状态  
            w.StartEditOperation();//开启编辑操作  
            IPoint p1;//定义一个点，用来作为IFeature实例的形状属性，即shape属性  
            //下面是设置点的坐标和参考系  
            p1 = new PointClass();
            p1.SpatialReference = this.axMapControl1.SpatialReference;
            p1.X = x1;
            p1.Y = y1;

            IPoint p2;//定义一个点，用来作为IFeature实例的形状属性，即shape属性  
            //下面是设置点的坐标和参考系  
            p2 = new PointClass();
            p2.SpatialReference = this.axMapControl1.SpatialReference;
            p2.X = x2;
            p2.Y = y2;

            IPointCollection m_PointCollection = new PolylineClass();
            m_PointCollection.AddPoint(p1);
            m_PointCollection.AddPoint(p2);

            IPolyline m_Polyline = new PolylineClass();
            
            m_Polyline = m_PointCollection as IPolyline;

            //将IPoint设置为IFeature的shape属性时，需要通过中间接口IGeometry转换  
            IGeometry peo;
            peo = m_Polyline;
            f = pFeatCls.CreateFeature();//实例化IFeature对象， 这样IFeature对象就具有当前图层上要素的字段信息  
            f.Shape = peo;//设置IFeature对象的形状属性  
            //f.set_Value(3, "house1");//设置IFeature对象的索引是3的字段值  
            f.Store();//保存IFeature对象  
            fr.WriteFeature(f);//将IFeature对象，添加到当前图层上  
            w.StopEditOperation();//停止编辑操作  
            w.StopEditing(true);//关闭编辑状态，并保存修改  
            //this.axMapControl1.Refresh();//刷新地图  
        }

        public static ISimpleMarkerSymbol GetPointStyle()
        {
            //创建SimpleMarkerSymbolClass对象

            ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();

            //创建RgbColorClass对象为pSimpleMarkerSymbol设置颜色

            IRgbColor pRgbColor = new RgbColorClass();

            pRgbColor.Red = 255;

            pSimpleMarkerSymbol.Color = pRgbColor as IColor;

            //设置pSimpleMarkerSymbol对象的符号类型，选择钻石

            pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;

            //设置pSimpleMarkerSymbol对象大小，设置为５

            pSimpleMarkerSymbol.Size = 15;

            //显示外框线

            pSimpleMarkerSymbol.Outline = true;

            //为外框线设置颜色

            IRgbColor pLineRgbColor = new RgbColorClass();

            pLineRgbColor.Green = 255;

            pSimpleMarkerSymbol.OutlineColor = pLineRgbColor as IColor;

            //设置外框线的宽度

            pSimpleMarkerSymbol.OutlineSize = 1;
            return pSimpleMarkerSymbol;
        }

        public static IMarkerLineSymbol GetLineStyle()
        {
            IMarkerLineSymbol pMarkerLine = new MarkerLineSymbol();

            IRgbColor pLineColor = new RgbColorClass();

            pLineColor.Blue = 255;

            pMarkerLine.Color = pLineColor as IColor;
            pMarkerLine.Width = 3;
            return pMarkerLine;
        }
        DataTable dtLineDate = null;
        List<string> listNode = null;
        int currentTime = 0;
        int timeStep = 10;
        private void btRoute_Click(object sender, EventArgs e)
        {
            listNode = GetNodeInfo();
            dtLineDate = OledbHelper.GetDataTable("select * from lineinfo");
            timerRun_Tick(this, e);
            timerRun.Enabled = true;
            btPause.Enabled = true;
            btPause.Text = "暂停";
        }

        private List<string> GetLineIDS(List<string> nodes)
        {
            List<string> lineids = new List<string>();
            if (nodes!=null&&nodes.Count>=2)
            {
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    lineids.Add((nodes[i] + "-" + nodes[i + 1]));
                    lineids.Add((nodes[i+1] + "-" + nodes[i]));
                }
            }
            return lineids;
        }

        private double GetLength(DataRow[] rows, List<List<string>> linenodes,out List<string> list)
        {
            double d = double.MaxValue;
            list = null;
            foreach (List<string> nodes in linenodes)
            {
                if (nodes != null && nodes.Count >= 2)
                {
                    double len = GetLength(rows, nodes);
                    if (len > 0)
                    {
                        if (len < d)
                        {
                            d = len;
                            list = nodes;
                        }
                    }
                }
            }
            
            return d;
        }

        private double GetLength(DataRow[] rows, List<string> linenodes)
        {
            double length = 0;
            if (linenodes != null && linenodes.Count >= 2)
            {
                for (int i = 0; i < linenodes.Count - 1; i++)
                {
                    foreach (DataRow dr in rows)
                    {
                        if ((linenodes[i] + "-" + linenodes[i + 1]).Equals(dr["linid"].ToString()) || (linenodes[i + 1] + "-" + linenodes[i]).Equals(dr["linid"].ToString()))
                        {
                            double d = Convert.ToDouble(dr["value"]);
                            if (d >= PublicVar.MaxValue)
                            {
                                return -1;
                            }
                            length+=d;
                        }
                    }
                }
            }
            return length;
        }

        private void SetLineColor(DataRow[] rows,List<string> lineids)
        {
            //获取图层
            ILayer pLayer = axMapControl1.get_Layer(1);
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;  //转为要素图层
            UniqueValueRender(pFeatureLayer, "LINEID",rows,lineids);
        }

        private IMarkerLineSymbol GetLineColorSyle(DataRow[] rows,string id)
        {
            foreach (DataRow dr in rows)
            {
                if (id.Equals(dr["linid"].ToString()))
                {
                    return GetLineStyle(dr["color"].ToString());
                }
            }
            return (GetLineStyle("黑"));
        }

        public static IMarkerLineSymbol GetLineStyle(string color)
        {
            IMarkerLineSymbol pMarkerLine = new MarkerLineSymbol();

            IRgbColor pLineColor = new RgbColorClass();
            pMarkerLine.Width = 2;
            switch (color)
            {
                case "红":
                    pLineColor.Red = 255;
                    break;
                case "黑":
                    pLineColor.Red = 0;
                    pLineColor.Green = 0;
                    pLineColor.Blue = 0;
                    break;
                case "黄":
                    pLineColor.Red = 0XFF;
                    pLineColor.Green = 0XD7;
                    pLineColor.Blue = 0X00;
                    break;
                case "橙":
                   pLineColor.Red = 0XFF;
                    pLineColor.Green = 0XA5;
                    pLineColor.Blue = 0X00;
                    break;
                case "绿":
                    pLineColor.Green = 255;
                    pMarkerLine.Width = 5;
                    break;
                default:
                    pLineColor.Red = 0;
                    pLineColor.Green = 0;
                    pLineColor.Blue = 0;
                    break;
            }
            

            pMarkerLine.Color = pLineColor as IColor;
            
            return pMarkerLine;
        }

        /// <summary>
        /// 颜色序列（非最短路径的线路）
        /// </summary>
        /// <param name="pFeaLyr"></param>
        /// <param name="fieldname"></param>
        private void UniqueValueRender(IFeatureLayer pFeaLyr, string fieldname, DataRow[] rows, List<string> lineids)
        {
            IGeoFeatureLayer pGeoFeatLyr = pFeaLyr as IGeoFeatureLayer;
            ITable pTable = pFeaLyr as ITable;
            IUniqueValueRenderer pUniqueValueRender = new UniqueValueRendererClass();

            int intFieldNumber = pTable.FindField(fieldname);
            pUniqueValueRender.FieldCount = 1;//设置唯一值符号化的关键字段为一个
            pUniqueValueRender.set_Field(0, fieldname);//设置唯一值符号化的第一个关键字段

            //根据渲染字段的值的个数，设置一组随机颜色，如某一字段有5个值，则创建5个随机颜色与之匹配
            IQueryFilter pQueryFilter = new QueryFilterClass();
            bool bSuccess = false;
            IColor pNextUniqueColor= new RgbColorClass();//= this.GetColor(255, 0, 0);
            //查询字段的值
            pQueryFilter = new QueryFilterClass();
            pQueryFilter.AddField(fieldname);
            ICursor pCursor = pTable.Search(pQueryFilter, true);
            IRow pNextRow = pCursor.NextRow();
            IRowBuffer pNextRowBuffer = null;

            while (pNextRow != null)
            {
                pNextRowBuffer = pNextRow as IRowBuffer;
                string codeValue = pNextRowBuffer.get_Value(intFieldNumber).ToString();//lineid
                if (lineids != null && lineids.Contains(codeValue))
                {
                    pUniqueValueRender.AddValue(codeValue, "", (ISymbol)GetLineStyle("绿"));//添加渲染字段的值和渲染样式
                }
                else
                {
                    pUniqueValueRender.AddValue(codeValue, "", (ISymbol)GetLineColorSyle(rows, codeValue));//添加渲染字段的值和渲染样式
                }
                pNextRow = pCursor.NextRow();
            }
            IRotationRenderer pRotationRender = pUniqueValueRender as IRotationRenderer;
            pRotationRender.RotationField = "ANGEL";
            pRotationRender.RotationType = esriSymbolRotationType.esriRotateSymbolArithmetic;

            ISizeRenderer pSizeRender = pUniqueValueRender as ISizeRenderer;
            pSizeRender.SizeRendererFlags = (int)esriSizeRendererFlags.esriSizeRendererUseExpression;
            pSizeRender.SizeRendererExpression = "[SIZE]";

            pGeoFeatLyr.Renderer = pUniqueValueRender as IFeatureRenderer;
            axMapControl1.Refresh();
            //axTOCControl1.Update();
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            currentTime = 0;
            btPause.Enabled = false;
            btPause.Text = "暂停";
            timerRun.Enabled = false;
        }

        private void btPause_Click(object sender, EventArgs e)
        {
            if (btPause.Text == "暂停")
            {
                timerRun.Enabled = false;
                btPause.Text = "恢复";
            }
            else
            {
                timerRun.Enabled = true;
                btPause.Text = "暂停";
            }
        }

        private void timerRun_Tick(object sender, EventArgs e)
        {
            if (dtLineDate == null || dtLineDate.Rows.Count <= 0)
            {
                return;
            }
            DataRow[] useLineRows = dtLineDate.Select("time=" + currentTime);
            if (useLineRows != null && useLineRows.Length > 0)
            {
                var rs = LineSearchClass.SearchAllWay(cbStart.Text, cbEnd.Text, listNode, useLineRows, 10);


                List<string> listselectlines = null;
                if (rs != null && rs.Count > 0)
                {
                    List<string> list = null;
                    double minLength = GetLength(useLineRows, rs, out list);
                    if (list != null && list.Count >= 0)
                    {
                        string s = string.Empty;
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == list.Count - 1)
                            {
                                s += list[i];
                            }
                            else
                            {
                                s += list[i] + "->";
                            }
                        }
                        tbResult.Text = s;
                        tbMinLength.Text = minLength.ToString();
                        tbTime.Text = minLength / PublicVar.speed + "秒";
                        //设置选中线颜色
                        listselectlines = GetLineIDS(list);

                    }
                    else
                    {
                        tbResult.Text = "无效路径！";
                        tbMinLength.Text = string.Empty;
                        tbTime.Text = string.Empty;
                        MessageHelper.ShowError("未找到有效路径！");
                    }
                }
                else
                {
                    tbResult.Text = "无效路径！";
                    tbMinLength.Text = string.Empty;
                    tbTime.Text = string.Empty;
                }
                //设置线颜色
                SetLineColor(useLineRows, listselectlines);
            }
            if (currentTime >= 600)
            {
                btReset.PerformClick();
            }
            else
            {
                currentTime += timeStep;
            }
        }

        /// <summary>
        /// 生成模拟数据
        /// </summary>
        private void GenerateLineData()
        {
            List<string> listsqls = new List<string>();
            List<OleDbParameter[]> listparams = new List<OleDbParameter[]>();

            string sql = "delete from lineinfo";
            listsqls.Add(sql);
            listparams.Add(null);

            //获取图层
            ILayer pLayer = axMapControl1.get_Layer(1);
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;  //转为要素图层
            IFeatureClass pFeaterClass = pFeatureLayer.FeatureClass;
            string where = "";
            IQueryFilter filter = new QueryFilterClass();
            filter.WhereClause = where;
            IFeatureCursor pFeatcursor = pFeaterClass.Search(filter, false);
            IFeature pFeature = pFeatcursor.NextFeature();
            Random rd=new Random();
            string[] colors=new string[]{"红","黑","黄","橙"};
            while (pFeature != null)
            {
                string lineid = GetFeatureField(pFeature, "LINEID");
                if (!string.IsNullOrEmpty(lineid))
                {
                    for (int time = 0; time <= 600; time+=10)
                    {
                        sql = "insert into lineinfo([time],linid,[value],color)values(?,?,?,?)";
                        listsqls.Add(sql);

                        OleDbParameter[] parms = new OleDbParameter[]
                        { 
                            new OleDbParameter("time",time),
                            new OleDbParameter("linid",lineid),
                            new OleDbParameter("value",rd.Next(1,20)),
                            new OleDbParameter("color",colors[rd.Next(0,4)])
                        };
                        listparams.Add(parms);
                    }
                }
                pFeature = pFeatcursor.NextFeature();
            }
            if (OledbHelper.ExecSqlByTran(listsqls, listparams))
            {
                MessageHelper.ShowInfo("生成数据成功！");
            }
            else
            {
                MessageHelper.ShowError("生成数据失败！");
            }
        }

        private void btGenData_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowQuestion("该操作会删除原有数据且不可恢复，确定执行？"))
            {
                GenerateLineData();
            }
        }
    }
}