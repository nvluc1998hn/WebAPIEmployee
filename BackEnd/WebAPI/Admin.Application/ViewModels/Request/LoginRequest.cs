﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Application.ViewModels.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string IPClient { get; set; }
    }
}
