﻿using System;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class UserLockoutServices
    {
          private readonly UnitOfWork _uow;

        public UserLockoutServices()
        {
            _uow = new UnitOfWork();
        }

        public  void ProcessBadLogin(int userId)
        {
           
                var userLockout = Get(userId);
                if (userLockout == null)
                {
                    _uow.UserLockoutRepository.Insert(new UserLockoutEntity { UserId = userId, BadLoginCount = 1 });
                   
                }
                else
                {
                    userLockout.BadLoginCount += 1;
                    if (userLockout.BadLoginCount == 5)
                    {
                        userLockout.LockedUntil = DateTime.UtcNow.AddMinutes(15);
                        new UserServices().SendLockOutEmail(userId);
                    }

                    _uow.UserLockoutRepository.Update(userLockout, userLockout.Id);
                }
                _uow.Save();
            
        }

        public  bool AccountIsLocked(int userId)
        {
            var userLockout = Get(userId);

            if (userLockout == null) return false;

            if (userLockout.LockedUntil == null) return false;

            if (DateTime.UtcNow > userLockout.LockedUntil)
            {
                //Lockout has expired reset lock
                DeleteUserLockouts(userId);
                return false;
            }
            else
            {
                return true;
            }


        }

        public  bool DeleteUserLockouts(int userId)
        {
            
                _uow.UserLockoutRepository.DeleteRange(x => x.UserId == userId);
                _uow.Save();
                return true;
            
        }

        public  UserLockoutEntity Get(int userId)
        {
           
                return _uow.UserLockoutRepository.GetFirstOrDefault(x => x.UserId == userId);
            
        }
    }
}