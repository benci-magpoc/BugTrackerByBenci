﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BugTrackerByBenci.Models
{

    public class Invite
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Invite")]
        public string? Name { get; set; }
    }
}