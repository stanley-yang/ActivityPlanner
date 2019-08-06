    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace ActivityCenter.Models
    {
        public class User
        {
            // auto-implemented properties need to match the columns in your table
            // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
            [Key]
            public int UserId { get; set; }
            // MySQL VARCHAR and TEXT types can be represeted by a string
            
            [Required(ErrorMessage="{0} is required.")]
            [MinLength(2, ErrorMessage = "Name requires at least 2 characters.")]
            [Display(Name = "Name")]
            public string Name { get; set; }
            
            [Required(ErrorMessage="{0} is required.")]
            [Display(Name = "Email")]
            [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
            public string Email { get; set; }
            [Required(ErrorMessage="{0} is required.")]
            [DataType(DataType.Password)]
            // [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
            [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*\d)(?=.*[@#^$!%*?&])[A-Za-z\d@#^$!%*?&]{8,}$",ErrorMessage = "Password must contain at least 8 characters, a number, a letter and a special character")]
            [Display(Name = "Create a password")]
            public string Password { get; set; }
            
            public List<Rsvp> Rsvps {get; set;}
            public List<Activity> CreatedActivities {get; set;}

            public DateTime CreatedAt {get;set;}
            public DateTime UpdatedAt {get;set;}

            [NotMapped]
            [DataType(DataType.Password)]
            public string Confirm {get; set;}
        }
    }
    