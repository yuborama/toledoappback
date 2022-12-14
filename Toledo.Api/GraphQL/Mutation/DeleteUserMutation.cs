using HotChocolate.AspNetCore.Authorization;
using System.Security.Claims;
using Toledo.Core.DTO;
using Toledo.Core.Enumerations;

namespace Toledo.Api.GraphQL.Mutation;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class DeleteUserMutation
{
    [Authorize]
    public async Task<DeletedId> DeleteUser(
        Guid id,
        ClaimsPrincipal claimsPrincipal,
        ToledoContext context
    )
    {
        var user = context.Users.FirstOrDefault(x => x.Id == id);
        if (user is null)
            throw new GraphQLException(Errors.Exceptions.ID_NOT_FOUND);

        try
        {
            context.Pets.RemoveRange(context.Pets.Where(x=>x.UserId == user.Id));
            await context.SaveChangesAsync();

            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new GraphQLException(e.Message);
        }

        return new DeletedId(user.Id);
    }
}
