using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.HEAS.Web.Models.Account
{
    public class LoginUser
    {
        public enum StateEnum
        {
            ChangePassword,
            Normal,
            Stop,
        }


        public StateEnum? State { get; set; }

        public int ID { get; set; }

        public string Name { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }
        public string StateValue
        {
            get
            { return State.ToString(); }
            set
            {
                StateEnum s;
                if (Enum.TryParse<StateEnum>(value, out s))
                {
                    State = s;
                }
                else
                {
                    State = (StateEnum?)null;
                }
            }
        }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsManager { get; set; }
        public string LoninGUID { get; set; }

        public int ErrorCount { get; set; }

        public string AreaName { get; set; }

        public int? DeptID { get; set; }
    }
}
