using System;
using System.Collections.Generic;

namespace SoftaxeERP_API.Models;

public partial class TblDemandDetail
{
    public int? Sno { get; set; }

    public string VchType { get; set; }

    public int? VchNo { get; set; }

    public string MsCode { get; set; }

    public DateTime? VchDate { get; set; }

    public string DmCode { get; set; }

    public string Code { get; set; }

    public string Mcode { get; set; }

    public string Descrp { get; set; }

    public double? Qty { get; set; }

    public int? Debit { get; set; }

    public int? Credit { get; set; }

    public int? Bags { get; set; }

    public string BagsType { get; set; }

    public string MatType { get; set; }

    public string GoDown { get; set; }

    public double? Rate { get; set; }

    public string Uom { get; set; }

    public double? DivUom { get; set; }

    public string LocalImport { get; set; }

    public string InwardType { get; set; }

    public int? FinId { get; set; }

    public string LocId { get; set; }

    public int? Uid { get; set; }

    public int? Tucks { get; set; }

    public int? GpNo { get; set; }

    public string Godowns { get; set; }

    public int? Sqty { get; set; }

    public int? Freight { get; set; }

    public string FreightType { get; set; }

    public int? Aprove { get; set; }

    public int? AppBy { get; set; }

    public int? Did { get; set; }

    public int? LastVchNo { get; set; }

    public DateTime? LastVchDate { get; set; }

    public string LastParty { get; set; }

    public double? LastRate { get; set; }

    public double? MinRate { get; set; }

    public double? MaxRate { get; set; }

    public int? Verify { get; set; }

    public int? Audit { get; set; }

    public int? Verifyby { get; set; }

    public int? Auditby { get; set; }

    public int? Sent { get; set; }

    public int Idd { get; set; }

    public int? CmpId { get; set; }
}
