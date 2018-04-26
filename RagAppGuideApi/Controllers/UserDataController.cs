using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Configuration;

using System.Collections.Specialized;
using System.DirectoryServices.AccountManagement;

//using System.Web.Http.Cors;

namespace RagAppGuideApi.Controllers
{
    
    public class UserDataController : Controller
    {
        //[Produces("application/json")]
        [HttpGet]
        [Route("api/UserData")]
        public JsonResult Login()
        {

            string AdUserName = null;
            bool IsValidUser = false;
            ADUser user = new ADUser();


            //string serverUser = Request.ServerVariables["LOGON_USER"];
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                AdUserName = System.Web.HttpContext.Current.User.Identity.Name;
                AdUserName = AdUserName.Replace("MIG\\", "");
            }

            //
            if (DoesUserExist(AdUserName))
            //if (DoesUserExist(Environment.UserName))
            {
                //List<GroupPrincipal> userGroup = GetGroups(Environment.UserName);
                List<GroupPrincipal> userGroup = GetGroups(AdUserName);

                //string[] groupSections = ((NameValueCollection)ConfigurationManager.GetSection("ADGroupSection")).AllKeys;
                string ADGroup = ConfigurationManager.AppSettings["ADGroup"];

                foreach (GroupPrincipal usrGrp in userGroup)
                {
                    if (usrGrp.Name == ADGroup)
                    {

                        user.userName = AdUserName;
                        user.domain = usrGrp.SamAccountName;
                        user.group = usrGrp.Name;
                        //
                        IsValidUser = true;

                        break;
                    }
                    //foreach (string group in groupSections)
                    //{
                    //    if (usrGrp.Name == group.ToString())
                    //    {
                    //        IsValidUser = true;
                    //    }
                    //}
                }

                if (IsValidUser)
                {   
                    return Json(user);
                }
                else
                {   
                    return Json(user);
                }
            }
            else
            {
                return Json(user);
            }

        }

        [NonAction]
        public List<GroupPrincipal> GetGroups(string userName)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();

            // establish domain context
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain);

            // find your user
            UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, userName);

            // if found - grab its groups
            if (user != null)
            {
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

                // iterate over all groups
                foreach (Principal p in groups)
                {
                    // make sure to add only group principals
                    if (p is GroupPrincipal)
                    {
                        result.Add((GroupPrincipal)p);
                    }
                }
            }

            return result;
        }

        [NonAction]
        private static bool DoesUserExist(string userName)
        {
            if (userName == null)
            {
                return false;
            }
            //using (var domainContext = new PrincipalContext(ContextType.Domain, ((NameValueCollection)ConfigurationManager.GetSection("ADUserDomains")).Get("Domain")))
            using (var domainContext = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["Domain"]))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
                {
                    return foundUser != null;
                }
            }
        }
    }

    public class ADUser
    {
        //
        public ADUser() { }

        public virtual string userName { get; set; }
        public virtual string domain { get; set; }
        public virtual string group { get; set; }

    }

}