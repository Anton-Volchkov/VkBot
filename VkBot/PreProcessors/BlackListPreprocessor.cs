using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkBot.Domain;
using VkBot.PreProcessors.Abstractions;
using VkNet.Abstractions;
using VkNet.Model;

namespace VkBot.PreProcessors
{
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
            long? userId = msg.Action.MemberId ?? msg.FromId;
            long? peerId = msg.PeerId;

           

            var isBlockedUser = await _mainContext.BlackList
                                                  .AnyAsync(x => x.ChatVkId == peerId.Value && x.UserVkId == userId.Value, cancellationToken);

            if (!isBlockedUser)
            {
                return true;
            }


            try
            {
                _vkApi.Messages.RemoveChatUser((ulong)msg.PeerId.Value - 2000000000, userId);

                _mainContext.ChatRoles.Remove(await _mainContext.ChatRoles.FirstOrDefaultAsync(x => x.UserVkID == userId.Value &&
                                                  x.ChatVkID == peerId.Value, cancellationToken: cancellationToken));
                await _mainContext.SaveChangesAsync(cancellationToken);

                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}