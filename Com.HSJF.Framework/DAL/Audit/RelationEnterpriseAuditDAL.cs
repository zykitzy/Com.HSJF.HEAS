﻿using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.Audit
{
    public class RelationEnterpriseAuditDAL : BaseRepository<RelationEnterpriseAudit, HEASContext>
    {
        public IEnumerable<RelationEnterpriseAudit> GetByPersonID(string personid)
        {
            return base.GetAll().Where(t => t.PersonID == personid);
        }
    }
}
