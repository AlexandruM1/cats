﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public interface IAuditService : IDisposable
    {
        bool AddAudit(Audit audit);
        bool DeleteAudit(Audit audit);
        bool DeleteById(int id);
        bool EditAudit(Audit audit);
        Audit FindById(int id);
        List<Audit> GetAllAudit();
        List<Audit> FindBy(Expression<Func<Audit, bool>> predicate);

        List<FieldChange> GetChanges(string table, string property, string foreignTable, string foreignFeildName,
                                     string foreignFeildKey, string key);

    }
}
