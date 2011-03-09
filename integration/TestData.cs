using System.Collections.Generic;
using System.Linq;
using CommunitySite.Core.Domain;

namespace CommunitySite.Integration
{
    public static class TestData
    {
        public static IQueryable<Member> Members
        {
            get
            {
                return new List<Member>
                           {
                            new Member
                                {
                                  FirstName  = "Travis",
                                  LastName = "Dietz",
                                  Email = "travis.dietz@adventuretechgroup.com",
                                  Username = "tdietz",
                                  Password = "p@ssw0rd"
                                },   
                                new Member
                                    {
                                        FirstName = "Brian",
                                        LastName = "Wigfield",
                                        Email = "brian.wigfield@adventuretechgroup.com",
                                        Username = "bwigfield",
                                        Password = "p@55w0rd"
                                    }
                           }.AsQueryable();
            }
        }
    }
}