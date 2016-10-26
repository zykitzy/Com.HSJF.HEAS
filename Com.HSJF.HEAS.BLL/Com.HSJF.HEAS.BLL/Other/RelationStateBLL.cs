using System;
using System.Linq;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Others;
using Com.HSJF.HEAS.BLL.Other.Dto;

namespace Com.HSJF.HEAS.BLL.Other
{
    //public class RelationStateBLL
    //{
        /// <summary>
        /// 锁定验证
        /// </summary>
        /// <param name="RelationModel"></param>
        /// <returns></returns>
        //public Error QueryRelationStateLocking(RelationStateBLLModel RelationModel)
        //{
        //    RelationStateDAL relationstatedal = new RelationStateDAL();
        //    SalesManDAL saleDal = new SalesManDAL();
        //    RelationState relation = relationstatedal.GetAll(s => s.RelationNumber.Equals(RelationModel.Number)).FirstOrDefault();
        //    Error Error = new Error();
        //    if (relation != null)
        //    {
        //        if (relation.IsLock == 1)
        //        {
        //            Error.Key = "RelationState";
        //            Error.Message = RelationModel.TextName + " " + RelationModel.Name + " 已锁定";
        //        }
        //    }
        //    return Error;
        //}

        /// <summary>
        /// 绑定验证
        /// </summary>
        /// <param name="RelationModel"></param>
        /// <returns></returns>
        //public Error QueryRelationStateBinding(RelationStateBLLModel RelationModel)
        //{
        //    RelationStateDAL relationstatedal = new RelationStateDAL();
        //    SalesManDAL saleDal = new SalesManDAL();
        //    RelationState relation = relationstatedal.GetAll(s => s.RelationNumber.Equals(RelationModel.Number)).FirstOrDefault();
        //    Error Error = new Error();
        //    if (relation != null)
        //    {
        //        if (relation.IsBinding == 1)
        //        {
        //            if (relation.SalesID != RelationModel.SalesID)
        //            {
        //                Error.Key = "RelationState";
        //                Error.Message = RelationModel.TextName + " " + RelationModel.Name + " 已经绑定销售人员 " + (relation.SalesID == null ? "" : saleDal.Get(relation.SalesID) == null ? "" : saleDal.Get(relation.SalesID).Name);
        //            }
        //        }
        //    }
        //    return Error;
        //}

        //public void AddLockRelationState(RelationStateBLLModel RelationModel)
        //{
        //    RelationStateDAL relationstatedal = new RelationStateDAL();
        //    RelationState relation = relationstatedal.GetAll(s => s.RelationNumber.Equals(RelationModel.Number)).FirstOrDefault();
        //    if (relation == null)
        //    {
        //        RelationState Rela = AddRelationState(RelationModel, 0);
        //        Rela.SalesID = RelationModel.SalesID;
        //        relationstatedal.Add(Rela);
        //    }
        //    else
        //    {
        //        RelationState Rela = AddRelationState(RelationModel, relation.IsBinding);
        //        if (relation.IsBinding == 0)
        //        {
        //            Rela.SalesID = RelationModel.SalesID;
        //        }
        //        else
        //        {
        //            Rela.SalesID = relation.SalesID;
        //        }
        //        relationstatedal.Add(Rela);
        //        relationstatedal.Delete(relation);
        //    }
        //   // relationstatedal.AcceptAllChange();
        //}

        //private RelationState AddRelationState(RelationStateBLLModel RelationModel, int IsBinding)
        //{
        //    RelationState Rela = new RelationState();
        //    if (RelationModel.Type.Equals("-PersonType-JieKuanRenPeiOu"))
        //    {
        //        Rela.RelationType = 2;
        //    }
        //    else if (RelationModel.Type.Equals("-PersonType-JieKuanRen"))
        //    {
        //        Rela.RelationType = 1;
        //    }
        //    else if (RelationModel.Type.Equals("-FacilityCategary-MainFacility"))
        //    {
        //        Rela.RelationType = 0;
        //    }
        //    Rela.RelationNumber = RelationModel.Number;
        //    Rela.IsLock = 1;
        //    Rela.IsBinding = IsBinding;
        //    Rela.ID = Guid.NewGuid();
        //    Rela.CreateTime = DateTime.Now;
        //    Rela.State = 0;
        //    Rela.Desc = "";
        //    return Rela;
        //}

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="RelationModel"></param>
        //public void UpdateLockRelationState(RelationStateBLLModel RelationModel)
        //{
        //    RelationStateDAL relationstatedal = new RelationStateDAL();
        //    RelationState relation = relationstatedal.GetAll(s => s.RelationNumber.Equals(RelationModel.Number)).FirstOrDefault();
        //    if (relation != null)
        //    {
        //        relation.IsLock = 0;
        //        if (RelationModel.Desc == "Bind")
        //        {
        //            relation.IsBinding = 1;
        //        }
        //        relation.CreateTime = DateTime.Now;
        //        relationstatedal.Update(relation);
        //        relationstatedal.AcceptAllChange();
        //    }
        //}

        ///// <summary>
        ///// 加锁解绑
        ///// </summary>
        ///// <param name="RelationModel"></param>
        //public void UpdateBindRelationState(RelationStateBLLModel RelationModel)
        //{
        //    RelationStateDAL relationstatedal = new RelationStateDAL();
        //    RelationState relation = relationstatedal.GetAll(s => s.RelationNumber.Equals(RelationModel.Number)).FirstOrDefault();
        //    if (relation != null)
        //    {
        //        relation.IsLock = 0;
        //        relation.IsBinding = 1;
        //        relation.CreateTime = DateTime.Now;
        //        relationstatedal.Update(relation);
        //    }
        //}
    //}
}