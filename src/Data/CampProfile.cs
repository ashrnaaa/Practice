using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Talk, TalksModel>();
            this.CreateMap<Speaker, SpeakerModel>();
            this.CreateMap<CampsModel, Camp>();
            this.CreateMap<Camp, CampsModel>()
                .ForMember(C => C.Venue, m => m.MapFrom(o => o.Location.VenueName));
        }

    }
}
