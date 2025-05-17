﻿namespace Identity.Api;

public class TokenGenerationRequest
{
    public Guid UserId { get; init; }
    
    public required string Email { get; init; }

    public Dictionary<string, object> CustomClaims { get; init; }
}
