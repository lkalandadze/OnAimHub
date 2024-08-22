﻿using OnAim.Admin.Infrasturcture.Entities.Abstract;

namespace OnAim.Admin.Infrasturcture.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        //public bool IsBanned { get; set; }

        //who created/updated/deleted this user
        public int? UserId { get; set; }
    }
}
