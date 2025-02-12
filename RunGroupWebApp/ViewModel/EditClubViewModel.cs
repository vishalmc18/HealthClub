﻿using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModel
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public string Image { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public int AdressId { get; set; }
        
        public Address Address { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
