﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{

    public class NeedAssessmentService : INeedAssessmentService
    {
        private readonly IUnitOfWork _unitOfWork;


        public NeedAssessmentService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #region Default Service Implementation
        public bool AddNeedAssessment(NeedAssessmentDetail detail)
        {

            _unitOfWork.NeedAssessmentDetailRepository.Add(detail);
            _unitOfWork.Save();
            return true;

        }
        public bool EditNeedAssessment(NeedAssessment needAssessment)
        {
            _unitOfWork.NeedAssessmentRepository.Edit(needAssessment);
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteNeedAssessment(NeedAssessment needAssessment)
        {
            if (needAssessment == null) return false;
            _unitOfWork.NeedAssessmentRepository.Delete(needAssessment);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.NeedAssessmentRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.NeedAssessmentRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<NeedAssessment> GetAllNeedAssessment()
        {
            return _unitOfWork.NeedAssessmentRepository.GetAll();
        }
        public NeedAssessment FindById(int id)
        {
            return _unitOfWork.NeedAssessmentRepository.FindById(id);
        }
        public List<NeedAssessment> FindBy(Expression<Func<NeedAssessment, bool>> predicate)
        {
            return _unitOfWork.NeedAssessmentRepository.FindBy(predicate);
        }
        #endregion

        public IEnumerable<NeedAssessmentHeaderViewModel> ReturnViewModel()
        {
            var needAssessment = _unitOfWork.NeedAssessmentRepository.GetAll().ToList();
            return needAssessment.Select(need =>  new NeedAssessmentHeaderViewModel()
                                                                              {
                                                                                  NeedAID = need.NeedAID,
                                                                                  Region = need.Region, 
                                                                                  RegionName = need.AdminUnit.Name, 
                                                                                  Season = need.Season, 
                                                                                  NeedADate = (DateTime) need.NeedADate, 
                                                                                  NeedAApproved = need.NeedAApproved,
                                                                                  NeedACreaterName = need.UserProfile1.UserName,
                                                                                  NeedACreatedBy = need.NeddACreatedBy,
                                                                                  TypeOfNeedAssessment = need.TypeOfNeedAssessment
                                                                              });
        }

        public IEnumerable<NeedAssessmentViewModel> ReturnNeedAssessmentHeaderViewModel(int regionId)
        {
            List<NeedAssessmentHeader> needAssessment = _unitOfWork.NeedAssessmentHeaderRepository.Get(r => r.NeedAssessment.Region == regionId).ToList();
            return needAssessment.Select(need => need.NeedAssessment.NeedADate != null ? new NeedAssessmentViewModel()
                                                                                             {
                                                                                                 NeedAID = (int) need.NeedAID,
                                                                                                 Region = need.NeedAssessment.Region, 
                                                                                                 ZoneName = need.AdminUnit.Name, 
                                                                                                 WoredaName = need.AdminUnit.Name, 
                                                                                                 RegionName = need.AdminUnit.Name, 
                                                                                                 Season = need.NeedAssessment.Season, 
                                                                                                 NeedADate = (DateTime) need.NeedAssessment.NeedADate, 
                                                                                                 NeedAApproved = need.NeedAssessment.NeedAApproved, 
                                                                                                 NeedAApprovedBy = need.NeedAssessment.NeedAApprovedBy, 
                                                                                                 NeedACreatedBy = need.NeedAssessment.NeddACreatedBy,
                                                                                             } : null);
        }

        public IEnumerable<NeedAssessmentViewModel> ReturnNeedAssessmentDetailViewModel(int region)//,string season)
        {
            List<NeedAssessmentDetail> needAssessment = _unitOfWork.NeedAssessmentDetailRepository.Get(r => r.NeedAssessmentHeader.NeedAssessment.Region == region ).ToList();
            return needAssessment.Select(need => new NeedAssessmentViewModel()
                                                     {
                                                       
                                                         ZoneName = need.AdminUnit.Name, 
                                                         WoredaName = need.AdminUnit.Name,
                                                         ProjectedMale = need.ProjectedMale, 
                                                         ProjectedFemale = need.ProjectedFemale, 
                                                         RegularPSNP = need.RegularPSNP, 
                                                         PSNP = need.PSNP, 
                                                         NonPSNP = need.NonPSNP, 
                                                         Contingencybudget = need.Contingencybudget,
                                                         TotalBeneficiaries = need.TotalBeneficiaries, 
                                                         PSNPFromWoredasMale = need.PSNPFromWoredasMale, 
                                                         PSNPFromWoredasFemale = need.PSNPFromWoredasFemale, 
                                                         PSNPFromWoredasDOA = need.NonPSNPFromWoredasDOA,
                                                         NonPSNPFromWoredasMale = need.NonPSNPFromWoredasFemale,
                                                         NonPSNPFromWoredasFemale = need.NonPSNPFromWoredasFemale, 
                                                         NonPSNPFromWoredasDOA = need.NonPSNPFromWoredasDOA
                                                     });
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }

    }
}


