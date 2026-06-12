using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.DAL.Models.Enums
{
    public enum BloodType
    {
        [Display(Name = "A+")]
        APositive,

        [Display(Name = "A-")]
        ANegative,

        [Display(Name = "B+")]
        BPositive,

        [Display(Name = "B-")]
        BNegative,

        [Display(Name = "O+")]
        OPositive,

        [Display(Name = "O-")]
        ONegative,

        [Display(Name = "AB+")]
        ABPositive,

        [Display(Name = "AB-")]
        ABNegative
    }
    }
