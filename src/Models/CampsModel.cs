using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodeCamp.Data;

namespace CoreCodeCamp.Models
{
    public class CampsModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string Moniker { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        [Required]
        [Range(1, 100)]
        public int Length { get; set; } = 1;

        [Required]
        public string Venue { get; set; }
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }

        public string LocationCountry { get; set; }

        public ICollection<TalksModel> Talks { get; set; }
    }
}
