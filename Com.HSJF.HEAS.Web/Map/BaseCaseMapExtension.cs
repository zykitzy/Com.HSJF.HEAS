using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.HEAS.Web.Models.Biz;
using EmitMapper;
using EmitMapper.MappingConfiguration;

namespace Com.HSJF.HEAS.Web.Map
{
    public static partial class MapExtension
    {

        public static BaseCase MapToBaseCase(this BaseCaseViewModel model)
        {
            var map = ObjectMapperManager.DefaultInstance.GetMapper<BaseCaseViewModel, BaseCase>(
                new DefaultMapConfig()
                .IgnoreMembers<BaseCaseViewModel, BaseCase>(new[] { "BorrowerPerson", "RelationPerson", "Collateral", "AuditHistory", "Introducer" })
                );

            BaseCase baseCase = map.Map(model);
            baseCase.NewCaseNum = model.CaseNum;
            return baseCase;
        }


        public static BaseCaseViewModel MaptoViewModel(BaseCase entity)
        {
            var map = ObjectMapperManager.DefaultInstance.GetMapper<BaseCase, BaseCaseViewModel>(
                new DefaultMapConfig()
                .IgnoreMembers<BaseCase, BaseCaseViewModel>(new[] { "Collaterals", "RelationPersons", "Introducers" })
                );

            var viewModel = map.Map(entity);
            viewModel.CaseNum = entity.NewCaseNum;

            return viewModel;
        }
    }
}