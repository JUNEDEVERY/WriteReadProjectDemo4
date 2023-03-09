using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteReadProjectDemo
{
    public partial class User 
    {
        public string FullName
        {
            get
            {
                return  UserSurname + "" + UserName + "" + UserPatronymic;
            }
        }
    }
}
