﻿using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using PhotoCollage.Core.Entities;
using PhotoCollage.Persistence;
using PhotoCollage.Web.Helpers.Commands;

namespace PhotoCollage.Web.Libraries.Create;

internal sealed class CreateLibraryCommand : ICommand<int>
{
    internal required string Name { get; init; }
    internal string? Description { get; init; }
}

internal sealed class CreateLibraryCommandHandler : ICommandHandler<CreateLibraryCommand, int>
{
    private readonly ILogger<CreateLibraryCommandHandler> logger;
    private readonly IDbContextFactory<PhotoCollageContext> contextFactory;

    public CreateLibraryCommandHandler(
        ILogger<CreateLibraryCommandHandler> handlerLogger,
        IDbContextFactory<PhotoCollageContext> photoCollageContextFactory)
    {
        this.logger = handlerLogger;
        this.contextFactory = photoCollageContextFactory;
    }

    public async Task<Result<int>> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // todo: add validation
            using var context = this.contextFactory.CreateDbContext();

            var library = Library.Create(request.Name, request.Description);
            context.Libraries.Add(library);

            _ = await context.SaveChangesAsync();
            return Result.Created(library.Id.Value);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error creating library");
            return Result.Error(ex.Message);
        }
    }
}
