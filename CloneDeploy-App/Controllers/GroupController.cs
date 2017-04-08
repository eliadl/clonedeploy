﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;
using CloneDeploy_Services.Workflows;


namespace CloneDeploy_App.Controllers
{
    

    public class GroupController : ApiController
    {
        private readonly GroupServices _groupServices;

        public GroupController()
        {
            _groupServices = new GroupServices();
        }


        [CustomAuth(Permission = "GroupSearch")]
        public IEnumerable<GroupEntity> GetAll(string searchstring = "")
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            return string.IsNullOrEmpty(searchstring)
                ? _groupServices.SearchGroupsForUser(Convert.ToInt32(userId))
                : _groupServices.SearchGroupsForUser(Convert.ToInt32(userId), searchstring);

        }

        [CustomAuth(Permission = "GroupSearch")]
        public ApiStringResponseDTO GetCount()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO() {Value = _groupServices.GroupCountUser(Convert.ToInt32(userId))};
        }

        [CustomAuth(Permission = "GroupRead")]
        public GroupEntity Get(int id)
        {
            var result = _groupServices.GetGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuthAttribute(Permission = "GroupCreate")]
        [HttpPost]
        public ApiIntResponseDTO Import(ApiStringResponseDTO csvContents)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();
            return new ApiIntResponseDTO() { Value = _groupServices.ImportCsv(csvContents.Value,Convert.ToInt32(userId)) };
        }

        [CustomAuth(Permission = "GroupRead")]
        [HttpGet]
        public ApiBoolResponseDTO Export(string path)
        {
            _groupServices.ExportCsv(path);
            return new ApiBoolResponseDTO() { Value = true };
        }

        [HttpGet]
        [CustomAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveGroupMember(int id, int computerId)
        {
            return new ApiBoolResponseDTO() {Value = _groupServices.DeleteMembership(computerId, id)};
        }

        [HttpDelete]
        [CustomAuth(Permission = "GroupUpdate")]
        public ApiBoolResponseDTO RemoveMunkiTemplates(int id)
        {
            return new ApiBoolResponseDTO() {Value = _groupServices.DeleteMunkiTemplates(id)};
        }

        [Authorize]
        public ApiBoolResponseDTO ReCalcSmart()
        {
            _groupServices.UpdateAllSmartGroupsMembers();
            return new ApiBoolResponseDTO() {Value = true};
        }

        [CustomAuth(Permission = "GroupRead")]
        public IEnumerable<GroupMunkiEntity> GetMunkiTemplates(int id)
        {
            return _groupServices.GetGroupMunkiTemplates(id);
        }

        [HttpGet]
        [CustomAuth(Permission = "GroupRead")]
        public GroupPropertyEntity GetGroupProperties(int id)
        {
            var result = _groupServices.GetGroupProperty(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GroupRead")]
        public ApiStringResponseDTO GetMemberCount(int id)
        {
            return new ApiStringResponseDTO() {Value = _groupServices.GetGroupMemberCount(id)};
        }
       

        [CustomAuth(Permission = "GroupCreate")]
        public ActionResultDTO Post(GroupEntity group)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            var result = _groupServices.AddGroup(group, Convert.ToInt32(userId));
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GroupUpdate")]
        public ActionResultDTO Put(int id, GroupEntity group)
        {
            group.Id = id;
            var result = _groupServices.UpdateGroup(group);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GroupDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _groupServices.DeleteGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [HttpGet]
        [CustomAuth(Permission = "SmartUpdate")]
        public ActionResultDTO UpdateSmartMembership(int id)
        {
            return _groupServices.UpdateSmartMembership(id);
        }

        [HttpGet]
        [CustomAuth(Permission = "ImageTaskDeployGroup")]
        public ApiIntResponseDTO StartGroupUnicast(int id)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiIntResponseDTO()
            {
                Value = _groupServices.StartGroupUnicast(id, Convert.ToInt32(userId))
            };

        }

        [CustomAuth(Permission = "GroupRead")]
        public ApiStringResponseDTO GetEffectiveManifest(int id)
        {
            var effectiveManifest = new EffectiveMunkiTemplate().Group(id);
            return new ApiStringResponseDTO() { Value = Encoding.UTF8.GetString(effectiveManifest.ToArray()) };
        }

        [CustomAuth(Permission = "ImageTaskMulticast")]
        [HttpGet]
        public ApiStringResponseDTO StartMulticast(int id)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == "user_id")
                             .Select(c => c.Value).SingleOrDefault();

            return new ApiStringResponseDTO() { Value = new Multicast(id, Convert.ToInt32(userId)).Create() };
        }   

        [CustomAuth(Permission = "GroupRead")]
        public IEnumerable<ComputerEntity> GetGroupMembers(int id, string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _groupServices.GetGroupMembers(id)
                : _groupServices.GetGroupMembers(id, searchstring);

        }

        [CustomAuth(Permission = "GroupRead")]
        public GroupBootMenuEntity GetCustomBootMenu(int id)
        {
            var result = _groupServices.GetGroupBootMenu(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}