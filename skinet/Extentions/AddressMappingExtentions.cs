using API.DTOs;
using core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Extentions
{
    public static class AddressMappingExtentions
    {
        public static AddressDto? ToDto(this Address address)
        {
            if (address == null) return null;
            return new AddressDto
                {
                    Line1 = address.Line1,
                    Line2 = address.Line2,
                    City = address.City,
                    PostalCode = address.PostalCode,
                    Country = address.Country,
                    State = address.State
                };
        }
        public static Address ToEntity(this AddressDto addressDto)
        {
            return addressDto == null
                ? throw new ArgumentNullException(nameof(addressDto))
                : new Address
                {
                    Line1 = addressDto.Line1,
                    Line2 = addressDto.Line2,
                    City = addressDto.City,
                    PostalCode = addressDto.PostalCode,
                    Country = addressDto.Country,
                    State = addressDto.State
                };
        }
        public static void UpdateFromDto(this Address address, AddressDto addressDto)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }
            if (addressDto == null)
            {
                throw new ArgumentNullException(nameof(addressDto));
            }
            address.Line1 = addressDto.Line1;
            address.Line2 = addressDto.Line2;
            address.City = addressDto.City;
            address.PostalCode = addressDto.PostalCode;
            address.Country = addressDto.Country;
            address.State = addressDto.State;

        }
    }
}
