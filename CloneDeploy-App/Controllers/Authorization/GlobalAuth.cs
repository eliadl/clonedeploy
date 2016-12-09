﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers.Authorization
{
    public class GlobalAuthAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var identity = (ClaimsPrincipal) Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                .Select(c => c.Value).SingleOrDefault();

            if (userId == null) return;


            var authorized = false;
            switch (Permission)
            {
                case "GlobalRead":
                case "GlobalUpdate":
                case "GlobalCreate":
                case "GlobalDelete":
                    if (new AuthorizationServices(Convert.ToInt32(userId), Permission).IsAuthorized())
                        authorized = true;
                    break;

            }

            if (!authorized)
            {
                var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                    new ValidationResultDTO() {Success = false, ErrorMessage = "Forbidden"});
                throw new HttpResponseException(response);
            }
        }
    }
}