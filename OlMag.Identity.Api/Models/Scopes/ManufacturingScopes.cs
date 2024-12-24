﻿using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OlMag.Identity.Api.Models.Scopes
{
    public static class ManufacturingScopes
    {
        public static readonly OpenIddictScopeDescriptor ApiScope = new()
        {
            Name = "olmag.api",
            Resources =
            {
                "OlMag.Api"
            },
            Description = "Access to protected OlMag API resources"
        };

        public static readonly OpenIddictApplicationDescriptor[] Applications =
        [
            new OpenIddictApplicationDescriptor
            {
                ClientId = "OlMag.Api",
                ClientSecret = "1447597C-38EB-4424-928F-9DA8F6909525",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials
                }
            },
        ];
    }
}