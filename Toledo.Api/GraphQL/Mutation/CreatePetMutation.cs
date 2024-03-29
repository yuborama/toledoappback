using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using Toledo.Core.Enumerations;

namespace Toledo.Api.GraphQL.Mutation;

public record PetInput(
    string? Name,
    EnumGender? Gender,
    string? Breed,
    EnumPetSize? Size,
    DateTime? DateOfBirth,
    bool? Sterilized,
    string? LocationOfSterilization,
    string? Address,
    string? Ubication,
    double? Longitude,
    double? Latitude,
    string? Zone,
    string? Color,
    string? Notes,
    string? PetType,
    string? Vaccine,
    string? VaccinePhoto,
    Guid? UserId,
    List<string> Images
);

public record PetPayload(Pet pet);

[ExtendObjectType(OperationTypeNames.Mutation)]
public class CreatePetMutation
{
    [Authorize]
    public async Task<PetPayload> CreatePet(
        PetInput input,
        ClaimsPrincipal claimsPrincipal,
        ToledoContext context
    )
    {

        var pet = new Pet
        {
            Name = input.Name ?? "",
            Gender = input.Gender ?? EnumGender.OTHER,
            Breed = input.Breed ?? "",
            Size = input.Size ?? EnumPetSize.MEDIUM,
            DateOfBirth = input.DateOfBirth ?? DateTime.UtcNow,
            Sterilized = input.Sterilized ?? false,
            LocationOfSterilization = input.LocationOfSterilization,
            Address = input.Address ?? "",
            Ubication = input.Ubication ?? "",
            Longitude = input.Longitude ?? 0,
            Latitude = input.Latitude ?? 0,
            Zone = input.Zone ?? "",
            Notes = input.Notes ?? "",
            PetType = input.PetType ?? "",
            Vaccine = input.Vaccine ?? "",
            VaccinePhoto = input.VaccinePhoto ?? "",
            UserId = input.UserId,
            Color = input.Color ?? "",
            Images = input.Images
        };

        context.Pets.Add(pet);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new GraphQLException(e.Message);
        }

        return new PetPayload(pet);
    }
}
