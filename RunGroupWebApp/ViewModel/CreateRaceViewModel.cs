﻿using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.ViewModel
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public string AppUserId{ get; set;}
        public AppUser? AppUser { get; set; }

    }
}
