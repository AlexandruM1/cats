﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.Hub;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;
  

namespace Cats.Services.Hub
{

    public class UserHubService : IUserHubService
    {
        private readonly IUnitOfWork _unitOfWork;


        public UserHubService()
        {
            this._unitOfWork = new UnitOfWork();
        }
        #region Default Service Implementation
        public bool AddUserHub(UserHub entity)
        {
            _unitOfWork.UserHubRepository.Add(entity);
            _unitOfWork.Save();
            return true;

        }
        public bool EditUserHub(UserHub entity)
        {
            _unitOfWork.UserHubRepository.Edit(entity);
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteUserHub(UserHub entity)
        {
            if (entity == null) return false;
            _unitOfWork.UserHubRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.UserHubRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.UserHubRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<UserHub> GetAllUserHub()
        {
            return _unitOfWork.UserHubRepository.GetAll();
        }
        public UserHub FindById(int id)
        {
            return _unitOfWork.UserHubRepository.FindById(id);
        }
        public List<UserHub> FindBy(Expression<Func<UserHub, bool>> predicate)
        {
            return _unitOfWork.UserHubRepository.FindBy(predicate);
        }
        #endregion

        public IEnumerable<UserHub> Get(System.Linq.Expressions.Expression<Func<UserHub, bool>> filter = null, Func<IQueryable<UserHub>, IOrderedQueryable<UserHub>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.UserHubRepository.Get(filter, orderBy, includeProperties);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }

       
           
         public void ChangeHub(int userProfileId,int warehouseId)
        {
           
          
            var newdefault = (from w in _unitOfWork.UserHubRepository.GetAll()
                              where w.HubID == warehouseId && w.UserProfileID == userProfileId
                              select w).FirstOrDefault();
            var prevdefaults = (from t in _unitOfWork.UserHubRepository.GetAll()
                                where t.HubID != warehouseId && t.UserProfileID == userProfileId
                                && t.IsDefault.Trim().Equals("1")
                                select t).ToList();
            newdefault.IsDefault = "1";
            foreach (UserHub uw in prevdefaults)
            {
                uw.IsDefault = "0";
            }
            _unitOfWork.Save();
        }
       

        public void AddUserHub(int warehouseID, int userID)
        {
            UserProfile uProfile = _unitOfWork.UserProfileRepository.Get(t=>t.UserProfileID==userID,null,"UserHubs").FirstOrDefault();
            if (uProfile != null)
            {
                var associations = from v in uProfile.UserHubs
                                   where v.HubID == warehouseID
                                   select v;
                if (!associations.Any())
                {
                    var userHub = new UserHub
                                      {
                                          UserProfileID = uProfile.UserProfileID,
                                          HubID = warehouseID,
                                          IsDefault = "1"
                                      };
                    AddUserHub(userHub);
                }
            }
        }

        public void RemoveUserHub(int warehouseID, int userID)
        {
          
            UserProfile uProfile = _unitOfWork.UserProfileRepository.FindById(userID);
            var associations = from v in uProfile.UserHubs
                               where v.HubID == warehouseID
                               select v;
            if (associations.Any())
            {
                UserHub userHub = associations.FirstOrDefault();
                if (userHub != null) DeleteById(userHub.UserHubID);
            }
        }
    }
}


