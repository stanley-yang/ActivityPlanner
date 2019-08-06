    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    namespace ActivityCenter.Models
    {
        public class Activity
        {
            // auto-implemented properties need to match the columns in your table
            // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
            [Key]
            public int ActivityId { get; set; }
            // MySQL VARCHAR and TEXT types can be represeted by a string

            [Required(ErrorMessage="{0} is required.")]
            [Display(Name = "Activity")]
            public string Event { get; set; }


             
            // [MinLength(2, ErrorMessage = "Person One requires at least 2 characters.")]
            [Display(Name = "hoursmins")]
            public string hoursmins { get; set; }
            
            [Required(ErrorMessage="{0} is required.")]
            // [DataType(DataType.Time)]
            // [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            // [MinLength(2, ErrorMessage = "Person Two requires at least 2 characters.")]
            [Display(Name = "Duration")]
            public int Duration { get; set; }

            [Required(ErrorMessage="{0} are required.")]
            [Display(Name = "Date")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            // [RestrictedDate(ErrorMessage="Date must be in the future")]
            public DateTime Datetime { get; set; }

            [Required(ErrorMessage="{0} are required.")]
            [Display(Name = "Time")]
            [DataType(DataType.Time)]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            // [RestrictedDate(ErrorMessage="Time must be in the future")]
            public TimeSpan time { get; set; }

            [Required(ErrorMessage="{0} are required.")]
            [Display(Name = "DateTime")]
            [DataType(DataType.DateTime)]
            [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
            // [RestrictedDate(ErrorMessage="Time must be in the future")]
            public TimeSpan realdatetime { get; set; }


            [Required(ErrorMessage="{0} is required.")]
            public string Description { get; set; }
            // The MySQL DATETIME type can be represented by a DateTime
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
            public int UserId {get; set;}
            public User Planner {get; set;}
            public List<Rsvp> Rsvps {get; set;}

        }


        
    }
    