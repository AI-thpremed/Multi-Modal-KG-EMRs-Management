using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jSocial.Models;
using System.Threading.Tasks;

namespace Neo4jSocial.DAO
{
    public class ProfileDAO:BaseDAO
    {
        //public Profile GetProfile(string user)
        //{

        //    using (var session = Driver.Session())
        //    {
        //        var profile = session.Run("MATCH (n:Profile {User:'" + user + "'}) RETURN n").FirstOrDefault();
        //        Profile p = new Profile();
        //        if (profile!=null)
        //        {
        //            var node = profile["n"].As<INode>();
        //            p.Avatar = node["Avatar"].As<string>();
        //            p.Birthday = node["Birthday"].As<LocalDate>();
        //            p.City = node["City"].As<string>();
        //            p.Company = node["Company"].As<string>();
        //            p.Email = node["Email"].As<string>();
        //            p.FirstName = node["FirstName"].As<string>();
        //            p.Gender = node["Gender"].As<string>();
        //            p.Interest = node["Interest"].As<string>();
        //            p.Job = node["Job"].As<string>();
        //            p.LastName = node["LastName"].As<string>();
        //            p.School = node["School"].As<string>();
        //            p.User = node["User"].As<string>();

        //        }
        //        return p;

        //    }
        //}

        //public bool Edit(Profile p)
        //{
        //    string avt = "";
        //    if (p.Avatar!="" && p.Avatar!=null)
        //    {
        //        avt = "', p.Avatar = '" + p.Avatar;
        //    }
        //    try
        //    {
        //        using (var session = Driver.Session())
        //        {
        //            session.Run("MATCH (p:Profile {User:'" + p.User + "'}) SET  p.Birthday = date('" + p.Birthday + "'), p.City = '" + p.City + "', p.LastName = '" + p.LastName + "', p.FirstName = '" + p.FirstName + "', p.Interest = '" + p.Interest + "', p.Email = '" + p.Email + "', p.Gender = '" + p.Gender + "', p.Job = '" + p.Job + "', p.Company = '" + p.Company + avt + "', p.School = '" + p.School + "' ");
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        //[Obsolete]
        public  async Task<bool> Add(string p)
        {

            var query = "CREATE (p:Profile {User:'" + p + "'})";


      

            var session = Driver.AsyncSession();




            var results = await session.ExecuteWriteAsync(async tx =>
            {
                var cursor = await tx.RunAsync(query);
                var fetched = await cursor.FetchAsync();

                //var output = new object;
                while (fetched)
                {
                    //var maa = new MovieTitleAndActors
                    //{
                    //    Title = cursor.Current["title"].As<string>(),
                    //    Actors = cursor.Current["actors"].As<IEnumerable<string>>()
                    //};
                    //output.Add(maa);

                    fetched = await cursor.FetchAsync();
                }

                return 1;
            });


            return true;


         
        }






        //public async Task<int?> EmployEveryoneInCompany(string companyName)
        //{
        //    int? jobsFilled = null;

        //    var session = Driver.AsyncSession();

        //    try
        //    {
        //        var names = await session.ExecuteReadAsync(async tx =>
        //        {
        //            var cursor = await tx.RunAsync("MATCH (a:Person) RETURN a.name AS name");
        //            var people = await cursor.ToListAsync();
        //            return people.Select(person => {
        //                return Neo4j.Driver.ValueExtensions.As<string>(person["name"]);
        //            });
        //        });

        //        jobsFilled = await session.ExecuteWriteAsync(tx =>
        //        {
        //            return Task.WhenAll(names.Select(async personName =>
        //            {
        //                var cursor = await tx.RunAsync("MATCH (emp:Person {name: $person_name}) " +
        //                                           "MERGE (com:Company {name: $company_name}) " +
        //                                           "MERGE (emp)-[:WORKS_FOR]->(com)",
        //                new
        //                {
        //                    person_name = personName,
        //                    company_name = companyName
        //                });

        //                var summary = await cursor.ConsumeAsync();

        //                return summary.Counters.RelationshipsCreated;
        //            }))
        //                .ContinueWith(t => t.Result.Sum());

        //        });
        //    }
        //    finally
        //    {
        //        await session.CloseAsync();
        //    }

        //    return jobsFilled;
        //}



        //try
        //{
        //    using (var session = Driver.Session())
        //    {
        //        //session.Run("CREATE (p:Profile {User:'" + p.User + "'}) SET  p.Birthday = date('" + p.Birthday + "'), p.City = '" + p.City + "', p.LastName = '" + p.LastName + "', p.FirstName = '" + p.FirstName + "', p.Interest = '" + p.Interest + "', p.Email = '" + p.Email + "', p.Gender = '" + p.Gender + "', p.Job = '" + p.Job + "', p.Company = '" + p.Company + "', p.Avatar = '" + p.Avatar + "', p.School = '" + p.School + "' ");

        //        session.Run("CREATE (p:Profile {User:'" + p.User + "'})");


        //    }
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
        //}
        //public bool Post(string user, string content, string avt, string displayName)
        //{

        //    try
        //    {
        //        using (var session = Driver.Session())
        //        {
        //            string idStt = session.WriteTransaction(tx => {
        //                var result = tx.Run("MATCH (a:Profile {User:'" + user + "'}) CREATE (a)-[:POST]->(s:Status { Content:'" + content + "', Time: localdatetime({ timezone: '+07:00'}), Avatar:'"+avt+"', DisplayName:'"+displayName+"', UserPost:'" + user + "'}) RETURN id(s)");
        //                return result.Single()[0].As<string>();
        //            });
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //}
        //public List<Status> GetAllPost(string user)
        //{
        //    List<Status> lstStatus = new List<Status>();

        //    using (var session = Driver.Session())
        //    {
        //        lstStatus = session.ReadTransaction(tx => {
        //            var result = tx.Run("MATCH (a:Profile {User:'" + user + "'})-[r:POST|SHARE]->(s:Status) RETURN s.Content,s.Time,s.UserPost,id(s) as idstt,id(r) as idrelation, s.Avatar, s.DisplayName, type(r) as typestt,r.Time as timeshare ORDER BY s.Time DESC");
        //            foreach (var recod in result)
        //            {
        //                Status stt = new Status();
        //                stt.Id = recod.Values["idstt"].As<int>();
        //                stt.RelationId = recod.Values["idrelation"].As<int>();
        //                stt.Content = recod.Values["s.Content"].As<string>();
        //                stt.UserPost = recod.Values["s.UserPost"].As<string>();
        //                stt.AvatarPost = recod.Values["s.Avatar"].As<string>();
        //                stt.DisplayName = recod.Values["s.DisplayName"].As<string>();
        //                stt.TypeStt = recod.Values["typestt"].As<string>();
        //                stt.TimePost = recod.Values["s.Time"].As<LocalDateTime>();
        //                stt.TimeShare = recod.Values["timeshare"].As<LocalDateTime>();
        //                lstStatus.Add(stt);
        //            }
        //            return lstStatus;
        //        });
        //    }
        //    return lstStatus;
        //}
        //public string GetAvt(string user)
        //{
        //    string res = user.ToUpper()+"/";
        //    using (var session = Driver.Session())
        //    {
        //        var avt = session.Run("match (p:Profile {User:'"+user+"'}) return p.Avatar").FirstOrDefault().Values["p.Avatar"];
        //        if (avt!=null)
        //        {
        //            res += avt.As<string>();
        //        }
        //        return res;
        //    }
        //}
        //public void Share(int id, string user)
        //{
        //    using (var session = Driver.Session())
        //    {
        //        session.Run("MATCH (s:Status),(p:Profile {User:'" + user + "'}) WHERE id(s)=" + id + "  CREATE (p)-[:SHARE {Time:localdatetime({ timezone: '+07:00'})}]->(s)");
        //    }
        //}
        //public void DeleteShare(int id)
        //{
        //    using (var session = Driver.Session())
        //    {
        //        session.Run("MATCH ()-[r:SHARE]->() WHERE id(r)="+id+" DELETE r");
        //    }
        //}
        //public void DeleteSTT(int id)
        //{
        //    using (var session = Driver.Session())
        //    {
        //        session.Run("MATCH ()-[r:POST]->(s) WHERE id(s)="+id+" DETACH DELETE s");
        //    }
        //}

        //#region đếm số bài đăng/chia sẻ/bình luận
        //public int CountStatus(string user)
        //{
        //    using (var session = Driver.Session())
        //    {
        //        return session.Run("MATCH (p:Profile {User:'"+user+"'})-[:POST]->(s:Status) RETURN id(s)").ToList().Count();
        //    }
        //}
        //public int CountShare(string user)
        //{
        //    using (var session = Driver.Session())
        //    {
        //        return session.Run("MATCH (p:Profile {User:'" + user + "'})-[:SHARE]->(s:Status) RETURN id(s)").ToList().Count();
        //    }
        //}
        //public int CountCmt(string user)
        //{
        //    using (var session = Driver.Session())
        //    {
        //        return session.Run("MATCH (p:Profile {User:'" + user + "'})-[:POST]->(s:Status)-[:CMT]->(cmt) RETURN id(cmt)").ToList().Count();
        //    }
        //}
        //#endregion

        //#region Thông báo có bạn bè kết bạn
        //public List<Profile> Requests(string me)
        //{
        //    List<Profile> lst = new List<Profile>();
        //    using (var session = Driver.Session())
        //    {
        //        lst = session.ReadTransaction(tx => {
        //            var result = tx.Run("MATCH (p:Profile {User:'" + me+ "'})<-[:FRIEND]-(f:Profile) WHERE NOT (p:Profile {User:'" + me + "'})-[:FRIEND]->(f:Profile) RETURN f.User");
        //            foreach (var item in result)
        //            {
        //                string user = item.Values["f.User"].As<string>();
        //                Profile newP = new Profile();
        //                newP = GetProfile(user);
        //                lst.Add(newP);
        //            }
        //            return lst;
        //        });
        //    }
        //    return lst;
        //}
        //#endregion

        ////Get list friend
        //public List<Profile> GetFriend(string user)
        //{
        //    user = user.Replace(" ", "");

        //    List<Profile> lst = new List<Profile>();
        //    using (var session = Driver.Session())
        //    {
        //        lst = session.ReadTransaction(tx =>
        //        {
        //            var result = tx.Run("MATCH (m:Profile {User:'" + user + "'})-[:FRIEND]->(p:Profile),(m)<-[:FRIEND]-(p:Profile) RETURN p.Avatar, p.Birthday, p.City, p.Company, p.Email, p.FirstName, p.Gender, p.Interest, p.Job, p.LastName, p.School, p.User");
        //            foreach (var item in result)
        //            {
        //                Profile p = new Profile();
        //                p.Avatar = item.Values["p.Avatar"].As<string>();
        //                p.Birthday = item.Values["p.Birthday"].As<LocalDate>();
        //                p.City = item.Values["p.City"].As<string>();
        //                p.Company = item.Values["p.Company"].As<string>();
        //                p.Email = item.Values["p.Email"].As<string>();
        //                p.FirstName = item.Values["p.FirstName"].As<string>();
        //                p.Gender = item.Values["p.Gender"].As<string>();
        //                p.Interest = item.Values["p.Interest"].As<string>();
        //                p.Job = item.Values["p.Job"].As<string>();
        //                p.LastName = item.Values["p.LastName"].As<string>();
        //                p.School = item.Values["p.School"].As<string>();
        //                p.User = item.Values["p.User"].As<string>();
        //                lst.Add(p);
        //            }
        //            return lst;
        //        });
        //        return lst;
        //    }
        //}
        ////Count friend
        //public int CountFriend(string user)
        //{
        //    int c = 0;
        //    using (var session = Driver.Session())
        //    {
        //        c = session.Run("MATCH (m:Profile {User:'" + user + "'})-[:FRIEND]->(p:Profile),(m)<-[:FRIEND]-(p:Profile) RETURN count(p) as c").FirstOrDefault().Values["c"].As<int>();
        //    }
        //    return c;
        //}

        //#region Gợi ý kết bạn
        //public List<SuggestionFriend> SuggestionFriend(string user)
        //{
        //    List<SuggestionFriend> lst = new List<SuggestionFriend>();
        //    using (var session = Driver.Session())
        //    {
        //        lst = session.ReadTransaction(tx =>
        //        {
        //            var result = tx.Run("MATCH (m:Profile {User:'"+user+"'})-[:FRIEND]->(f:Profile)-[:FRIEND]->(g:Profile), (m)<-[:FRIEND]-(f)<-[:FRIEND]-(g) WHERE NOT (m)-[:FRIEND]-(g) RETURN g.User,g.FirstName,g.LastName,g.Avatar,g.Birthday,g.Gender,count(f) as num ORDER BY num DESC LIMIT 10");
        //            foreach (var item in result)
        //            {
        //                SuggestionFriend p = new SuggestionFriend();
        //                p.Avatar = item.Values["g.Avatar"].As<string>();
        //                p.Birthday = item.Values["g.Birthday"].As<LocalDate>();
        //                p.FirstName = item.Values["g.FirstName"].As<string>();
        //                p.Gender = item.Values["g.Gender"].As<string>();
        //                p.LastName = item.Values["g.LastName"].As<string>();
        //                p.User = item.Values["g.User"].As<string>();
        //                p.Num = item.Values["num"].As<int>();
        //                lst.Add(p);
        //            }
        //            return lst;
        //        });
        //        return lst;
        //    }
        //}

        //#endregion
    }
}