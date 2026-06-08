using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public static class ValidationPatterns
    {
        /// <summary>
        /// رقم موبايل مصري (11 رقم يبدأ بـ 010 | 011 | 012 | 015)
        /// </summary>
        public const string EgyptianPhone = @"^(010|011|012|015)\d{8}$";

        /// <summary>
        /// بريد إلكتروني
        /// </summary>
        public const string Email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    }
}
