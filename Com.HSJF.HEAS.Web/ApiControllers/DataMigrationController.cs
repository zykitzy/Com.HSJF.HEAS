using System;
using System.Web.Http;
using Com.HSJF.HEAS.BLL.DataMigration;
using Com.HSJF.HEAS.Web.Models;

namespace Com.HSJF.HEAS.Web.ApiControllers
{
    /// <summary>
    /// 添加字段 数据迁移
    /// </summary>
    public class DataMigrationController : ApiController
    {
        /// <summary>
        /// 数据迁移系统
        /// </summary>
        /// <param name="key">迁移目标</param>
        /// <param name="token">认证Token</param>
        /// <returns></returns>
        public BaseApiResponse<string> Post(int key, string token)
        {
            var response = new BaseApiResponse<string>()
            {
                Status = StatusEnum.Success.ToString()
            };


            if (string.Compare(token, "hsjf", StringComparison.OrdinalIgnoreCase) != 0)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = "非法Token";
                return response;
            }

            try
            {
                var dataMigration = new DataMigration();

                switch (key)
                {
                    case 1:
                        // 更新案件号
                        dataMigration.UpdateCaseNum();
                        break;
                    case 2:
                        // 更新年利率
                        dataMigration.TransferAnnualRate();
                        break;
                    case 3:
                        // 更新审批期限
                        dataMigration.TransferAuditTerm();
                        break;
                    case 4:
                        // 签约合并文件
                        dataMigration.MortgageFileMerge();
                        break;
                    case 5:
                        dataMigration.InsertHisCase();
                        break;
                    case 6:
                        var list = dataMigration.GetNoFileCase();
                        if (list != null)
                        {
                            response.Message = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                        }
                        break;
                    default:
                        response.Status = StatusEnum.Failed.ToString();
                        response.Message = "key无法识别";
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
