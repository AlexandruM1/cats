﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats.Models;

namespace Cats.Services.Common
{
    

   public interface ILedgerService
   {
       LedgerService.AvailableShippingCodes GetFreeSICodes(int hubId);
       decimal GetFreeSICodes(int hubId, int siCode);
       decimal GetAvailableAmount(int siCode);
   }
}
