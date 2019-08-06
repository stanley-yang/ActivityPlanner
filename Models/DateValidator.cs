    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    namespace ActivityCenter.Models
    {
        public class RestrictedDate : ValidationAttribute
        {
            public override bool IsValid(object date) 
            {
                DateTime testdate = (DateTime)date;
                return testdate > DateTime.Now;
            }
        }
    }