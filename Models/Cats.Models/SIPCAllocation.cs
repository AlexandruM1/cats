﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
    public partial class SIPCAllocation
    {
        public int ISPCAllocationID { get; set; }
        public int FDPID { get; set; }
        public int RequisitionDetailID { get; set; }
        public int Code { get; set; }
        public decimal AllocatedAmount { get; set; }
        public int AllocationType { get; set; }
        public virtual ReliefRequisitionDetail ReliefRequisitionDetail { get; set; }
    }
}