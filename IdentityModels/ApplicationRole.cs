using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;

namespace Projectauth.IdentityModels;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{
}
