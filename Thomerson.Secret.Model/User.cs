﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomerson.Secret.Model
{
    public class User
    {
        public string Id { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
