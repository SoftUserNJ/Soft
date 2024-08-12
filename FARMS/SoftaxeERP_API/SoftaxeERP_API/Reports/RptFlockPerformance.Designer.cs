namespace SoftaxeERP_API.Reports
{
    partial class RptFlockPerformance
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptFlockPerformance));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.Id = new DevExpress.XtraReports.Parameters.Parameter();
            this.FarmName = new DevExpress.XtraReports.Parameters.Parameter();
            this.Compid = new DevExpress.XtraReports.Parameters.Parameter();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.TotalCost = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell149 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell150 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell151 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell152 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell154 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell156 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell137 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell141 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell145 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell138 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell142 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell146 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell139 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell143 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell147 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell140 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell144 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell148 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell97 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell105 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell113 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell121 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell129 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell98 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell106 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell114 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell122 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell130 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell99 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell107 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell115 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell123 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell131 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell100 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell108 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell116 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell124 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell132 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell101 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell109 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell117 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell125 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell133 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell102 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell110 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell118 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell126 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell134 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell103 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell111 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell119 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell127 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell135 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell201 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell202 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell203 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell204 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell205 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell206 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell207 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell208 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell209 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell210 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell211 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell212 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell104 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell112 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell120 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell128 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell136 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell153 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell155 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell157 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell158 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell159 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell160 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell161 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell162 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell163 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell164 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.PEF = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.Logo = new DevExpress.XtraReports.Parameters.Parameter();
            this.ProfitLossFlock = new DevExpress.XtraReports.UI.CalculatedField();
            this.RemChicks = new DevExpress.XtraReports.UI.CalculatedField();
            this.Livebility = new DevExpress.XtraReports.UI.CalculatedField();
            this.OtherExpAmt = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 15F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 15F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            // 
            // Id
            // 
            this.Id.Description = "Id";
            this.Id.Name = "Id";
            this.Id.Type = typeof(int);
            this.Id.ValueInfo = "0";
            this.Id.Visible = false;
            // 
            // FarmName
            // 
            this.FarmName.Description = "FarmName";
            this.FarmName.Name = "FarmName";
            this.FarmName.Visible = false;
            // 
            // Compid
            // 
            this.Compid.Name = "Compid";
            this.Compid.Type = typeof(int);
            this.Compid.ValueInfo = "0";
            this.Compid.Visible = false;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "DefaultConnection";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "RptFlockPerformance";
            queryParameter1.Name = "@id";
            queryParameter1.Type = typeof(global::DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?Id", typeof(int));
            queryParameter2.Name = "@cmpId";
            queryParameter2.Type = typeof(global::DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("?Compid", typeof(int));
            storedProcQuery1.Parameters.AddRange(new DevExpress.DataAccess.Sql.QueryParameter[] {
            queryParameter1,
            queryParameter2});
            storedProcQuery1.StoredProcName = "RptFlockPerformance";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // TotalCost
            // 
            this.TotalCost.DataMember = "RptFlockPerformance";
            this.TotalCost.Expression = "[ChicksAmt] + [FeedAmt] + [MediAmt] + [DieselAmt] + [RentAmount] + [SalariesAmoun" +
    "t]  + [ElectricityAmount] + [WoodAmount] + [MessAmount] + [ShedEquipmentAmount] " +
    "+ [OtherCost] + [OtherExpAmt]";
            this.TotalCost.Name = "TotalCost";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrTable3,
            this.xrTable4,
            this.xrLabel1});
            this.GroupFooter1.HeightF = 533.8013F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow15,
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow4,
            this.xrTableRow5});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1139F, 161.7193F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell149,
            this.xrTableCell150,
            this.xrTableCell151,
            this.xrTableCell152,
            this.xrTableCell154,
            this.xrTableCell156});
            this.xrTableRow15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(161)))), ((int)(((byte)(216)))));
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.StylePriority.UseBackColor = false;
            this.xrTableRow15.StylePriority.UseForeColor = false;
            this.xrTableRow15.Weight = 1D;
            // 
            // xrTableCell149
            // 
            this.xrTableCell149.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell149.Multiline = true;
            this.xrTableCell149.Name = "xrTableCell149";
            this.xrTableCell149.StylePriority.UseFont = false;
            this.xrTableCell149.StylePriority.UseTextAlignment = false;
            this.xrTableCell149.Text = "Partner Name";
            this.xrTableCell149.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell149.Weight = 4.7031659510639319D;
            // 
            // xrTableCell150
            // 
            this.xrTableCell150.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell150.Multiline = true;
            this.xrTableCell150.Name = "xrTableCell150";
            this.xrTableCell150.StylePriority.UseFont = false;
            this.xrTableCell150.StylePriority.UseTextAlignment = false;
            this.xrTableCell150.Text = "Flock (%)";
            this.xrTableCell150.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell150.Weight = 1.3711455909438413D;
            // 
            // xrTableCell151
            // 
            this.xrTableCell151.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell151.Multiline = true;
            this.xrTableCell151.Name = "xrTableCell151";
            this.xrTableCell151.StylePriority.UseFont = false;
            this.xrTableCell151.StylePriority.UseTextAlignment = false;
            this.xrTableCell151.Text = "Profit & (Loss) ";
            this.xrTableCell151.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell151.Weight = 1.9298065827019637D;
            // 
            // xrTableCell152
            // 
            this.xrTableCell152.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell152.Multiline = true;
            this.xrTableCell152.Name = "xrTableCell152";
            this.xrTableCell152.StylePriority.UseFont = false;
            this.xrTableCell152.StylePriority.UseTextAlignment = false;
            this.xrTableCell152.Text = "Report";
            this.xrTableCell152.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell152.Weight = 3.5063714660874448D;
            // 
            // xrTableCell154
            // 
            this.xrTableCell154.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell154.Multiline = true;
            this.xrTableCell154.Name = "xrTableCell154";
            this.xrTableCell154.StylePriority.UseFont = false;
            this.xrTableCell154.StylePriority.UseTextAlignment = false;
            this.xrTableCell154.Text = "Ratio";
            this.xrTableCell154.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell154.Weight = 4.0750996963874684D;
            // 
            // xrTableCell156
            // 
            this.xrTableCell156.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell156.Multiline = true;
            this.xrTableCell156.Name = "xrTableCell156";
            this.xrTableCell156.StylePriority.UseFont = false;
            this.xrTableCell156.StylePriority.UseTextAlignment = false;
            this.xrTableCell156.Text = "Flock Start and End Date";
            this.xrTableCell156.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell156.Weight = 3.8396936039358409D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell8,
            this.xrTableCell1,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell137,
            this.xrTableCell11,
            this.xrTableCell141,
            this.xrTableCell12,
            this.xrTableCell145});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SH1]")});
            this.xrTableCell5.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "SULTANIA";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell5.Weight = 4.7031654259863513D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([S1] != 0, [S1] ,\'\' )\n")});
            this.xrTableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseBorders = false;
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell8.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell8.Weight = 0.85763074208289747D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(!IsNullOrEmpty([ST1]), \'/\' + [ST1], \'\') ")});
            this.xrTableCell1.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 0.51351537393852364D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([ProfitLossFlock] < 0, \'Green\', \'Maroon\')"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "round(Iif([ST1] == \'A\', (([ProfitLossFlock] * [S1]) / 100), Iif([ST1] == \'Q\', ( (" +
                    "[ProfitLossFlock] / [ChicksQty]) * [S1]), \'\')))")});
            this.xrTableCell9.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell9.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell9.Weight = 1.9298065827019641D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Liveability";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 1.7085228300181623D;
            // 
            // xrTableCell137
            // 
            this.xrTableCell137.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Livebility]")});
            this.xrTableCell137.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell137.Multiline = true;
            this.xrTableCell137.Name = "xrTableCell137";
            this.xrTableCell137.StylePriority.UseFont = false;
            this.xrTableCell137.StylePriority.UseTextAlignment = false;
            this.xrTableCell137.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell137.TextFormatString = "{0:#,#.00 %}";
            this.xrTableCell137.Weight = 1.7978486360692825D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell11.Multiline = true;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "PEF Ratio";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 2.083337128397186D;
            // 
            // xrTableCell141
            // 
            this.xrTableCell141.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PEF]")});
            this.xrTableCell141.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell141.Multiline = true;
            this.xrTableCell141.Name = "xrTableCell141";
            this.xrTableCell141.StylePriority.UseFont = false;
            this.xrTableCell141.StylePriority.UseTextAlignment = false;
            this.xrTableCell141.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell141.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell141.Weight = 1.9917625679902828D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Flock No:";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell12.Weight = 1.9561616557764197D;
            // 
            // xrTableCell145
            // 
            this.xrTableCell145.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[jobno]")});
            this.xrTableCell145.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell145.Multiline = true;
            this.xrTableCell145.Name = "xrTableCell145";
            this.xrTableCell145.StylePriority.UseFont = false;
            this.xrTableCell145.StylePriority.UseTextAlignment = false;
            this.xrTableCell145.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell145.Weight = 1.8835319481594213D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell2,
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell138,
            this.xrTableCell17,
            this.xrTableCell142,
            this.xrTableCell18,
            this.xrTableCell146});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SH2]")});
            this.xrTableCell13.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell13.Multiline = true;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell13.Weight = 4.7031654259863513D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([S2] != 0, [S2] ,\'\' )\n")});
            this.xrTableCell14.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell14.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell14.Weight = 0.85763074208289747D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(!IsNullOrEmpty([ST2]), \'/\' + [ST2], \'\') ")});
            this.xrTableCell2.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell2.Weight = 0.51351537393852364D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([ProfitLossFlock] < 0, \'Green\', \'Maroon\')"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "round(Iif([ST2] == \'A\', ([ProfitLossFlock] * [S2]) / 100, Iif([ST2] == \'Q\', (([Pr" +
                    "ofitLossFlock] / [ChicksQty]) * [S2]), \'\' ) ))")});
            this.xrTableCell15.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell15.Multiline = true;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell15.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell15.Weight = 1.9298065827019641D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "FCR:";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell16.Weight = 1.7085228300181623D;
            // 
            // xrTableCell138
            // 
            this.xrTableCell138.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PEF]")});
            this.xrTableCell138.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell138.Multiline = true;
            this.xrTableCell138.Name = "xrTableCell138";
            this.xrTableCell138.StylePriority.UseFont = false;
            this.xrTableCell138.StylePriority.UseTextAlignment = false;
            this.xrTableCell138.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell138.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell138.Weight = 1.7978486360692825D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell17.Multiline = true;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "Mortality (Bird)";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell17.Weight = 2.083337128397186D;
            // 
            // xrTableCell142
            // 
            this.xrTableCell142.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Mortality]")});
            this.xrTableCell142.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell142.Multiline = true;
            this.xrTableCell142.Name = "xrTableCell142";
            this.xrTableCell142.StylePriority.UseFont = false;
            this.xrTableCell142.StylePriority.UseTextAlignment = false;
            this.xrTableCell142.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell142.Weight = 1.9917625679902828D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell18.Multiline = true;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Start Date:";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell18.Weight = 1.9561616557764197D;
            // 
            // xrTableCell146
            // 
            this.xrTableCell146.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[startdate]")});
            this.xrTableCell146.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell146.Multiline = true;
            this.xrTableCell146.Name = "xrTableCell146";
            this.xrTableCell146.StylePriority.UseFont = false;
            this.xrTableCell146.StylePriority.UseTextAlignment = false;
            this.xrTableCell146.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell146.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell146.Weight = 1.8835319481594213D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell3,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell139,
            this.xrTableCell23,
            this.xrTableCell143,
            this.xrTableCell24,
            this.xrTableCell147});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 2.0763012695312497D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SH3]")});
            this.xrTableCell19.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell19.Multiline = true;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell19.Weight = 4.7031654259863513D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([S3] != 0, [S3] ,\'\' )\n")});
            this.xrTableCell20.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell20.Multiline = true;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseBorders = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell20.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell20.Weight = 0.85763074208289747D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(!IsNullOrEmpty([ST3]), \'/\' + [ST3], \'\') ")});
            this.xrTableCell3.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 0.51351537393852364D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([ProfitLossFlock] < 0, \'Green\', \'Maroon\')"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "round(Iif([ST3] == \'A\', ([ProfitLossFlock] * [S3]) / 100, Iif([ST3] == \'Q\', ( ([P" +
                    "rofitLossFlock] / [ChicksQty]) * [S3]), \'\' ) ))\n")});
            this.xrTableCell21.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell21.Multiline = true;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell21.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell21.Weight = 1.9298065827019641D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell22.Multiline = true;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.RowSpan = 2;
            this.xrTableCell22.StylePriority.UseFont = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "Weight\r\nPer Bird:\r";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell22.Weight = 1.7085228300181623D;
            // 
            // xrTableCell139
            // 
            this.xrTableCell139.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([SaleQty] / ([ChicksQty] - [Mortality])\n == \'NaN\' || [SaleQty] / ([ChicksQty]" +
                    " - [Mortality])\n == \'∞\', \'\', [SaleQty] / ([ChicksQty] - [Mortality])\n) ")});
            this.xrTableCell139.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell139.Multiline = true;
            this.xrTableCell139.Name = "xrTableCell139";
            this.xrTableCell139.RowSpan = 2;
            this.xrTableCell139.StylePriority.UseFont = false;
            this.xrTableCell139.StylePriority.UseTextAlignment = false;
            this.xrTableCell139.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell139.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell139.Weight = 1.7978486360692825D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell23.Multiline = true;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseFont = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "Diff. (Flock\r\nDays – Avg. Sale\r\nDays)";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell23.Weight = 2.083337128397186D;
            // 
            // xrTableCell143
            // 
            this.xrTableCell143.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell143.Multiline = true;
            this.xrTableCell143.Name = "xrTableCell143";
            this.xrTableCell143.StylePriority.UseFont = false;
            this.xrTableCell143.StylePriority.UseTextAlignment = false;
            this.xrTableCell143.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell143.Weight = 1.9917625679902828D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell24.Multiline = true;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "End Date: ";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell24.Weight = 1.9561616557764197D;
            // 
            // xrTableCell147
            // 
            this.xrTableCell147.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[enddate]")});
            this.xrTableCell147.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell147.Multiline = true;
            this.xrTableCell147.Name = "xrTableCell147";
            this.xrTableCell147.StylePriority.UseFont = false;
            this.xrTableCell147.StylePriority.UseTextAlignment = false;
            this.xrTableCell147.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell147.TextFormatString = "{0:dd/MM/yyyy}";
            this.xrTableCell147.Weight = 1.8835319481594213D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell4,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell140,
            this.xrTableCell29,
            this.xrTableCell144,
            this.xrTableCell30,
            this.xrTableCell148});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1.3924719238281256D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SH4]")});
            this.xrTableCell25.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell25.Multiline = true;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell25.Weight = 4.7031654259863513D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([S4] != 0, [S4] ,\'\' )")});
            this.xrTableCell26.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell26.Multiline = true;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseBorders = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell26.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell26.Weight = 0.85763074208289747D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(!IsNullOrEmpty([ST4]), \'/\' + [ST4], \'\') ")});
            this.xrTableCell4.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "xrTableCell4";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 0.51351537393852364D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([ProfitLossFlock] < 0, \'Green\', \'Maroon\')"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "round(Iif([ST4] == \'A\', ([ProfitLossFlock] * [S4]) / 100, Iif([ST4] == \'Q\', ( ([P" +
                    "rofitLossFlock] / [ChicksQty]) * [S4]), \'\' )))")});
            this.xrTableCell27.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell27.Multiline = true;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell27.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell27.Weight = 1.9298065827019641D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell28.Multiline = true;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell28.Weight = 1.7085228300181623D;
            // 
            // xrTableCell140
            // 
            this.xrTableCell140.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell140.Multiline = true;
            this.xrTableCell140.Name = "xrTableCell140";
            this.xrTableCell140.StylePriority.UseFont = false;
            this.xrTableCell140.StylePriority.UseTextAlignment = false;
            this.xrTableCell140.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell140.Weight = 1.7978486360692825D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell29.Multiline = true;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.Text = "Average Sale\r\nAge";
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell29.Weight = 2.083337128397186D;
            // 
            // xrTableCell144
            // 
            this.xrTableCell144.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell144.Multiline = true;
            this.xrTableCell144.Name = "xrTableCell144";
            this.xrTableCell144.StylePriority.UseFont = false;
            this.xrTableCell144.StylePriority.UseTextAlignment = false;
            this.xrTableCell144.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell144.Weight = 1.9917625679902828D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell30.Multiline = true;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StylePriority.UseFont = false;
            this.xrTableCell30.StylePriority.UseTextAlignment = false;
            this.xrTableCell30.Text = "No. of Days:";
            this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell30.Weight = 1.9561616557764197D;
            // 
            // xrTableCell148
            // 
            this.xrTableCell148.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "DateDiffDay([startdate], [enddate])")});
            this.xrTableCell148.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell148.Multiline = true;
            this.xrTableCell148.Name = "xrTableCell148";
            this.xrTableCell148.StylePriority.UseFont = false;
            this.xrTableCell148.StylePriority.UseTextAlignment = false;
            this.xrTableCell148.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell148.Weight = 1.8835319481594213D;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0.0001816523F, 486.6733F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable3.SizeF = new System.Drawing.SizeF(1139F, 24.99994F);
            this.xrTable3.StylePriority.UseBorders = false;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell32,
            this.xrTableCell33,
            this.xrTableCell37,
            this.xrTableCell34,
            this.xrTableCell38,
            this.xrTableCell35,
            this.xrTableCell36,
            this.xrTableCell39,
            this.xrTableCell40});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StylePriority.UseFont = false;
            this.xrTableCell31.StylePriority.UseTextAlignment = false;
            this.xrTableCell31.Text = "Total Farm Rent";
            this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell31.Weight = 1.7544035454399733D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.StylePriority.UseFont = false;
            this.xrTableCell32.StylePriority.UseTextAlignment = false;
            this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell32.Weight = 1.2937603077129443D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.Text = "Rent End Dat";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell33.Weight = 1.4703126929093022D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell37.Multiline = true;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell37.Weight = 1.8805936152347971D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell34.Multiline = true;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseFont = false;
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.Text = "Installment";
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell34.Weight = 1.3184853141557897D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell38.Multiline = true;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StylePriority.UseFont = false;
            this.xrTableCell38.StylePriority.UseTextAlignment = false;
            this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell38.Weight = 1.20566398907112D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell35.Multiline = true;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.StylePriority.UseTextAlignment = false;
            this.xrTableCell35.Text = "Total Rent Paid";
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell35.Weight = 1.6181551129234468D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell36.Multiline = true;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.StylePriority.UseFont = false;
            this.xrTableCell36.StylePriority.UseTextAlignment = false;
            this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell36.Weight = 1.2074014187785311D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell39.Multiline = true;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseFont = false;
            this.xrTableCell39.StylePriority.UseTextAlignment = false;
            this.xrTableCell39.Text = "Remaining Rent";
            this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell39.Weight = 1.6435713712425628D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell40.Multiline = true;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseFont = false;
            this.xrTableCell40.StylePriority.UseTextAlignment = false;
            this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell40.Weight = 1.2508725738257434D;
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0.0001695421F, 181.7447F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow9,
            this.xrTableRow10,
            this.xrTableRow11,
            this.xrTableRow12,
            this.xrTableRow13,
            this.xrTableRow19,
            this.xrTableRow14,
            this.xrTableRow1});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1139F, 258.9286F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell41,
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell65,
            this.xrTableCell73,
            this.xrTableCell81,
            this.xrTableCell89,
            this.xrTableCell97,
            this.xrTableCell105,
            this.xrTableCell113,
            this.xrTableCell121,
            this.xrTableCell129});
            this.xrTableRow7.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableRow7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(161)))), ((int)(((byte)(216)))));
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.StylePriority.UseBackColor = false;
            this.xrTableRow7.StylePriority.UseFont = false;
            this.xrTableRow7.StylePriority.UseForeColor = false;
            this.xrTableRow7.Weight = 1.3571436582293441D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Multiline = true;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StylePriority.UseTextAlignment = false;
            this.xrTableCell41.Text = "Particulars";
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell41.Weight = 1.8587283449500116D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Multiline = true;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.Text = "Qty /\r\nBags";
            this.xrTableCell42.Weight = 0.69236206913499942D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Multiline = true;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.Text = "Avg. Rate";
            this.xrTableCell43.Weight = 0.90017372439650278D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.Multiline = true;
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.Text = "Total\r\nAmount";
            this.xrTableCell65.Weight = 1.0579104160251456D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.Multiline = true;
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.Text = "Cost Per\r\nBird";
            this.xrTableCell73.Weight = 0.73481160902020382D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.Multiline = true;
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.Text = "Cost Per\r\nKg ";
            this.xrTableCell81.Weight = 0.756013836473137D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell89.Multiline = true;
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.StylePriority.UseFont = false;
            this.xrTableCell89.Text = "Particulars";
            this.xrTableCell89.Weight = 1.9386490406859815D;
            // 
            // xrTableCell97
            // 
            this.xrTableCell97.Multiline = true;
            this.xrTableCell97.Name = "xrTableCell97";
            this.xrTableCell97.Text = "Weight /\r\nBags";
            this.xrTableCell97.Weight = 0.69328174529171649D;
            // 
            // xrTableCell105
            // 
            this.xrTableCell105.Multiline = true;
            this.xrTableCell105.Name = "xrTableCell105";
            this.xrTableCell105.Text = "Avg.\r\nRate\r";
            this.xrTableCell105.Weight = 0.81559109275953512D;
            // 
            // xrTableCell113
            // 
            this.xrTableCell113.Multiline = true;
            this.xrTableCell113.Name = "xrTableCell113";
            this.xrTableCell113.Text = "Total\r\nAmount";
            this.xrTableCell113.Weight = 1.1905360517851409D;
            // 
            // xrTableCell121
            // 
            this.xrTableCell121.Multiline = true;
            this.xrTableCell121.Name = "xrTableCell121";
            this.xrTableCell121.Text = "Rate Per\r\nBird";
            this.xrTableCell121.Weight = 0.6714015832785688D;
            // 
            // xrTableCell129
            // 
            this.xrTableCell129.Multiline = true;
            this.xrTableCell129.Name = "xrTableCell129";
            this.xrTableCell129.Text = "Rate Per\r\nKg";
            this.xrTableCell129.Weight = 0.6905404861990575D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell45,
            this.xrTableCell44,
            this.xrTableCell46,
            this.xrTableCell66,
            this.xrTableCell74,
            this.xrTableCell82,
            this.xrTableCell90,
            this.xrTableCell98,
            this.xrTableCell106,
            this.xrTableCell114,
            this.xrTableCell122,
            this.xrTableCell130});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell45.Multiline = true;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.StylePriority.UseTextAlignment = false;
            this.xrTableCell45.Text = "Chicks";
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell45.Weight = 1.8587273732713454D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ChicksQty]")});
            this.xrTableCell44.Multiline = true;
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseTextAlignment = false;
            this.xrTableCell44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell44.TextFormatString = "{0:#,#}";
            this.xrTableCell44.Weight = 0.69236254811457953D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([ChicksAmt] / [ChicksQty] == \'NaN\' || [ChicksAmt] / [ChicksQty] == \'∞\', \'\', [" +
                    "ChicksAmt] / [ChicksQty]) ")});
            this.xrTableCell46.Multiline = true;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StylePriority.UseTextAlignment = false;
            this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell46.TextFormatString = "{0:#,#.00}";
            this.xrTableCell46.Weight = 0.900174217095589D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ChicksAmt]")});
            this.xrTableCell66.Multiline = true;
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.StylePriority.UseTextAlignment = false;
            this.xrTableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell66.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell66.Weight = 1.0579104160251456D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([ChicksAmt] / [ChicksQty] == \'NaN\' || [ChicksAmt] / [ChicksQty] == \'∞\', \'\', [" +
                    "ChicksAmt] / [ChicksQty]) ")});
            this.xrTableCell74.Multiline = true;
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.StylePriority.UseTextAlignment = false;
            this.xrTableCell74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell74.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell74.Weight = 0.73481160902020382D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.Multiline = true;
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.StylePriority.UseTextAlignment = false;
            this.xrTableCell82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell82.Weight = 0.756013836473137D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell90.Multiline = true;
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.StylePriority.UseFont = false;
            this.xrTableCell90.StylePriority.UseTextAlignment = false;
            this.xrTableCell90.Text = "Shed Equipment\'s Cost";
            this.xrTableCell90.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell90.Weight = 1.9386490406859815D;
            // 
            // xrTableCell98
            // 
            this.xrTableCell98.Multiline = true;
            this.xrTableCell98.Name = "xrTableCell98";
            this.xrTableCell98.Weight = 0.69328174529171649D;
            // 
            // xrTableCell106
            // 
            this.xrTableCell106.Multiline = true;
            this.xrTableCell106.Name = "xrTableCell106";
            this.xrTableCell106.Weight = 0.81559109275953512D;
            // 
            // xrTableCell114
            // 
            this.xrTableCell114.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShedEquipmentAmount]")});
            this.xrTableCell114.Multiline = true;
            this.xrTableCell114.Name = "xrTableCell114";
            this.xrTableCell114.StylePriority.UseTextAlignment = false;
            this.xrTableCell114.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell114.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell114.Weight = 1.1905360517851409D;
            // 
            // xrTableCell122
            // 
            this.xrTableCell122.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([ShedEquipmentAmount] / [ChicksQty] == \'NaN\' || [ShedEquipmentAmount] / [Chic" +
                    "ksQty] == \'∞\', \'\', [ShedEquipmentAmount] / [ChicksQty])")});
            this.xrTableCell122.Multiline = true;
            this.xrTableCell122.Name = "xrTableCell122";
            this.xrTableCell122.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell122.Weight = 0.6714015832785688D;
            // 
            // xrTableCell130
            // 
            this.xrTableCell130.Multiline = true;
            this.xrTableCell130.Name = "xrTableCell130";
            this.xrTableCell130.Weight = 0.6905404861990575D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell47,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell67,
            this.xrTableCell75,
            this.xrTableCell83,
            this.xrTableCell91,
            this.xrTableCell99,
            this.xrTableCell107,
            this.xrTableCell115,
            this.xrTableCell123,
            this.xrTableCell131});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell47.Multiline = true;
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.StylePriority.UseTextAlignment = false;
            this.xrTableCell47.Text = "Feed";
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell47.Weight = 1.8587283449500116D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FeedQty]")});
            this.xrTableCell48.Multiline = true;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StylePriority.UseTextAlignment = false;
            this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell48.TextFormatString = "{0:#,#}";
            this.xrTableCell48.Weight = 0.69236206913499942D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([FeedAmt] / [FeedQty] == \'NaN\' || [FeedAmt] / [FeedQty] == \'∞\', \'\', [FeedAmt]" +
                    " / [FeedQty]) \n")});
            this.xrTableCell49.Multiline = true;
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.StylePriority.UseTextAlignment = false;
            this.xrTableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell49.TextFormatString = "{0:#,#.00}";
            this.xrTableCell49.Weight = 0.90017372439650278D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FeedAmt]")});
            this.xrTableCell67.Multiline = true;
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.StylePriority.UseTextAlignment = false;
            this.xrTableCell67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell67.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell67.Weight = 1.0579104160251456D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([FeedAmt] / [ChicksQty] == \'NaN\' || [FeedAmt] / [ChicksQty] == \'∞\', \'\', [Feed" +
                    "Amt] / [ChicksQty]) \n")});
            this.xrTableCell75.Multiline = true;
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.StylePriority.UseTextAlignment = false;
            this.xrTableCell75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell75.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell75.Weight = 0.73481160902020382D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.Multiline = true;
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.StylePriority.UseTextAlignment = false;
            this.xrTableCell83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell83.Weight = 0.756013836473137D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell91.Multiline = true;
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.StylePriority.UseFont = false;
            this.xrTableCell91.StylePriority.UseTextAlignment = false;
            this.xrTableCell91.Text = "Other Cost ";
            this.xrTableCell91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell91.Weight = 1.9386490406859815D;
            // 
            // xrTableCell99
            // 
            this.xrTableCell99.Multiline = true;
            this.xrTableCell99.Name = "xrTableCell99";
            this.xrTableCell99.StylePriority.UseTextAlignment = false;
            this.xrTableCell99.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell99.Weight = 0.69328174529171649D;
            // 
            // xrTableCell107
            // 
            this.xrTableCell107.Multiline = true;
            this.xrTableCell107.Name = "xrTableCell107";
            this.xrTableCell107.StylePriority.UseTextAlignment = false;
            this.xrTableCell107.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell107.Weight = 0.81559109275953512D;
            // 
            // xrTableCell115
            // 
            this.xrTableCell115.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OtherCost]")});
            this.xrTableCell115.Multiline = true;
            this.xrTableCell115.Name = "xrTableCell115";
            this.xrTableCell115.StylePriority.UseTextAlignment = false;
            this.xrTableCell115.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell115.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell115.Weight = 1.1905360517851409D;
            // 
            // xrTableCell123
            // 
            this.xrTableCell123.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([OtherCost] / [ChicksQty] == \'NaN\' || [OtherCost] / [ChicksQty] == \'∞\', \'\', [" +
                    "OtherCost] / [ChicksQty]) ")});
            this.xrTableCell123.Multiline = true;
            this.xrTableCell123.Name = "xrTableCell123";
            this.xrTableCell123.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell123.Weight = 0.6714015832785688D;
            // 
            // xrTableCell131
            // 
            this.xrTableCell131.Multiline = true;
            this.xrTableCell131.Name = "xrTableCell131";
            this.xrTableCell131.Weight = 0.6905404861990575D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell50,
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell68,
            this.xrTableCell76,
            this.xrTableCell84,
            this.xrTableCell92,
            this.xrTableCell100,
            this.xrTableCell108,
            this.xrTableCell116,
            this.xrTableCell124,
            this.xrTableCell132});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell50.Multiline = true;
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.StylePriority.UseFont = false;
            this.xrTableCell50.StylePriority.UseTextAlignment = false;
            this.xrTableCell50.Text = "Medicine";
            this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell50.Weight = 1.8587283449500116D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MediQty]")});
            this.xrTableCell51.Multiline = true;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.StylePriority.UseTextAlignment = false;
            this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell51.TextFormatString = "{0:#,#}";
            this.xrTableCell51.Weight = 0.69236206913499942D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([MediAmt] / [MediQty] == \'NaN\' || [MediAmt] / [MediQty] == \'∞\', \'\', [MediAmt]" +
                    " / [MediQty]) \n\n")});
            this.xrTableCell52.Multiline = true;
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.StylePriority.UseTextAlignment = false;
            this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell52.TextFormatString = "{0:#,#.00}";
            this.xrTableCell52.Weight = 0.90017372439650278D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MediAmt]")});
            this.xrTableCell68.Multiline = true;
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.StylePriority.UseTextAlignment = false;
            this.xrTableCell68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell68.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell68.Weight = 1.0579104160251456D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([MediAmt] / [ChicksQty] == \'NaN\' || [MediAmt] / [ChicksQty] == \'∞\', \'\', [Medi" +
                    "Amt] / [ChicksQty]) ")});
            this.xrTableCell76.Multiline = true;
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.StylePriority.UseTextAlignment = false;
            this.xrTableCell76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell76.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell76.Weight = 0.73481160902020382D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.Multiline = true;
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.StylePriority.UseTextAlignment = false;
            this.xrTableCell84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell84.Weight = 0.756013836473137D;
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Other Expense (\'+ [expense] + \'%)\'")});
            this.xrTableCell92.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell92.Multiline = true;
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.StylePriority.UseFont = false;
            this.xrTableCell92.StylePriority.UseTextAlignment = false;
            this.xrTableCell92.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell92.Weight = 1.9386490406859815D;
            // 
            // xrTableCell100
            // 
            this.xrTableCell100.Multiline = true;
            this.xrTableCell100.Name = "xrTableCell100";
            this.xrTableCell100.StylePriority.UseTextAlignment = false;
            this.xrTableCell100.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell100.Weight = 0.69328174529171649D;
            // 
            // xrTableCell108
            // 
            this.xrTableCell108.Multiline = true;
            this.xrTableCell108.Name = "xrTableCell108";
            this.xrTableCell108.StylePriority.UseTextAlignment = false;
            this.xrTableCell108.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell108.Weight = 0.81559109275953512D;
            // 
            // xrTableCell116
            // 
            this.xrTableCell116.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OtherExpAmt]")});
            this.xrTableCell116.Multiline = true;
            this.xrTableCell116.Name = "xrTableCell116";
            this.xrTableCell116.StylePriority.UseTextAlignment = false;
            this.xrTableCell116.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell116.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell116.Weight = 1.1905360517851409D;
            // 
            // xrTableCell124
            // 
            this.xrTableCell124.Multiline = true;
            this.xrTableCell124.Name = "xrTableCell124";
            this.xrTableCell124.Weight = 0.6714015832785688D;
            // 
            // xrTableCell132
            // 
            this.xrTableCell132.Multiline = true;
            this.xrTableCell132.Name = "xrTableCell132";
            this.xrTableCell132.Weight = 0.6905404861990575D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55,
            this.xrTableCell69,
            this.xrTableCell77,
            this.xrTableCell85,
            this.xrTableCell93,
            this.xrTableCell101,
            this.xrTableCell109,
            this.xrTableCell117,
            this.xrTableCell125,
            this.xrTableCell133});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell53.Multiline = true;
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.StylePriority.UseTextAlignment = false;
            this.xrTableCell53.Text = "Diesel";
            this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell53.Weight = 1.8587283449500116D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DieselQty]")});
            this.xrTableCell54.Multiline = true;
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.StylePriority.UseTextAlignment = false;
            this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell54.TextFormatString = "{0:#,#}";
            this.xrTableCell54.Weight = 0.69236206913499942D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([DieselAmt] / [DieselQty] == \'NaN\' || [DieselAmt] / [DieselQty] == \'∞\', \'\', [" +
                    "DieselAmt] / [DieselQty]) \n\n")});
            this.xrTableCell55.Multiline = true;
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.StylePriority.UseTextAlignment = false;
            this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell55.TextFormatString = "{0:#,#.00}";
            this.xrTableCell55.Weight = 0.90017372439650278D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DieselAmt]")});
            this.xrTableCell69.Multiline = true;
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.StylePriority.UseTextAlignment = false;
            this.xrTableCell69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell69.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell69.Weight = 1.0579104160251456D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([DieselAmt] / [ChicksQty] == \'NaN\' || [DieselAmt] / [ChicksQty] == \'∞\', \'\', [" +
                    "DieselAmt] / [ChicksQty]) \n")});
            this.xrTableCell77.Multiline = true;
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.StylePriority.UseTextAlignment = false;
            this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell77.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell77.Weight = 0.73481160902020382D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.Multiline = true;
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.Weight = 0.756013836473137D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell93.Multiline = true;
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.StylePriority.UseFont = false;
            this.xrTableCell93.StylePriority.UseTextAlignment = false;
            this.xrTableCell93.Text = "Total Cost ";
            this.xrTableCell93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell93.Weight = 1.9386490406859815D;
            // 
            // xrTableCell101
            // 
            this.xrTableCell101.Multiline = true;
            this.xrTableCell101.Name = "xrTableCell101";
            this.xrTableCell101.StylePriority.UseTextAlignment = false;
            this.xrTableCell101.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell101.Weight = 0.69328174529171649D;
            // 
            // xrTableCell109
            // 
            this.xrTableCell109.Multiline = true;
            this.xrTableCell109.Name = "xrTableCell109";
            this.xrTableCell109.StylePriority.UseTextAlignment = false;
            this.xrTableCell109.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell109.Weight = 0.81559109275953512D;
            // 
            // xrTableCell117
            // 
            this.xrTableCell117.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TotalCost]")});
            this.xrTableCell117.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.xrTableCell117.Multiline = true;
            this.xrTableCell117.Name = "xrTableCell117";
            this.xrTableCell117.StylePriority.UseFont = false;
            this.xrTableCell117.StylePriority.UseTextAlignment = false;
            this.xrTableCell117.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell117.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell117.Weight = 1.1905360517851409D;
            // 
            // xrTableCell125
            // 
            this.xrTableCell125.Multiline = true;
            this.xrTableCell125.Name = "xrTableCell125";
            this.xrTableCell125.Weight = 0.6714015832785688D;
            // 
            // xrTableCell133
            // 
            this.xrTableCell133.Multiline = true;
            this.xrTableCell133.Name = "xrTableCell133";
            this.xrTableCell133.Weight = 0.6905404861990575D;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell56,
            this.xrTableCell57,
            this.xrTableCell58,
            this.xrTableCell70,
            this.xrTableCell78,
            this.xrTableCell86,
            this.xrTableCell94,
            this.xrTableCell102,
            this.xrTableCell110,
            this.xrTableCell118,
            this.xrTableCell126,
            this.xrTableCell134});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell56.Multiline = true;
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.StylePriority.UseFont = false;
            this.xrTableCell56.StylePriority.UseTextAlignment = false;
            this.xrTableCell56.Text = "Rent";
            this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell56.Weight = 1.8587283449500116D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.Multiline = true;
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StylePriority.UseTextAlignment = false;
            this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell57.Weight = 0.69236206913499942D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.Multiline = true;
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.StylePriority.UseTextAlignment = false;
            this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell58.Weight = 0.90017372439650278D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RentAmount]")});
            this.xrTableCell70.Multiline = true;
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.StylePriority.UseTextAlignment = false;
            this.xrTableCell70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell70.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell70.Weight = 1.0579104160251456D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([RentAmount] / [ChicksQty] == \'NaN\' || [RentAmount] / [ChicksQty] == \'∞\', \'\'," +
                    " [RentAmount] / [ChicksQty])")});
            this.xrTableCell78.Multiline = true;
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.StylePriority.UseTextAlignment = false;
            this.xrTableCell78.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell78.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell78.Weight = 0.73481160902020382D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.Multiline = true;
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.StylePriority.UseTextAlignment = false;
            this.xrTableCell86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell86.Weight = 0.756013836473137D;
            // 
            // xrTableCell94
            // 
            this.xrTableCell94.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell94.Multiline = true;
            this.xrTableCell94.Name = "xrTableCell94";
            this.xrTableCell94.StylePriority.UseFont = false;
            this.xrTableCell94.StylePriority.UseTextAlignment = false;
            this.xrTableCell94.Text = "Broiler (Sale)";
            this.xrTableCell94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell94.Weight = 1.9386490406859815D;
            // 
            // xrTableCell102
            // 
            this.xrTableCell102.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SaleQty]")});
            this.xrTableCell102.Multiline = true;
            this.xrTableCell102.Name = "xrTableCell102";
            this.xrTableCell102.StylePriority.UseTextAlignment = false;
            this.xrTableCell102.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell102.TextFormatString = "{0:#,#}";
            this.xrTableCell102.Weight = 0.69328174529171649D;
            // 
            // xrTableCell110
            // 
            this.xrTableCell110.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(Abs([SaleAmt] / [SaleQty]) == \'NaN\' || Abs([SaleAmt] / [SaleQty]) == \'∞\', \'\'," +
                    " Abs([SaleAmt] / [SaleQty])) ")});
            this.xrTableCell110.Multiline = true;
            this.xrTableCell110.Name = "xrTableCell110";
            this.xrTableCell110.TextFormatString = "{0:#,#.00}";
            this.xrTableCell110.Weight = 0.81559109275953512D;
            // 
            // xrTableCell118
            // 
            this.xrTableCell118.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SaleAmt]")});
            this.xrTableCell118.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.xrTableCell118.Multiline = true;
            this.xrTableCell118.Name = "xrTableCell118";
            this.xrTableCell118.StylePriority.UseFont = false;
            this.xrTableCell118.StylePriority.UseTextAlignment = false;
            this.xrTableCell118.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell118.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell118.Weight = 1.1905360517851409D;
            // 
            // xrTableCell126
            // 
            this.xrTableCell126.Multiline = true;
            this.xrTableCell126.Name = "xrTableCell126";
            this.xrTableCell126.Weight = 0.6714015832785688D;
            // 
            // xrTableCell134
            // 
            this.xrTableCell134.Multiline = true;
            this.xrTableCell134.Name = "xrTableCell134";
            this.xrTableCell134.Weight = 0.6905404861990575D;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell61,
            this.xrTableCell71,
            this.xrTableCell79,
            this.xrTableCell87,
            this.xrTableCell95,
            this.xrTableCell103,
            this.xrTableCell111,
            this.xrTableCell119,
            this.xrTableCell127,
            this.xrTableCell135});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell59.Multiline = true;
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.StylePriority.UseFont = false;
            this.xrTableCell59.StylePriority.UseTextAlignment = false;
            this.xrTableCell59.Text = "Salaries & Wages - Farms";
            this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell59.Weight = 1.8587283449500116D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.Multiline = true;
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.StylePriority.UseTextAlignment = false;
            this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell60.Weight = 0.69236206913499942D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Multiline = true;
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.StylePriority.UseTextAlignment = false;
            this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell61.Weight = 0.90017372439650278D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SalariesAmount]")});
            this.xrTableCell71.Multiline = true;
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.StylePriority.UseTextAlignment = false;
            this.xrTableCell71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell71.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell71.Weight = 1.0579104160251456D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([SalariesAmount] / [ChicksQty] == \'NaN\' || [SalariesAmount] / [ChicksQty] == " +
                    "\'∞\', \'\', [SalariesAmount] / [ChicksQty])")});
            this.xrTableCell79.Multiline = true;
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.StylePriority.UseTextAlignment = false;
            this.xrTableCell79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell79.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell79.Weight = 0.73481160902020382D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.Multiline = true;
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.StylePriority.UseTextAlignment = false;
            this.xrTableCell87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell87.Weight = 0.756013836473137D;
            // 
            // xrTableCell95
            // 
            this.xrTableCell95.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell95.Multiline = true;
            this.xrTableCell95.Name = "xrTableCell95";
            this.xrTableCell95.StylePriority.UseFont = false;
            this.xrTableCell95.StylePriority.UseTextAlignment = false;
            this.xrTableCell95.Text = "Empty Bags (Sale)";
            this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell95.Weight = 1.9386490406859815D;
            // 
            // xrTableCell103
            // 
            this.xrTableCell103.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BagQty]")});
            this.xrTableCell103.Multiline = true;
            this.xrTableCell103.Name = "xrTableCell103";
            this.xrTableCell103.StylePriority.UseTextAlignment = false;
            this.xrTableCell103.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell103.TextFormatString = "{0:#,#}";
            this.xrTableCell103.Weight = 0.69328174529171649D;
            // 
            // xrTableCell111
            // 
            this.xrTableCell111.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(Abs([BagAmount] / [BagQty]) == \'NaN\' || Abs([BagAmount] / [BagQty]) == \'∞\', \'" +
                    "\', Abs([BagAmount] / [BagQty])) ")});
            this.xrTableCell111.Multiline = true;
            this.xrTableCell111.Name = "xrTableCell111";
            this.xrTableCell111.StylePriority.UseTextAlignment = false;
            this.xrTableCell111.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell111.TextFormatString = "{0:#,#.00}";
            this.xrTableCell111.Weight = 0.81559109275953512D;
            // 
            // xrTableCell119
            // 
            this.xrTableCell119.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BagAmount]")});
            this.xrTableCell119.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.xrTableCell119.Multiline = true;
            this.xrTableCell119.Name = "xrTableCell119";
            this.xrTableCell119.StylePriority.UseFont = false;
            this.xrTableCell119.StylePriority.UseTextAlignment = false;
            this.xrTableCell119.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell119.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell119.Weight = 1.1905360517851409D;
            // 
            // xrTableCell127
            // 
            this.xrTableCell127.Multiline = true;
            this.xrTableCell127.Name = "xrTableCell127";
            this.xrTableCell127.Weight = 0.6714015832785688D;
            // 
            // xrTableCell135
            // 
            this.xrTableCell135.Multiline = true;
            this.xrTableCell135.Name = "xrTableCell135";
            this.xrTableCell135.Weight = 0.6905404861990575D;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell201,
            this.xrTableCell202,
            this.xrTableCell203,
            this.xrTableCell204,
            this.xrTableCell205,
            this.xrTableCell206,
            this.xrTableCell207,
            this.xrTableCell208,
            this.xrTableCell209,
            this.xrTableCell210,
            this.xrTableCell211,
            this.xrTableCell212});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 1D;
            // 
            // xrTableCell201
            // 
            this.xrTableCell201.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell201.Multiline = true;
            this.xrTableCell201.Name = "xrTableCell201";
            this.xrTableCell201.StylePriority.UseFont = false;
            this.xrTableCell201.StylePriority.UseTextAlignment = false;
            this.xrTableCell201.Text = "Electricity Cost";
            this.xrTableCell201.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell201.Weight = 1.8587283449500116D;
            // 
            // xrTableCell202
            // 
            this.xrTableCell202.Multiline = true;
            this.xrTableCell202.Name = "xrTableCell202";
            this.xrTableCell202.StylePriority.UseTextAlignment = false;
            this.xrTableCell202.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell202.Weight = 0.69236206913499942D;
            // 
            // xrTableCell203
            // 
            this.xrTableCell203.Multiline = true;
            this.xrTableCell203.Name = "xrTableCell203";
            this.xrTableCell203.StylePriority.UseTextAlignment = false;
            this.xrTableCell203.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell203.Weight = 0.90017372439650278D;
            // 
            // xrTableCell204
            // 
            this.xrTableCell204.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ElectricityAmount]")});
            this.xrTableCell204.Multiline = true;
            this.xrTableCell204.Name = "xrTableCell204";
            this.xrTableCell204.StylePriority.UseTextAlignment = false;
            this.xrTableCell204.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell204.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell204.Weight = 1.0579104160251456D;
            // 
            // xrTableCell205
            // 
            this.xrTableCell205.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([ElectricityAmount] / [ChicksQty] == \'NaN\' || [ElectricityAmount] / [ChicksQt" +
                    "y] == \'∞\', \'\', [ElectricityAmount] / [ChicksQty])")});
            this.xrTableCell205.Multiline = true;
            this.xrTableCell205.Name = "xrTableCell205";
            this.xrTableCell205.StylePriority.UseTextAlignment = false;
            this.xrTableCell205.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell205.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell205.Weight = 0.73481160902020382D;
            // 
            // xrTableCell206
            // 
            this.xrTableCell206.Multiline = true;
            this.xrTableCell206.Name = "xrTableCell206";
            this.xrTableCell206.StylePriority.UseTextAlignment = false;
            this.xrTableCell206.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell206.Weight = 0.756013836473137D;
            // 
            // xrTableCell207
            // 
            this.xrTableCell207.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell207.Multiline = true;
            this.xrTableCell207.Name = "xrTableCell207";
            this.xrTableCell207.StylePriority.UseFont = false;
            this.xrTableCell207.StylePriority.UseTextAlignment = false;
            this.xrTableCell207.Text = "Other Sale";
            this.xrTableCell207.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell207.Weight = 1.9386490406859815D;
            // 
            // xrTableCell208
            // 
            this.xrTableCell208.Multiline = true;
            this.xrTableCell208.Name = "xrTableCell208";
            this.xrTableCell208.StylePriority.UseTextAlignment = false;
            this.xrTableCell208.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell208.Weight = 0.69328174529171649D;
            // 
            // xrTableCell209
            // 
            this.xrTableCell209.Multiline = true;
            this.xrTableCell209.Name = "xrTableCell209";
            this.xrTableCell209.StylePriority.UseTextAlignment = false;
            this.xrTableCell209.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell209.Weight = 0.81559109275953512D;
            // 
            // xrTableCell210
            // 
            this.xrTableCell210.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OtherSale]")});
            this.xrTableCell210.Multiline = true;
            this.xrTableCell210.Name = "xrTableCell210";
            this.xrTableCell210.StylePriority.UseTextAlignment = false;
            this.xrTableCell210.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell210.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell210.Weight = 1.1905360517851409D;
            // 
            // xrTableCell211
            // 
            this.xrTableCell211.Multiline = true;
            this.xrTableCell211.Name = "xrTableCell211";
            this.xrTableCell211.Weight = 0.6714015832785688D;
            // 
            // xrTableCell212
            // 
            this.xrTableCell212.Multiline = true;
            this.xrTableCell212.Name = "xrTableCell212";
            this.xrTableCell212.Weight = 0.6905404861990575D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell62,
            this.xrTableCell63,
            this.xrTableCell64,
            this.xrTableCell72,
            this.xrTableCell80,
            this.xrTableCell88,
            this.xrTableCell96,
            this.xrTableCell104,
            this.xrTableCell112,
            this.xrTableCell120,
            this.xrTableCell128,
            this.xrTableCell136});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell62.Multiline = true;
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.StylePriority.UseFont = false;
            this.xrTableCell62.StylePriority.UseTextAlignment = false;
            this.xrTableCell62.Text = "Wood Cost";
            this.xrTableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell62.Weight = 1.8587283449500116D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.Multiline = true;
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.StylePriority.UseTextAlignment = false;
            this.xrTableCell63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell63.Weight = 0.69236206913499942D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.Multiline = true;
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.StylePriority.UseTextAlignment = false;
            this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell64.Weight = 0.90017372439650278D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WoodAmount]")});
            this.xrTableCell72.Multiline = true;
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.StylePriority.UseTextAlignment = false;
            this.xrTableCell72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell72.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell72.Weight = 1.0579104160251456D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([WoodAmount] / [ChicksQty] == \'NaN\' || [WoodAmount] / [ChicksQty] == \'∞\', \'\'," +
                    " [WoodAmount] / [ChicksQty])")});
            this.xrTableCell80.Multiline = true;
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.StylePriority.UseTextAlignment = false;
            this.xrTableCell80.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell80.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell80.Weight = 0.73481160902020382D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.Multiline = true;
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.StylePriority.UseTextAlignment = false;
            this.xrTableCell88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell88.Weight = 0.756013836473137D;
            // 
            // xrTableCell96
            // 
            this.xrTableCell96.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell96.Multiline = true;
            this.xrTableCell96.Name = "xrTableCell96";
            this.xrTableCell96.StylePriority.UseFont = false;
            this.xrTableCell96.StylePriority.UseTextAlignment = false;
            this.xrTableCell96.Text = "Profit and ( Loss) on Flock";
            this.xrTableCell96.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell96.Weight = 1.9386490406859815D;
            // 
            // xrTableCell104
            // 
            this.xrTableCell104.Multiline = true;
            this.xrTableCell104.Name = "xrTableCell104";
            this.xrTableCell104.StylePriority.UseTextAlignment = false;
            this.xrTableCell104.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell104.Weight = 0.69328174529171649D;
            // 
            // xrTableCell112
            // 
            this.xrTableCell112.Multiline = true;
            this.xrTableCell112.Name = "xrTableCell112";
            this.xrTableCell112.StylePriority.UseTextAlignment = false;
            this.xrTableCell112.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell112.Weight = 0.81559109275953512D;
            // 
            // xrTableCell120
            // 
            this.xrTableCell120.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProfitLossFlock]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([ProfitLossFlock] < 0, \'Green\', \'Maroon\')")});
            this.xrTableCell120.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell120.Multiline = true;
            this.xrTableCell120.Name = "xrTableCell120";
            this.xrTableCell120.StylePriority.UseFont = false;
            this.xrTableCell120.StylePriority.UseTextAlignment = false;
            this.xrTableCell120.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell120.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell120.Weight = 1.1905360517851409D;
            // 
            // xrTableCell128
            // 
            this.xrTableCell128.Multiline = true;
            this.xrTableCell128.Name = "xrTableCell128";
            this.xrTableCell128.Weight = 0.6714015832785688D;
            // 
            // xrTableCell136
            // 
            this.xrTableCell136.Multiline = true;
            this.xrTableCell136.Name = "xrTableCell136";
            this.xrTableCell136.Weight = 0.6905404861990575D;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell153,
            this.xrTableCell155,
            this.xrTableCell157,
            this.xrTableCell158,
            this.xrTableCell159,
            this.xrTableCell160,
            this.xrTableCell161,
            this.xrTableCell162,
            this.xrTableCell163,
            this.xrTableCell164});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Mess Exp";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell6.Weight = 1.8587283449500116D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell7.Weight = 0.69236206913499942D;
            // 
            // xrTableCell153
            // 
            this.xrTableCell153.Multiline = true;
            this.xrTableCell153.Name = "xrTableCell153";
            this.xrTableCell153.StylePriority.UseTextAlignment = false;
            this.xrTableCell153.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell153.Weight = 0.90017372439650278D;
            // 
            // xrTableCell155
            // 
            this.xrTableCell155.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MessAmount]")});
            this.xrTableCell155.Multiline = true;
            this.xrTableCell155.Name = "xrTableCell155";
            this.xrTableCell155.StylePriority.UseTextAlignment = false;
            this.xrTableCell155.Text = "xrTableCell155";
            this.xrTableCell155.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell155.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell155.Weight = 1.0579104160251456D;
            // 
            // xrTableCell157
            // 
            this.xrTableCell157.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([MessAmount] / [ChicksQty] == \'NaN\' || [MessAmount] / [ChicksQty] == \'∞\', \'\'," +
                    " [MessAmount] / [ChicksQty])")});
            this.xrTableCell157.Multiline = true;
            this.xrTableCell157.Name = "xrTableCell157";
            this.xrTableCell157.StylePriority.UseTextAlignment = false;
            this.xrTableCell157.Text = "xrTableCell157";
            this.xrTableCell157.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell157.TextFormatString = "{0:#,##0.00;(#,##0.00);0}";
            this.xrTableCell157.Weight = 0.73481160902020382D;
            // 
            // xrTableCell158
            // 
            this.xrTableCell158.Multiline = true;
            this.xrTableCell158.Name = "xrTableCell158";
            this.xrTableCell158.StylePriority.UseTextAlignment = false;
            this.xrTableCell158.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell158.Weight = 0.756013836473137D;
            // 
            // xrTableCell159
            // 
            this.xrTableCell159.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.xrTableCell159.Multiline = true;
            this.xrTableCell159.Name = "xrTableCell159";
            this.xrTableCell159.StylePriority.UseFont = false;
            this.xrTableCell159.StylePriority.UseTextAlignment = false;
            this.xrTableCell159.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell159.Weight = 1.9386490406859815D;
            // 
            // xrTableCell160
            // 
            this.xrTableCell160.Multiline = true;
            this.xrTableCell160.Name = "xrTableCell160";
            this.xrTableCell160.StylePriority.UseTextAlignment = false;
            this.xrTableCell160.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell160.Weight = 0.69328174529171649D;
            // 
            // xrTableCell161
            // 
            this.xrTableCell161.Multiline = true;
            this.xrTableCell161.Name = "xrTableCell161";
            this.xrTableCell161.StylePriority.UseTextAlignment = false;
            this.xrTableCell161.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell161.Weight = 0.81559109275953512D;
            // 
            // xrTableCell162
            // 
            this.xrTableCell162.Multiline = true;
            this.xrTableCell162.Name = "xrTableCell162";
            this.xrTableCell162.StylePriority.UseTextAlignment = false;
            this.xrTableCell162.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell162.Weight = 1.1905360517851409D;
            // 
            // xrTableCell163
            // 
            this.xrTableCell163.Multiline = true;
            this.xrTableCell163.Name = "xrTableCell163";
            this.xrTableCell163.Weight = 0.6714015832785688D;
            // 
            // xrTableCell164
            // 
            this.xrTableCell164.Multiline = true;
            this.xrTableCell164.Name = "xrTableCell164";
            this.xrTableCell164.Weight = 0.6905404861990575D;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(161)))), ((int)(((byte)(216)))));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0.0001934992F, 461.6733F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(165.2778F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Rent Details";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PEF
            // 
            this.PEF.DataMember = "RptFlockPerformance";
            this.PEF.Expression = "[SaleQty] / [FeedQty]";
            this.PEF.Name = "PEF";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel21,
            this.xrLabel23,
            this.xrLabel22,
            this.xrPictureBox2,
            this.xrLabel3,
            this.xrLabel2});
            this.ReportHeader.HeightF = 225.5387F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel21
            // 
            this.xrLabel21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[contact]")});
            this.xrLabel21.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(0F, 100.3038F);
            this.xrLabel21.Multiline = true;
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(532.0001F, 24.46636F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "xrLabel8";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel23
            // 
            this.xrLabel23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CmpName]")});
            this.xrLabel23.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(7.629395E-05F, 74.70002F);
            this.xrLabel23.Multiline = true;
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(549.9519F, 25.60379F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "xrLabel5";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Email]")});
            this.xrLabel22.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(0F, 124.7702F);
            this.xrLabel22.Multiline = true;
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(532F, 24.23456F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "xrLabel53";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ImageUrl", "?Logo")});
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(0.0001525879F, 0F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(87.84F, 74.7F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?FarmName")});
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 149.0048F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(1138.999F, 43.11717F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "NAWAZ FARID PROTEIN FARM\r\nSULTANIA POULTRY SERVICES";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Arial", 14F, ((DevExpress.Drawing.DXFontStyle)((DevExpress.Drawing.DXFontStyle.Bold | DevExpress.Drawing.DXFontStyle.Underline))));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(1.000604F, 192.122F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(1138F, 33.41669F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Flock Wise Detail";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // Logo
            // 
            this.Logo.Description = "Logo";
            this.Logo.Name = "Logo";
            this.Logo.Visible = false;
            // 
            // ProfitLossFlock
            // 
            this.ProfitLossFlock.DataMember = "RptFlockPerformance";
            this.ProfitLossFlock.Expression = "[TotalCost] + [SaleAmt] + [BagAmount] + [OtherSale]";
            this.ProfitLossFlock.Name = "ProfitLossFlock";
            // 
            // RemChicks
            // 
            this.RemChicks.DataMember = "RptFlockPerformance";
            this.RemChicks.Expression = "(([ChicksQty] + ROUND(([ChicksQty] / 100) * 2)) - [Mortality])";
            this.RemChicks.Name = "RemChicks";
            // 
            // Livebility
            // 
            this.Livebility.DataMember = "RptFlockPerformance";
            this.Livebility.Expression = "[RemChicks] / ([ChicksQty] + round(([ChicksQty]/ 100) *2))";
            this.Livebility.Name = "Livebility";
            // 
            // OtherExpAmt
            // 
            this.OtherExpAmt.DataMember = "RptFlockPerformance";
            this.OtherExpAmt.Expression = "Round(([EXPBALANCE] * [expense]) / 100)";
            this.OtherExpAmt.Name = "OtherExpAmt";
            // 
            // RptFlockPerformance
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.GroupFooter1,
            this.ReportHeader});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.TotalCost,
            this.PEF,
            this.ProfitLossFlock,
            this.RemChicks,
            this.Livebility,
            this.OtherExpAmt});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "RptFlockPerformance";
            this.DataSource = this.sqlDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Landscape = true;
            this.Margins = new DevExpress.Drawing.DXMargins(15F, 15F, 15F, 15F);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ParameterPanelLayoutItems.AddRange(new DevExpress.XtraReports.Parameters.ParameterPanelLayoutItem[] {
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Id, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.FarmName, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Compid, DevExpress.XtraReports.Parameters.Orientation.Horizontal),
            new DevExpress.XtraReports.Parameters.ParameterLayoutItem(this.Logo, DevExpress.XtraReports.Parameters.Orientation.Horizontal)});
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Id,
            this.FarmName,
            this.Compid,
            this.Logo});
            this.Version = "22.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.Parameters.Parameter Id;
        private DevExpress.XtraReports.Parameters.Parameter FarmName;
        private DevExpress.XtraReports.Parameters.Parameter Compid;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
        private DevExpress.XtraReports.UI.CalculatedField TotalCost;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell149;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell150;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell151;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell152;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell154;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell156;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell137;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell141;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell145;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell138;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell142;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell146;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell20;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell21;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell22;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell139;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell23;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell143;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell24;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell147;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell25;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell26;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell27;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell28;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell140;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell29;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell144;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell30;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell148;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell31;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell32;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell33;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell37;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell34;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell38;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell35;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell36;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell39;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell40;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell41;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell42;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell43;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell65;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell73;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell81;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell97;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell105;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell113;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell121;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell129;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell45;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell44;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell46;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell66;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell74;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell82;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell98;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell106;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell114;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell122;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell130;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell47;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell48;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell49;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell67;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell75;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell83;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell99;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell107;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell115;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell123;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell131;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell50;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell51;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell52;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell68;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell76;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell84;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell100;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell108;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell116;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell124;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell132;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell53;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell54;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell55;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell69;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell77;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell85;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell101;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell109;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell117;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell125;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell133;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell56;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell57;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell58;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell70;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell78;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell86;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell102;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell110;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell118;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell126;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell134;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell59;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell60;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell61;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell71;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell79;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell87;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell103;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell111;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell119;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell127;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell135;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.CalculatedField PEF;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell19;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel xrLabel23;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox2;
        private DevExpress.XtraReports.Parameters.Parameter Logo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.CalculatedField ProfitLossFlock;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow19;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell201;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell202;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell203;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell204;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell205;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell206;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell208;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell209;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell210;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell211;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell212;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell62;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell63;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell64;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell72;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell80;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell88;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell104;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell112;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell120;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell128;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell136;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell153;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell155;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell157;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell158;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell160;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell161;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell162;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell163;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell164;
        private DevExpress.XtraReports.UI.CalculatedField RemChicks;
        private DevExpress.XtraReports.UI.CalculatedField Livebility;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell89;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell90;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell91;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell92;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell93;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell94;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell95;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell207;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell96;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell159;
        private DevExpress.XtraReports.UI.CalculatedField OtherExpAmt;
    }
}
