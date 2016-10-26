
using System.CodeDom;
using System.Runtime.CompilerServices;

namespace Com.HSJF.Framework.DAL
{
    public class DictionaryType
    {
        /// <summary>
        /// 婚姻状态
        /// </summary>
        public class MaritalStatus
        {
            /// <summary>
            /// 离婚
            /// </summary>
            public const string Divorced = "-MaritalStatus-Divorced";

            /// <summary>
            /// 已婚
            /// </summary>
            public const string Married = "-MaritalStatus-Married";

            /// <summary>
            /// 未婚
            /// </summary>
            public const string UnMarried = "-MaritalStatus-Unmarried";

            /// <summary>
            /// 丧偶
            /// </summary>
            public const string Widowed = "-MaritalStatus-Widowed";
        }

        /// <summary>
        /// 人员类型
        /// </summary>
        public class PersonType
        {
            /// <summary>
            /// 担保人
            /// </summary>
            public const string DanBaoRen = "-PersonType-DanBaoRen";

            /// <summary>
            /// 担保人配偶
            /// </summary>
            public const string DanBaoRenPeiOu = "-PersonType-DanBaoRenPeiOu";

            /// <summary>
            /// 抵押人
            /// </summary>
            public const string DiYaRen = "-PersonType-DiYaRen";

            /// <summary>
            /// 抵押人配偶
            /// </summary>
            public const string DiYaRenPeiOu = "-PersonType-DiYaRenPeiOu";

            /// <summary>
            /// 借款人
            /// </summary>
            public const string JieKuanRen = "-PersonType-JieKuanRen";

            /// <summary>
            /// 借款人配偶
            /// </summary>
            public const string JieKuanRenPeiOu = "-PersonType-JieKuanRenPeiOu";

            /// <summary>
            /// 其他权利人
            /// </summary>
            public const string QiTaQuanLiRen = "-PersonType-QiTaQuanLiRen";

            /// <summary>
            /// 其他权利人配偶
            /// </summary>
            public const string QiTaQuanLiRenPeiOu = "-PersonType-QiTaQuanLiRenPeiOu";
        }

        /// <summary>
        /// 地址类型
        /// </summary>
        public class AddressType
        {
            /// <summary>
            /// 公司地址
            /// </summary>
            public const string CompanyAddress = "-AddressType-CompanyAddress";

            /// <summary>
            /// 户籍地址
            /// </summary>
            public const string FamilyAddress = "-AddressType-FamilyAddress";

            /// <summary>
            /// 家庭地址
            /// </summary>
            public const string HomeAddress = "-AddressType-HomeAddress";

            /// <summary>
            /// 邮寄地址
            /// </summary>
            public const string MailingAddress = "-AddressType-MailingAddress";
        }

        public class CaseMode
        {
            /// <summary>
            /// 居间
            /// </summary>
            public const string JuJian = "-CaseMode-JuJian";

            /// <summary>
            /// 债转
            /// </summary>
            public const string ZhaiZhuan = "-CaseMode-ZhaiZhuan";

            /// <summary>
            /// 自有资金
            /// </summary>
            public const string ZiYouZiJin = "-CaseMode-ZiYouZiJin";
            /// <summary>
            /// 特别为数据权限准备的  为选择案件模式
            /// </summary>
            public const string WeiXuanZe = "-WeiXuanZe";

        }

        public class ThirdPlatform
        {
            /// <summary>
            /// 点融
            /// </summary>
            public const string DianRong = "-ThirdPlatform-DianRong";

            /// <summary>
            /// 聚财猫
            /// </summary>
            public const string JuCaiMao = "-ThirdPlatform-JuCaiMao";

            /// <summary>
            /// 米缸
            /// </summary>
            public const string MiGang = "-ThirdPlatform-MiGang";

            /// <summary>
            /// 挖财
            /// </summary>
            public const string WaCai = "-ThirdPlatform-WaCai";
        }

        /// <summary>
        /// 贷款期数
        /// </summary>
        public class LoanTerm
        {
            public const string M12 = "-LoanTerm-12M";

            public const string M2 = "-LoanTerm-2M";

            public const string M3 = "-LoanTerm-3M";

            public const string M6 = "-LoanTerm-6M";
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public class DocType
        {
            public const string HongKong = "-DocType-HongKong";

            public const string IdCard = "-DocType-IDCard";

            public const string Passport = "-DocType-Passport";

            public const string TaiWan = "-DocType-TaiWan";
        }

        /// <summary>
        /// 学历
        /// </summary>
        public class Education
        {
            /// <summary>
            /// 本科
            /// </summary>
            public const string Bachelor = "-Education-Bachelor";

            /// <summary>
            /// 专科
            /// </summary>
            public const string JuniorCollege = "-Education-JuniorCollege";

            /// <summary>
            /// 初中
            /// </summary>
            public const string JuniorHigh = "-Education-JuniorHigh";

            /// <summary>
            /// 研究生
            /// </summary>
            public const string Master = "-Education-Master";

            /// <summary>
            /// 高中
            /// </summary>
            public const string SeniorHigh = "-Education-SeniorHigh";

        }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public class EmergencyType
        {
            /// <summary>
            /// 父母
            /// </summary>
            public const string FuMu = "-EmergencyType-FuMu";

            /// <summary>
            /// 夫妻
            /// </summary>
            public const string FuQi = "-EmergencyType-FuQi";

            /// <summary>
            /// 朋友
            /// </summary>
            public const string PengYou = "-EmergencyType-PengYou";

            /// <summary>
            /// 亲属
            /// </summary>
            public const string QinShu = "-EmergencyType-QinShu";

            /// <summary>
            /// 其他
            /// </summary>
            public const string QiTa = "-EmergencyType-QiTa";

            /// <summary>
            /// 同事
            /// </summary>
            public const string TongShi = "-EmergencyType-TongShi";

            /// <summary>
            /// 子女
            /// </summary>
            public const string ZiNv = "-EmergencyType-ZiNv";
        }

        /// <summary>
        /// 抵押物分类
        /// </summary>
        public class FacilityCategary
        {
            /// <summary>
            /// 备用放
            /// </summary>
            public const string BackupFacility = "-FacilityCategary-BackupFacility";

            /// <summary>
            /// 抵押房
            /// </summary>
            public const string MainFacility = "-FacilityCategary-MainFacility";
        }

    }
}
