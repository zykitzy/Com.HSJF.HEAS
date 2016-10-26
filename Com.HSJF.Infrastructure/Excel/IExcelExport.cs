using NPOI.HSSF.UserModel;
using System.Data;

namespace Com.HSJF.Infrastructure.Excel
{
    /// <summary>
    /// 文件导出接口
    /// </summary>
    public interface IExcelExport
    {
        HSSFWorkbook Export(DataTable dt);
    }
}
