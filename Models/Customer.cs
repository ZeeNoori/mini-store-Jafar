using System;
using System.ComponentModel.DataAnnotations;

namespace MiniStoreManagement.Models
{
    public class Customer
    {

        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";


    }
}



