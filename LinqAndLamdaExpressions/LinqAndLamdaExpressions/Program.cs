namespace LinqAndLamdaExpressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<User> allUsers = ReadUsers("users.json");
            List<Post> allPosts = ReadPosts("posts.json");

            #region Demo

            // 1 - find all users having email ending with ".net".
            // Problema1(allUsers);

            #endregion

            // 2 - find all posts for users having email ending with ".net".
            //Problema2(allUsers, allPosts);

            // 3 - print number of posts for each user.
            //Problema3(allUsers, allPosts);

            // 4 - find all users that have lat and long negative.
            //Problema4(allUsers);


            // 5 - find the post with longest body.
            //Problema5(allPosts);


            // 6 - print the name of the employee that have post with longest body.

            //Problema6(allUsers, allPosts);


            // 7 - select all addresses in a new List<Address>. print the list.
            //Problema7(allUsers);

            // 8 - print the user with min lat

            //Problema8(allUsers);

            // 9 - print the user with max long

            //Problema9(allUsers);


            // 10 - create a new class: public class UserPosts { public User User {get; set}; public List<Post> Posts {get; set} }
            //    - create a new list: List<UserPosts>
            //    - insert in this list each user with his posts only


            //Problema10(allUsers, allPosts);


            // 11 - order users by zip code

            //Problema(allUsers);


            // 12 - order users by number of posts
            //Problema12(allUsers, allPosts);

        }

        private static void Problema10(List<User> allUsers, List<Post> allPosts)
        {
            List<UserPosts> groupedPosts = (from post in allPosts
                                            join user in allUsers on post.UserId equals user.Id
                                            group post by post.UserId into gr
                                            select new UserPosts
                                            {
                                                Posts = gr.Distinct().ToList(),
                                                User = allUsers.First(p => p.Id == gr.Key)
                                            }).ToList();
        }

        private static void Problema12(List<User> allUsers, List<Post> allPosts)
        {
            var groupedPosts = from post in allPosts
                               join user in allUsers on post.UserId equals user.Id
                               group post by post.UserId into gr
                               orderby gr.Count() descending
                               select new { key = allUsers.First(p => p.Id == gr.Key).Name, cnt = gr.Count() };


            foreach (var item in groupedPosts)
            {
                Console.WriteLine($"{item.key} {item.cnt}");
            }
        }

        private static void Problema(List<User> allUsers)
        {
            IEnumerable<User> result = from user in allUsers
                                       orderby user.Address.Zipcode
                                       select user;
            foreach (var user in result)
            {
                Console.WriteLine(user.Address.Zipcode);
            }
        }

        public class UserPosts
        {
            public User User { get; set; } 
            public List<Post> Posts { get; set; }
        }

        private static void Problema9(List<User> allUsers)
        {
            double maxLong = allUsers.Max(u => u.Address.Geo.Lng);
            User user9 = allUsers.Where(u => u.Address.Geo.Lng == maxLong).First();
            Console.WriteLine(user9.Name);
        }

        private static void Problema8(List<User> allUsers)
        {
            double minLat = allUsers.Min(u => u.Address.Geo.Lat);
            User user8 = allUsers.Where(u => u.Address.Geo.Lat == minLat).First();
            Console.WriteLine(user8.Name);
        }

        private static void Problema7(List<User> allUsers)
        {
            List<Address> addressList = (from user in allUsers
                                         select user.Address).ToList();

            foreach (var address in addressList)
            {
                Console.WriteLine($"{address.Geo.Lat} {address.Geo.Lng}");
            }
        }

        private static void Problema6(List<User> allUsers, List<Post> allPosts)
        {
            int maxPostLenght = allPosts.Max(p => p.Body.Length);

            Post maxPost = allPosts.Where(p => p.Body.Length == maxPostLenght).First();

            User userP = allUsers.First(p => p.Id == maxPost.UserId);
            Console.WriteLine(userP.Name);
        }

        private static void Problema5(List<Post> allPosts)
        {
            IEnumerable<Post> orderedPosts = from post in allPosts
                                             orderby post.Body.Length descending
                                             select post;
            Console.WriteLine(orderedPosts.First().Id.ToString());
        }

        private static void Problema4(List<User> allUsers)
        {
            var result = from user in allUsers
                         where user.Address.Geo.Lat < 0 && user.Address.Geo.Lng < 0
                         select user;

            foreach (var item in result)
            {
                Console.WriteLine(item.Name);
            }
        }

        private static void Problema3(List<User> allUsers, List<Post> allPosts)
        {
            var p3 = from post in allPosts
                     group post by post.UserId;

            foreach (var item in p3)
            {
                var userName = allUsers.First(p => p.Id == item.Key).Name;

                Console.WriteLine($"{userName} {item.Count()}");
            }
        }

        private static void Problema2(List<User> allUsers, List<Post> allPosts)
        {
            IEnumerable<int> usersIdsWithDotNetMails = from user in allUsers
                                                       where user.Email.EndsWith(".net")
                                                       select user.Id;

            IEnumerable<Post> posts = from post in allPosts
                                      where usersIdsWithDotNetMails.Contains(post.UserId)
                                      select post;

            foreach (var post in posts)
            {
                Console.WriteLine(post.Id + " " + "user: " + post.UserId);
            }
        }

        private static void Problema1(List<User> allUsers)
        {
            var users1 = from user in allUsers
                         where user.Email.EndsWith(".net")
                         select user;

            var users11 = allUsers.Where(user => user.Email.EndsWith(".net"));

            IEnumerable<string> userNames = from user in allUsers
                                            select user.Name;

            var userNames2 = allUsers.Select(user => user.Name);

            foreach (var value in userNames2)
            {
                Console.WriteLine(value);
            }

            IEnumerable<Company> allCompanies = from user in allUsers
                                                select user.Company;

            var users2 = from user in allUsers
                         orderby user.Email
                         select user;

            var netUser = allUsers.First(user => user.Email.Contains(".net"));
            Console.WriteLine(netUser.Username);
        }


        private static List<Post> ReadPosts(string file)
        {
            return ReadData.ReadFrom<Post>(file);
        }

        private static List<User> ReadUsers(string file)
        {
            return ReadData.ReadFrom<User>(file);
        }
    }

}

