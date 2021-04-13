using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
    }
}
