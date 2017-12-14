using System.Collections.Generic;
using System.Data.Entity;

namespace WebAPI.Models
{
    
    public class TogglerDBInitializer : DropCreateDatabaseAlways<WebAPIContext>
    {
        protected override void Seed(WebAPIContext context)
        {
            IList<User> users = new List<User>();

            users.Add(new User() { Name = "admin", UserType = "admin" });
            users.Add(new User() { Name = "user1", UserType = "normal" });
            users.Add(new User() { Name = "user2", UserType = "normal" });
            users.Add(new User() { Name = "ABC", UserType = "premium" });

            foreach (User usr in users)
                context.Users.Add(usr);

            IList<Feature> features = new List<Feature>();

            features.Add(new BlueButton("azul"));
            features.Add(new GreenButton("verde"));
            features.Add(new RedButton("vermelho"));

            foreach (Feature ftr in features)
                context.Features.Add(ftr);

            base.Seed(context);
        }
    }

}