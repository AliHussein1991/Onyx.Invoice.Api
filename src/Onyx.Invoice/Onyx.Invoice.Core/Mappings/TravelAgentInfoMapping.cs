using Onyx.Invoice.Core.Entities;
using Onyx.Invoice.Core.Models.Dtos;

namespace Onyx.Invoice.Core.Mappings
{
    public static class TravelAgentInfoMapping
    {
        public static TravelAgentInfoDto ToDto(this TravelAgentInfo entity)
        {
            return new TravelAgentInfoDto
            {
                TravelAgent = entity.TravelAgent,
                TotalNumberOfNights = entity.TotalNumberOfNights
            };
        }

        public static TravelAgentInfo ToEntity(this TravelAgentInfoDto dto)
        {
            return new TravelAgentInfo
            {
                TravelAgent = dto.TravelAgent,
                TotalNumberOfNights = dto.TotalNumberOfNights
            };
        }
    }
}
