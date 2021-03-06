﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public interface IUserHubService:IDisposable
    {
       bool AddUserHub(UserHub entity);
        void AddUserHub(int warehouseID, int userID);
        void RemoveUserHub(int warehouseID, int userID);
       bool DeleteUserHub(UserHub entity);
       bool DeleteById(int id);
       bool EditUserHub(UserHub entity);
       UserHub FindById(int id);
       List<UserHub> GetAllUserHub();
       List<UserHub> FindBy(Expression<Func<UserHub, bool>> predicate);
       IEnumerable<UserHub> Get(
                   Expression<Func<UserHub, bool>> filter = null,
                   Func<IQueryable<UserHub>, IOrderedQueryable<UserHub>> orderBy = null,
                   string includeProperties = "");

         void ChangeHub(int userProfileId, int warehouseId);
    }
}

   


