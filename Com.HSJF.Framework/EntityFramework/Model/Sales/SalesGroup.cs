using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;

namespace Com.HSJF.Framework.EntityFramework.Model.Sales
{
    public partial class SalesGroup : EntityModel
    {
        public SalesGroup()
        {
            this.SalesMen = new List<SalesMan>();
        }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ������˾
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// ͳһ���������/ע���
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// ��˾����
        /// </summary>
        public string ShortCode { get; set; }

        /// <summary>
        /// ״̬(1:����)
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        public string DistrictID { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public virtual District District { get; set; }

        /// <summary>
        /// ��������Ա
        /// </summary>
        public virtual ICollection<SalesMan> SalesMen { get; set; }
    }
}
