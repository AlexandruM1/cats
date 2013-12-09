﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cats.Models
{
    
   public partial class Bid
    {
       public Bid()
       {
           this.BidDetails=new List<BidDetail>();
           this.BidWinners=new List<BidWinner>();
       }
        public int BidID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BidNumber { get; set; }
        public DateTime OpeningDate { get; set; }
        public int StatusID { get; set; }
        public int TransportBidPlanID { get; set; }

        #region Navigation Properties

        public ICollection<BidDetail> BidDetails { get; set; }
        public virtual Status Status { get; set; }
        public virtual TransportBidPlan TransportBidPlan { get; set; }
        public ICollection<BidWinner> BidWinners { get; set; }
        public virtual ICollection<TransportBidQuotation> TransportBidQuotations { get; set; }
       
        #endregion
    }
}