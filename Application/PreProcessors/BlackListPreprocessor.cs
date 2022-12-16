using Application.PreProcessors.Abstractions;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkNet.Abstractions;
using VkNet.Model;

namespace Application.PreProcessors;

public class BlackListPreprocessor : ICommandPreprocessor
{
    private readonly MainContext _mainContext;
    private readonly IVkApi _vkApi;

    public BlackListPreprocessor(MainContext mainContext, IVkApi vkApi)
    {
        _mainContext = mainContext;
        _vkApi = vkApi;
    }

    public async Task<bool> ProcessAsync(Message msg, CancellationToken cancellationToken = default)
    {
        var userId = msg?.Action?.MemberId ?? msg?.FromId;
        var peerId = msg?.PeerId;

        if (userId == null || peerId == null) return true;

        var isBlockedUser = await _mainContext.BlackList
            .AnyAsync(x => x.ChatVkId == peerId.Value && x.UserVkId == userId.Value, cancellationToken);

        if (!isBlockedUser) return true;


        try
        {
            _vkApi.Messages.RemoveChatUser((ulong)msg.PeerId.Value - 2000000000, userId);

            _mainContext.ChatRoles.Remove(await _mainContext.ChatRoles.FirstOrDefaultAsync(x =>
                x.UserVkID == userId.Value &&
                x.ChatVkID == peerId.Value, cancellationToken));
            await _mainContext.SaveChangesAsync(cancellationToken);

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}