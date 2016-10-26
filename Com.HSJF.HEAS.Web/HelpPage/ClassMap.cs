using Com.HSJF.Framework.DAL.Biz;

namespace Com.HSJF.HEAS.Web.HelpPage
{
    public class ClassMap
    {
        private RelationPersonDAL perdal;
        private CollateralDAL coldal;
        public ClassMap()
        {
            perdal = new RelationPersonDAL();
            coldal = new CollateralDAL();
        }

        public string Getid(string key,string linkid)
        {
            string caseid = string.Empty;
            switch (key)
            {
                case "RelationPerson.IdentificationFile":
                case "RelationPerson.MarryFile":
                case "RelationPerson.SalaryPersonFile":
                case "RelationPerson.SelfLicenseFile":
                case "RelationPerson.SelfNonLicenseFile":
                case "RelationPerson.SingleFile":
                case "RelationPerson.BirthFile":
                case "RelationPerson.AccountFile":
                    {
                        caseid = perdal.Get(linkid).CaseID;
                        break;
                    }
                case "Collateral.HouseFile":
                    {
                        caseid = coldal.Get(linkid).CaseID;
                        break;
                    }
            }
            return caseid;
        }
    }
}
