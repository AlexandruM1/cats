﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cats.Data.UnitWork;

namespace Cats.Services.Common
{
    public class CommonService:ICommonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Models.CommodityType> GetCommodityTypes(System.Linq.Expressions.Expression<Func<Models.CommodityType, bool>> filter = null, Func<IQueryable<Models.CommodityType>, IOrderedQueryable<Models.CommodityType>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.CommodityTypeRepository.Get(filter, orderBy, includeProperties);
        }

        public IEnumerable<Models.Commodity> GetCommodities(System.Linq.Expressions.Expression<Func<Models.Commodity, bool>> filter = null, Func<IQueryable<Models.Commodity>, IOrderedQueryable<Models.Commodity>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.CommodityRepository.Get(filter, orderBy, includeProperties);
            
        }

        public IEnumerable<Models.Donor> GetDonors(System.Linq.Expressions.Expression<Func<Models.Donor, bool>> filter = null, Func<IQueryable<Models.Donor>, IOrderedQueryable<Models.Donor>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.DonorRepository.Get(filter, orderBy, includeProperties);
        }

        public IEnumerable<Models.Program> GetPrograms(System.Linq.Expressions.Expression<Func<Models.Program, bool>> filter = null, Func<IQueryable<Models.Program>, IOrderedQueryable<Models.Program>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.ProgramRepository.Get(filter, orderBy, includeProperties);
        }
        public IEnumerable<Models.Ration> GetRations(System.Linq.Expressions.Expression<Func<Models.Ration, bool>> filter = null, Func<IQueryable<Models.Ration>, IOrderedQueryable<Models.Ration>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.RationRepository.Get(filter, orderBy, includeProperties);
        }

        public IEnumerable<Models.AdminUnit> GetAminUnits(System.Linq.Expressions.Expression<Func<Models.AdminUnit, bool>> filter = null, Func<IQueryable<Models.AdminUnit>, IOrderedQueryable<Models.AdminUnit>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.AdminUnitRepository.Get(filter, orderBy, includeProperties);
        }
       


     

        public IEnumerable<Models.Detail> GetDetails(System.Linq.Expressions.Expression<Func<Models.Detail, bool>> filter = null, Func<IQueryable<Models.Detail>, IOrderedQueryable<Models.Detail>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.DetailRepository.Get(filter, orderBy, includeProperties);
        }
       
        public string GetStatusName(Models.Constant.WORKFLOW workflow, int statusId)
        {
            var workflowStatus =
                _unitOfWork.WorkflowStatusRepository.Get(t => t.WorkflowID == (int)workflow && t.StatusID == statusId).
                    FirstOrDefault();
            return workflowStatus != null ? workflowStatus.Description :
             string.Empty;
        }
        public List<Models.WorkflowStatus> GetStatus(Models.Constant.WORKFLOW workflow)
        {
            return _unitOfWork.WorkflowStatusRepository.Get(t => t.WorkflowID == (int)workflow).ToList();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }


        public IEnumerable<Models.Season> GetSeasons(System.Linq.Expressions.Expression<Func<Models.Season, bool>> filter = null, Func<IQueryable<Models.Season>, IOrderedQueryable<Models.Season>> orderBy = null, string includeProperties = "")
        {
          return  _unitOfWork.SeasonRepository.GetAll();
        }
    }
}