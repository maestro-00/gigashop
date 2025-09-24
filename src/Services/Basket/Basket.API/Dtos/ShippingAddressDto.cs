namespace Basket.API.Dtos;
public record ShippingAddressDto(string FirstName,string LastName, string EmailAddress,
   string AddressLine, string Country, string State, string ZipCode);