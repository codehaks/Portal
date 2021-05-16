using System.ComponentModel.DataAnnotations;

namespace Portal.Persistence.Models
{
    public enum GenderType
    {
        [Display(Name = "-")]
        None = 0,    
        Male = 1,
        Female = 2
    }
}