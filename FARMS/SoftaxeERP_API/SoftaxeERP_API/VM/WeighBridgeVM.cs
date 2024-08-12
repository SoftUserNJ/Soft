namespace SoftaxeERP_API.VM
{
    public class WeighBridgeVM
    {
        public int VchNo { get; set; }
        public string Vchtype { get; set; }
        public int FirstWeight { get; set; }
        public int SecondWeight { get; set; }

        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }

        public string FreightType { get; set; }
        public int Freight { get; set; }
        public int GPNO { get; set; }
        public DateTime vchDate { get; set; }

        public string Type { get; set; }

        public int Bags { get; set; }
        public int Qty { get; set; }
        public int StockWeight { get; set; }
        public int PayableWeight { get; set; }

        public string Cmb1 { get; set; }
        public string Cmb2 { get; set; }
        public string Cmb3 { get; set; }

        public int Bags1 { get; set; }

        public int Bags2 { get; set; }


        public int Bags3 { get; set; }



        public float Ded1 { get; set; }

        public float Ded2 { get; set; }


        public float Ded3 { get; set; }

        public int Godownid { get; set; }


        public int LabDedStock { get; set; }

        public int LabDedParty { get; set; }


        public string PartyName { get; set; }
        public string ItemName { get; set; }


        public int BagsDed { get; set; }

        public bool manualWeight { get; set; }

        public List<LabVM> lab { get; set; }




    }
}
