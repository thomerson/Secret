using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomerson.Secret.Model
{
    public class UserSecret : User
    {
        public string Type { get; set; }
        public string Remark { get; set; }

    }
}
