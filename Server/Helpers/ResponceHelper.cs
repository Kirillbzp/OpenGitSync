using Duende.IdentityServer.ResponseHandling;
using Microsoft.AspNetCore.Identity;
using OpenGitSync.Shared.DataTransferObjects;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenGitSync.Server.Helpers
{
    public static class ResponceHelper
    {
        public static ResponceResultDto ToError(this IdentityResult result)
        {
            return new ResponceResultDto() { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
        }
    }
}
