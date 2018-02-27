﻿using System.Security.Principal;
using TalentGo;

namespace TalentGoWebApp.Models
{
    public class LoginPartialViewModel
    {
        public IIdentity identity { get; set; }
        public string DisplayName { get; set; }

        public Person TargetUser { get; set; }
    }
}