using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp.Models
{
    public class CustomerReferralParam
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string AccountPreferences { get; set; }
        public bool TermsAndCondition { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ReferrerId { get; set; }
        public string RefereeId { get; set; }
        public string ReferrerEmail { get; set; }
        public string RefereeEmail { get; set; }
    }
}
