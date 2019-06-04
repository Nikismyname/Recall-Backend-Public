namespace Recall.Services.Options
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Recall.Data;
    using Recall.Data.Models.Options;
    using Recall.Services.Exceptions;
    using Recall.Services.Models.OptionsModels;
    using System.Linq;

    public class OptionsService : IOptionsService
    {
        private readonly RecallDbContext context;

        public OptionsService(RecallDbContext context)
        {
            this.context = context;
        }

        public void CreateOptionsForUser(int userId)
        {
            var user = this.context.Users.SingleOrDefault(x => x.Id == userId);
            if(user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            var options = new UserOptions();

            user.UserOptions = options;
            context.SaveChanges();
        }

        public AllOptions GetAllOptions (int userId)
        {
            var user = this.context.Users
                .Include(x=> x.UserOptions)
                .SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ServiceException("User Not Found!");
            }

            if(user.UserOptions == null)
            {
                this.CreateOptionsForUser(userId);
            }

            var options = Mapper.Instance.Map<AllOptions>(user.UserOptions);

            return options;
        }

        public void SaveOptions(AllOptions incOptions, int userId)
        {
            var user = context.Users
                .Include(x=> x.UserOptions)
                .SingleOrDefault(x => x.Id == userId);

            if(user == null)
            {
                throw new ServiceException("User Not Foud!");
            }

            var options = user.UserOptions;

            options.Theme = incOptions.Theme;

            this.context.SaveChanges();
        }
    }
}
