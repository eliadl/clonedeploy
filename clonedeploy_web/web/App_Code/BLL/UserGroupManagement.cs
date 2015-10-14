﻿using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class UserGroupManagement
    {
        public static bool AddUserGroupManagements(List<Models.UserGroupManagement> listOfGroups)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                foreach (var group in listOfGroups)
                    uow.UserGroupManagementRepository.Insert(group);

                return uow.Save();
            }
        }

        public static bool DeleteUserGroupManagements(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.UserGroupManagementRepository.DeleteRange(x => x.UserId == userId);
                return uow.Save();
            }
        }

        public static List<Models.UserGroupManagement> Get(int userId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.UserGroupManagementRepository.Get(x => x.UserId == userId);
            }
        }
    }
}