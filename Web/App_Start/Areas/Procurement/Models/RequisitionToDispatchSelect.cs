﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Areas.Procurement.Models
{
    public class RequisitionToDispatchSelect
    {
        public int RequisitionID { get; set; }
        public int ZoneID { get; set; }

        public int HubID { get; set; }
        public decimal QuanityInQtl { get; set; }
        public string RequisitionNo { get; set; }
        public string Zone { get; set; }
        public string OrignWarehouse { get; set; }
        public int RequisitionStatus { get; set; }
        public string RequisitionStatusName { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public bool IsSelected { get; set; }
        public RequisitionToDispatchSelectInput Input { get; set; }

        public class RequisitionToDispatchSelectInput
        {
           
          
                public bool IsSelected { get; set; }
                public int Number { get; set; }
           
        }
    }
}