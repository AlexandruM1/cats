﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;


namespace Cats.Services.EarlyWarning
{

    public class ReliefRequisitionService : IReliefRequisitionService
    {

        private readonly IUnitOfWork _unitOfWork;

       

        public ReliefRequisitionService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #region Default Service Implementation
        public bool AddReliefRequisition(ReliefRequisition reliefRequisition)
        {
            _unitOfWork.ReliefRequisitionRepository.Add(reliefRequisition);
            _unitOfWork.Save();
            return true;

        }
        public bool EditReliefRequisition(ReliefRequisition reliefRequisition)
        {
            _unitOfWork.ReliefRequisitionRepository.Edit(reliefRequisition);
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteReliefRequisition(ReliefRequisition reliefRequisition)
        {
            if (reliefRequisition == null) return false;
            _unitOfWork.ReliefRequisitionRepository.Delete(reliefRequisition);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.ReliefRequisitionRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.ReliefRequisitionRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<ReliefRequisition> GetAllReliefRequisition()
        {
            return _unitOfWork.ReliefRequisitionRepository.GetAll();
        }
        public ReliefRequisition FindById(int id)
        {
            return _unitOfWork.ReliefRequisitionRepository.FindById(id);
        }
        public List<ReliefRequisition> FindBy(Expression<Func<ReliefRequisition, bool>> predicate)
        {
            return _unitOfWork.ReliefRequisitionRepository.FindBy(predicate);
        }

        public IEnumerable<RegionalRequest> Get(
          Expression<Func<RegionalRequest, bool>> filter = null,
          Func<IQueryable<RegionalRequest>, IOrderedQueryable<RegionalRequest>> orderBy = null,
          string includeProperties = "")
        {
            return _unitOfWork.RegionalRequestRepository.Get(filter, orderBy, includeProperties);
        }


        #endregion

        public List<ReliefRequisition> GetApprovedRequistion()
        {
            return new List<ReliefRequisition>(){
                new ReliefRequisition(){
                ProgramID=1,
                RegionID=1,
                RequestedBy=1,
                 RequestedDate=DateTime.Today,
                RequisitionNo="XYZ123",
                Round=1,
                Status=1,
                ZoneID=1,
                CommodityID=1
             

                }
            };
        }
        public void AddReliefRequisions(List<ReliefRequisition> reliefRequisitions )
        {
            foreach (var reliefRequisition in reliefRequisitions)
            {
                this._unitOfWork.ReliefRequisitionRepository.Add(reliefRequisition);
            }
           
        } 
        public void Dispose()
        {
            _unitOfWork.Dispose();

        }





        public bool Save()
        {
          _unitOfWork.Save();
            return true;
        }


      
    }
}

