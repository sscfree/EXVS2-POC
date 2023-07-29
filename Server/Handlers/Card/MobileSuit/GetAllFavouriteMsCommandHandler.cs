﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using nue.protocol.exvs;
using Server.Persistence;
using WebUI.Shared.Dto.Common;
using WebUI.Shared.Dto.Enum;
using WebUI.Shared.Exception;

namespace Server.Handlers.Card.MobileSuit;

public record GetAllFavouriteMsCommand(String accessCode, String chipId) : IRequest<List<FavouriteMs>>;

public class GetAllFavouriteMsCommandHandler : IRequestHandler<GetAllFavouriteMsCommand, List<FavouriteMs>>
{
    private readonly ServerDbContext context;
    
    public GetAllFavouriteMsCommandHandler(ServerDbContext context)
    {
        this.context = context;
    }
    
    public Task<List<FavouriteMs>> Handle(GetAllFavouriteMsCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .Include(x => x.UserDomain)
            .FirstOrDefault(x => x.AccessCode == request.accessCode && x.ChipId == request.chipId);

        if (cardProfile is null)
        {
            throw new NullReferenceException("Card Profile is invalid");
        }
        
        var user = JsonConvert.DeserializeObject<Response.PreLoadCard.MobileUserGroup>(cardProfile.UserDomain.UserJson);
        
        if (user is null)
        {
            throw new InvalidCardDataException("Card Data is invalid");
        }

        var favouriteMsList = user.FavoriteMobileSuits
            .Select(favouriteMs => new FavouriteMs
            {
                MsId = favouriteMs.MstMobileSuitId,
                GaugeDesignId = favouriteMs.GaugeDesignId,
                BgmPlayingMethod = (BgmPlayingMethod) favouriteMs.BgmPlayMethod,
                BgmList = favouriteMs.BgmSettings,
                BattleNaviId = favouriteMs.BattleNavId,
                BurstType = (BurstType) favouriteMs.BurstType
            })
            .ToList();
        
        return Task.FromResult(favouriteMsList);
    }
}