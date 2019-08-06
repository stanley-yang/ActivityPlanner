    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    namespace ActivityCenter.Models
    {
        public class Rsvp
        {

            [Key]
            public int RsvpId {get; set;}
            public int ActivityId { get; set; }
            public int UserId {get; set;}
            public User user {get; set;}
            public Activity activity {get; set;}
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
            
        }
    }
    