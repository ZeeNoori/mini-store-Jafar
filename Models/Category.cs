using System;
using System.ComponentModel.DataAnnotations;

namespace MiniStoreManagement.Models
{
    public class Category
    {

        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

    }
}
