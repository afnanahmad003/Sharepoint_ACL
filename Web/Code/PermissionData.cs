using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace selftprovisioning.web
{
    public class PermissionData
    {
        public bool HasPassword;
        public string Id;
        public string Roles;
        public string Email;
        public string DisplayName;
        public bool IsInherited;
        public bool PreventsDownload;
        public string Scope;
        public string LinkType;
        public string WebUrl;

    }
}
