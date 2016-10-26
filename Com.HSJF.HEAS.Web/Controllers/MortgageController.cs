#region 引用

using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.HEAS.BLL.Audit;
using Com.HSJF.HEAS.BLL.Mortgage;
using Com.HSJF.HEAS.BLL.Other;
using Com.HSJF.HEAS.BLL.Other.Dto;
using Com.HSJF.HEAS.BLL.Sales;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Map;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.HEAS.Web.Models.Mortgage;
using Com.HSJF.HEAS.Web.Validations.PublicMortgage;
using Com.HSJF.Infrastructure.ExtendTools;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

#endregion 引用

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize(Roles = "Public,ConfrimPublic")]
    public class MortgageController : BaseController
    {
        private readonly MortgagePush _mortgagePush;
        private readonly BaseAuditDAL _baseAuditDal;
        private readonly BaseAuditBll _auditBll;
        private readonly MortgageDAL _mortgageDal;
        private readonly MortgageBll _mortgageBll;
        private readonly SalesGroupBll _salesGroupBll;

        public MortgageController()
        {
            _mortgagePush = new MortgagePush();
            _auditBll = new BaseAuditBll();
            _mortgageDal = new MortgageDAL();
            _baseAuditDal = new BaseAuditDAL();
            _mortgageBll = new MortgageBll();
            _salesGroupBll = new SalesGroupBll();
        }

        [Authorize]
        [HttpGet]
        public ActionResult MortgageIndex()
        {
            var salesGroups = new List<SelectListItem>();
            salesGroups.Add(new SelectListItem()
            {
                Selected = true,
                Text = "",
                Value = "",
            });
            _salesGroupBll.GetAll().ForEach(p => salesGroups.Add(new SelectListItem
            {
                Text = p.Name,
                Value = p.ID
            }));
            ViewBag.SaleGroups = salesGroups;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageIndex(BaseAuditListPageRequestViewModel request)
        {
            var response = new BaseAuditListPageResponseViewModel();
            var total = 0;

            var modellist = _baseAuditDal.GetAllAuthorizeAndSelfByPublic(CaseStatus.PublicMortgage, CurrentUser);

            if (!string.IsNullOrEmpty(request.CaseStatus))
            {
                modellist = modellist.Where(t => t.CaseStatus == request.CaseStatus);
            }
            if (!string.IsNullOrEmpty(request.BorrowerName) && !string.IsNullOrWhiteSpace(request.BorrowerName))
            {
                modellist = modellist.Where(t => t.BorrowerName.Contains(request.BorrowerName));
            }
            if (!string.IsNullOrEmpty(request.CaseNum) && !string.IsNullOrWhiteSpace(request.CaseNum))
            {
                modellist = modellist.Where(t => t.NewCaseNum.Contains(request.CaseNum));
            }
            if (request.SalesGroupId.IsNotNullOrWhiteSpace() && request.SalesGroupId.IsNotNullOrEmpty())
            {
                modellist = modellist.Where(t => t.SalesGroupID == request.SalesGroupId);
            }

            var PageList = _baseAuditDal.GetAllPage(modellist, out total, request.PageSize, request.PageIndex, request.Order, request.Sort);
            //var PageList = _baseAuditDal.GetAllPageOrderByCaseStatus(modellist, out total, request.PageSize, request.PageIndex, request.Order, request.Sort);

            var newlist = PageList.ToList().Select(t => new BaseAuditViewModel().CastModel(t));

            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            response.Total = total;
            response.TotalPage = (int)Math.Ceiling((decimal)response.Total / response.PageSize);
            response.Data = newlist;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditMortgage(string id)
        {
            BaseAuditDAL bad = new BaseAuditDAL();

            var entity = bad.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                return RedirectToAction("Failed", "Home");
            }

            var jingbanren = WebConfigurationManager.AppSettings["jinbanren"];
            var viewModel = new PublicMortgageViewModel();

            if (id.IsNullOrWhiteSpace() || id.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Error", "Home");
            }

            var mortgage = _mortgageBll.QueryById(id);
            var auditCase = _auditBll.QueryLeatestById(id);

            viewModel = MortgageMapper.MapToViewModel(auditCase, mortgage);

            //if (mortgage.IsNull())
            //{
            //    viewModel.ID = id;
            //}

            ViewBag.ID = id;
            // 经办人,并且去除admin用户
            ViewBag.PublicUsers = GetUserByPermission(CaseStatus.PublicMortgage)
                .Where(u => !jingbanren.Contains(u.UserName)).ToList();

            return View(viewModel);
        }

        /// <summary>
        /// 签约保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditMortgage(PublicMortgageViewModel model)
        {
            var response = new BaseResponse<string>();

            var bad = new BaseAuditDAL();
            var entity = bad.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }

            var auditCase = _auditBll.QueryLeatest(model.CaseNum);
            if (auditCase.IsNull() || auditCase.CaseStatus != CaseStatus.PublicMortgage)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", "案件已变更"), };
                return Json(response);
            }

            // 更新BaseAudit基础信息
            auditCase.OpeningBank = model.OpeningBank;
            auditCase.OpeningSite = model.OpeningSite;
            auditCase.BankCard = model.BankCard;

            //auditCase.ServiceCharge = model.ServiceCharge;
            //auditCase.ServiceChargeRate = model.ServiceChargeRate;
            auditCase.Deposit = model.Deposit;
            auditCase.DepositDate = model.DepositDate;
            auditCase.IsActivitieRate = model.IsActivitieRate;
            auditCase.RejectReason = model.RejectReason;
            auditCase.LenderName = model.LenderName;

            //auditCase.IntroducerAudits = new List<IntroducerAudit>();
            //model.Introducer.IfNotNull(t =>
            //{
            //    model.Introducer.ForEach(item =>
            //    {
            //        if (item.ID.Contains("TEMP"))
            //        {
            //            item.ID = Guid.NewGuid().ToString();
            //        }

            //        auditCase.IntroducerAudits.Add(item.MaptoIntroducerAudit());
            //    });
            //});

            var mortgage = model.CastDB(model);
            // 合同文件
            mortgage.ContractFile = SaveFiles(model.ContractFile, model.ID, model.ID);
            // 四条
            mortgage.FourFile = SaveFiles(model.FourFile, model.ID, model.ID);
            // 收据
            mortgage.OtherFile = SaveFiles(model.OtherFile, model.ID, model.ID);

            //// 借条
            //mortgage.NoteFile = SaveFiles(model.NoteFile, model.ID, model.ID);
            //// 他证
            //mortgage.ReceiptFile = SaveFiles(model.ReceiptFile, model.ID, model.ID);
            //// 承诺书
            //mortgage.UndertakingFile = SaveFiles(model.UndertakingFile, model.ID, model.ID);
            //// 联系方式确认书
            //mortgage.ContactConfirmFile = SaveFiles(model.ContactConfirmFile, model.ID, model.ID);

            // 还款委托书
            mortgage.RepaymentAttorneyFile = SaveFiles(model.RepaymentAttorneyFile, model.ID, model.ID);
            // 授权委托书
            mortgage.PowerAttorneyFile = SaveFiles(model.PowerAttorneyFile, model.ID, model.ID);
            // 收件收据
            mortgage.CollectionFile = SaveFiles(model.CollectionFile, model.ID, model.ID);

            mortgage.CreateTime = DateTime.Now;
            mortgage.CreateUser = CurrentUser.UserName;

            //  _mortgageBll.Edit(mortgage, auditCase, CurrentUser.UserName);
            //_mortgageDal.Update(mortgage);

            _mortgageDal.Update(mortgage);
            _baseAuditDal.Update(auditCase);

            _mortgageDal.AcceptAllChange();
            return base.GetBaseResponse<object>(true);
        }

        /// <summary>
        /// 查询签约详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPublicMortgage(string id)
        {
            var viewModel = new PublicMortgageViewModel();
            var response = new BaseResponse<PublicMortgageViewModel>();

            var mortgage = _mortgageBll.QueryById(id);
            var auditCase = _auditBll.QueryLeatestById(id);

            viewModel = MortgageMapper.MapToViewModel(auditCase, mortgage);

            if (mortgage.IsNull())
            {
                viewModel.ID = id;
                var ahp = new AuditHisHelper();
                var list = _baseAuditDal.GetListByCaseNum(viewModel.CaseNum);
                viewModel.AuditHistory = ahp.GetHistory(list);
            }
            else
            {
                viewModel.ContractFileName = GetFiles(viewModel.ContractFile);
                viewModel.OtherFileName = GetFiles(viewModel.OtherFile);
                viewModel.FourFileName = GetFiles(viewModel.FourFile);
                //viewModel.NoteFileName = GetFiles(viewModel.NoteFile);
                //viewModel.ReceiptFileName = GetFiles(viewModel.ReceiptFile);
                //viewModel.UndertakingFileName = GetFiles(viewModel.UndertakingFile);
                //viewModel.ContactConfirmFileName = GetFiles(viewModel.ContactConfirmFile);
                viewModel.RepaymentAttorneyFileName = GetFiles(viewModel.RepaymentAttorneyFile);
                viewModel.PowerAttorneyFileName = GetFiles(viewModel.PowerAttorneyFile);
                viewModel.CollectionFileName = GetFiles(viewModel.CollectionFile);

                var ahp = new AuditHisHelper();
                var list = _baseAuditDal.GetListByCaseNum(viewModel.CaseNum);
                viewModel.AuditHistory = ahp.GetHistory(list);
            }

            response.Data = viewModel;
            response.Status = "Success";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交公正签约
        /// </summary>
        /// <param name="model">公正签约信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SubmitMortgage(PublicMortgageViewModel model)
        {
            var response = new BaseResponse<string>();

            var baseAuditDal = new BaseAuditDAL();
            var entity = baseAuditDal.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }
            var publicMortgage = new PublicMortgage();

            #region 验证

            var validator = new MortgageValidator();

            var result = validator.Validate(model);
            if (result.IsNotValid())
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = result.GetErrors().ToArray();
                return Json(response);
            }

            #endregion 验证

            // 更新baseaudit

            var auditCase = _auditBll.QueryLeatest(model.CaseNum);

            if (auditCase.IsNull() && auditCase.CaseStatus == CaseStatus.PublicMortgage)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", "案件已变更"), };
                return Json(response);
            }

            // 更新BaseAudit基础信息
            auditCase.OpeningBank = model.OpeningBank;
            auditCase.OpeningSite = model.OpeningSite;
            auditCase.BankCard = model.BankCard;
            auditCase.LenderName = model.LenderName;//出借人
                                                    //auditCase.ServiceCharge = model.ServiceCharge;
                                                    //auditCase.ServiceChargeRate = model.ServiceChargeRate;
            auditCase.Deposit = model.Deposit;
            auditCase.DepositDate = model.DepositDate;
            auditCase.Description = model.Description;
            auditCase.IsActivitieRate = model.IsActivitieRate;
            auditCase.RejectReason = model.RejectReason;//签约失败理由
            auditCase.LenderName = model.LenderName;
            auditCase.IntroducerAudits = new List<IntroducerAudit>();

            model.Introducer.IfNotNull(t =>
            {
                model.Introducer.ForEach(item =>
                {
                    if (item.ID.Contains("TEMP"))
                    {
                        item.ID = Guid.NewGuid().ToString();
                    }
                    auditCase.IntroducerAudits.Add(item.MaptoIntroducerAudit());
                });
            });

            publicMortgage = model.CastDB(model);

            //合同文件
            publicMortgage.ContractFile = SaveFiles(model.ContractFile, model.ID, model.ID);
            // 四条
            publicMortgage.FourFile = SaveFiles(model.FourFile, model.ID, model.ID);
            //收据
            publicMortgage.OtherFile = SaveFiles(model.OtherFile, model.ID, model.ID);

            ////借条
            //publicMortgage.NoteFile = SaveFiles(model.NoteFile, model.ID, model.ID);
            ////他证
            //publicMortgage.ReceiptFile = SaveFiles(model.ReceiptFile, model.ID, model.ID);
            ////承诺书
            //publicMortgage.UndertakingFile = SaveFiles(model.UndertakingFile, model.ID, model.ID);
            ////联系方式确认书
            //publicMortgage.ContactConfirmFile = SaveFiles(model.ContactConfirmFile, model.ID, model.ID);

            //还款委托书
            publicMortgage.RepaymentAttorneyFile = SaveFiles(model.RepaymentAttorneyFile, model.ID, model.ID);
            //授权委托书
            publicMortgage.PowerAttorneyFile = SaveFiles(model.PowerAttorneyFile, model.ID, model.ID);
            //收件收据
            publicMortgage.CollectionFile = SaveFiles(model.CollectionFile, model.ID, model.ID);
            publicMortgage.CreateUser = CurrentUser.UserName;
            publicMortgage.CreateTime = DateTime.Now;

            // 推送到Hats  20160909 修改，推送在确认签约要件之后

            var isSuccess = _mortgageBll.SubmitCase(publicMortgage, auditCase, CurrentUser.UserName);

            //_mortgageDal.SubmitCase(publicMortgage, model.Description, CurrentUser.UserName);
            //_mortgageDal.AcceptAllChange();
            if (isSuccess)
            {
                response.Status = StatusEnum.Success.ToString();
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", "更新出错") };
            }
            /*
            var publicMortgageDto = new PublicMortgageDto();

            Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, publicMortgageDto);
            if (model.Introducer != null && model.Introducer.Any())
            {
                foreach (var r in model.Introducer)
                {
                    var newIntro = new IntroducerAudit();
                    Infrastructure.ExtendTools.ObjectExtend.CopyTo(r, newIntro);
                    publicMortgageDto.Introducer.Add(newIntro);
                }
            }

            UserDAL ud = new UserDAL();
            var contr = await ud.FindById(publicMortgageDto.ContractPerson);
            publicMortgageDto.ContractPersonText = contr.DisplayName;
            var pushResult = _mortgagePush.PushToHats(publicMortgageDto);
            if (pushResult.IsSuccess)
            {
                bool isSuccess = _mortgageBll.SubmitCase(publicMortgage, auditCase, CurrentUser.UserName);

                //_mortgageDal.SubmitCase(publicMortgage, model.Description, CurrentUser.UserName);
                //_mortgageDal.AcceptAllChange();
                if (isSuccess)
                {
                    response.Status = StatusEnum.Success.ToString();
                }
                else
                {
                    response.Status = StatusEnum.Failed.ToString();
                    response.Message = new[] { new ErrorMessage("", "更新出错") };
                }
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", pushResult.Message) };
            }   */

            return Json(response);
        }

        /// <summary>
        /// 签约失败，退回
        /// </summary>
        /// <param name="id">案件Id</param>
        /// <param name="Description">退回理由</param>
        /// <returns></returns>
        public ActionResult RejectMortgage(string id, string Description, string RejectReason)
        {
            var response = new BaseResponse<string>();
            var baseAuditDal = new BaseAuditDAL();
            var entity = baseAuditDal.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }

            //RelationStateBLL relationstatebll = new RelationStateBLL();
            RelationPersonAuditDAL relationpersonauditdal = new RelationPersonAuditDAL();
            CollateralAuditDAL collateralauditdal = new CollateralAuditDAL();

            var baseaduit = _baseAuditDal.Get(id);
            if (baseaduit == null && baseaduit.CaseStatus != CaseStatus.PublicMortgage)
            {
                response.Status = "Failed";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //IEnumerable<RelationPersonAudit> relationPersonList = relationpersonauditdal.FindByCaseID(baseaduit.ID).ToList();
            //IEnumerable<CollateralAudit> collateralauditList = collateralauditdal.FindByAuditID(baseaduit.ID).ToList();

            //if (relationPersonList.Any())
            //{
            //    foreach (var item in relationPersonList)
            //    {
            //        //RelationState Relation = relationstatebll.QueryRelationState(s => s.RelationNumber.Equals(item.IdentificationNumber));
            //        //if (Relation != null)
            //        //{
            //        //    Relation.IsLock = 0;
            //        //    Relation.CreateTime = DateTime.Now;
            //        //    relationstatebll.UpdateRelationState(Relation);
            //        //}
            //        RelationStateBLLModel RelationbllModel = new RelationStateBLLModel()
            //        {
            //            Number = item.IdentificationNumber,
            //        };
            //        relationstatebll.UpdateLockRelationState(RelationbllModel);
            //    }
            //}
            //if (collateralauditList.Any())
            //{
            //    foreach (var item in collateralauditList)
            //    {
            //        //RelationState Relation = relationstatebll.QueryRelationState(s => s.RelationNumber.Equals(item.HouseNumber));
            //        //if (Relation != null)
            //        //{
            //        //    Relation.IsLock = 0;
            //        //    Relation.CreateTime = DateTime.Now;
            //        //    relationstatebll.UpdateRelationState(Relation);
            //        //}
            //        RelationStateBLLModel RelationbllModel = new RelationStateBLLModel()
            //        {
            //            Number = item.HouseNumber,
            //        };
            //        relationstatebll.UpdateLockRelationState(RelationbllModel);
            //    }
            //}

            var pushResult = _mortgagePush.Reject(baseaduit.NewCaseNum);

            if (pushResult.IsSuccess)
            {
                response.Status = _mortgageDal.RejectCase(id, CurrentUser.UserName, Description, RejectReason) ? "Success" : "Failed";
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", pushResult.Message) };
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region Private Methods

        /// <summary>
        /// 保存附件
        /// </summary>
        /// <param name="filenames">名称</param>
        /// <param name="linkId"></param>
        /// <param name="linkkey"></param>
        /// <returns></returns>
        private string SaveFiles(string filenames, string linkId, string linkkey)
        {
            var up = new FileUpload();//文件上传
            string files = string.Empty;
            if (string.IsNullOrEmpty(filenames))
            {
                return files;
            }
            foreach (var file in filenames.Split(','))
            {
                var filemodel = up.Single(new Guid(file));
                if (filemodel != null)
                {
                    var entity = CopyFile(filemodel);
                    entity.LinkID = Guid.Parse(linkId);
                    entity.LinkKey = linkkey;
                    up.SaveFileDescription(entity);
                    files += entity.ID;
                    files += ",";
                }
            }
            return files.Trim(',');
        }

        private FileDescription CopyFile(FileDescription model)
        {
            var entity = new FileDescription();
            ObjectExtend.CopyTo(model, entity);
            entity.ID = Guid.NewGuid();
            entity.FileCreateTime = DateTime.Now;
            return entity;
        }

        /// <summary>
        /// 查找指定权限的所有用户
        /// </summary>
        /// <param name="permissionName">权限名</param>
        /// <returns>拥有权限的所有的用户信息</returns>
        private IEnumerable<User> GetUserByPermission(string permissionName)
        {
            if (string.IsNullOrEmpty(permissionName.Trim()))
            {
                throw new ArgumentNullException("permissionName", "the argument is not allowed to be null");
            }

            var users = new List<User>();

            Permission permission = new PermissionDAL().GetAll().FirstOrDefault(p => p.Name == permissionName);

            if (permission != null && permission.RolePermission != null && permission.RolePermission.Any())
            {
                permission.RolePermission.ToList().ForEach(p =>
                {
                    if (p.Role != null && p.Role.UserRole != null)
                    {
                        p.Role.UserRole.ToList().ForEach(rp =>
                        {
                            if (rp.User != null)
                            {
                                users.Add(rp.User);
                            }
                        });
                    }
                });
            }

            return users;
        }

        #endregion Private Methods

        /// <summary>
        /// 确认要件通过
        /// </summary>
        /// <param name="caseID"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "ConfrimPublic")]
        public async Task<ActionResult> ConfrimPublic(string id, string description)
        {
            var response = new BaseResponse<string>();
            var baseAuditDal = new BaseAuditDAL();
            var entity = baseAuditDal.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }
            var flag = await _mortgageBll.ConfrimPublic(id, CurrentUser.UserName, description);
            if (flag)
            {
                response.Status = StatusEnum.Success.ToString();
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", "更新出错") };
            }
            return Json(response);
        }

        /// <summary>
        /// 确认要件只读页面
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "ConfrimPublic")]
        public ActionResult MortgageConfirmReadonly(string id)
        {
            var bad = new BaseAuditDAL();
            var entity = bad.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            var jingbanren = WebConfigurationManager.AppSettings["jinbanren"];
            var viewModel = new PublicMortgageViewModel();

            if (id.IsNullOrWhiteSpace() || id.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Error", "Home");
            }

            var mortgage = _mortgageBll.QueryById(id);
            var auditCase = _auditBll.QueryLeatestById(id);

            viewModel = MortgageMapper.MapToViewModel(auditCase, mortgage);

            if (mortgage.IsNull())
            {
                viewModel.ID = id;
            }

            ViewBag.ID = id;
            // 经办人,并且去除admin用户
            ViewBag.PublicUsers = GetUserByPermission(CaseStatus.PublicMortgage)
                .Where(u => !jingbanren.Contains(u.UserName)).ToList();

            return View(viewModel);
        }

        /// <summary>
        /// 签约拒绝只读页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RejectMortgageReadonly(string id)
        {
            var bad = new BaseAuditDAL();
            var entity = bad.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            var jingbanren = WebConfigurationManager.AppSettings["jinbanren"];
            var viewModel = new PublicMortgageViewModel();

            if (id.IsNullOrWhiteSpace() || id.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Error", "Home");
            }

            var mortgage = _mortgageBll.QueryById(id);
            var auditCase = _auditBll.QueryLeatestById(id);

            viewModel = MortgageMapper.MapToViewModel(auditCase, mortgage);

            if (mortgage.IsNull())
            {
                viewModel.ID = id;
            }

            ViewBag.ID = id;
            // 经办人,并且去除admin用户
            ViewBag.PublicUsers = GetUserByPermission(CaseStatus.PublicMortgage)
                .Where(u => !jingbanren.Contains(u.UserName)).ToList();

            return View(viewModel);
        }

        /// <summary>
        /// 确认要见退回
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "ConfrimPublic")]
        public ActionResult RejectPublic(string id, string description)
        {
            var response = new BaseResponse<string>();
            var baseAuditDal = new BaseAuditDAL();
            var entity = baseAuditDal.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }
            response.Status = _mortgageDal.RejectPublic(id, CurrentUser.UserName, description) ? "Success" : "Failed";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 签约之后可补他证
        /// </summary>
        /// <param name="id">案件ID</param>
        /// <param name="fileid">文件ID</param>
        /// <returns></returns>
        public ActionResult PolishingOtherFile(string id, string fileid)
        {
            var response = new BaseResponse<PublicMortgageViewModel>();
            var bo = _mortgageBll.PolishingOtherFile(id, fileid);
            if (bo)
            {
                response.Data = null;
                response.Status = "Success";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            response.Data = null;
            response.Status = "Failed";
            response.Message = new[] { new ErrorMessage("", "更新出错") };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}