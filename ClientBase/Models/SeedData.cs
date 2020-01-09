using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientBase.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();
            
            context.Database.Migrate();

            if (context.Founders.Any() || context.Companies.Any())
                return;

            var founders = CreateFounders();

            var individualsCount = 5;
            var companiesCount = 5;
            var companiesWithManyFounders = 2;

            var individuals = founders.Take(individualsCount)
                                      .Select(founder =>
                                      {
                                          var company = new Company
                                          {
                                              IsIndividual = true,
                                              CreationDate = founder.CreationDate,
                                              TaxpayerId = founder.TaxpayerId,
                                              Name = $"ИП {founder.FullName}"
                                          };

                                          company.CompanyFounders = new CompanyFounder[]
                                          {
                                             new CompanyFounder
                                             {
                                                 Founder = founder,
                                                 Company = company
                                             }
                                          };

                                          return company;
                                      })
                                      .ToArray();

            var companyIds = new long[] { 9636532799, 9377642012, 9363232123, 9807759547, 9786834517 };
            var names = new string[] { "Пятерочка", "Дикси", "ДоДо", "Лента", "Доминос" };

            var companies = founders.Skip(individualsCount)
                                    .Take(companiesCount)
                                    .Select((founder, index) =>
                                    {
                                        return new Company
                                        {
                                            TaxpayerId = companyIds[index],
                                            IsIndividual = false,
                                            Name = names[index],
                                            CreationDate = founder.CreationDate
                                        };
                                    })
                                    .ToArray();

            for (int i = 0; i < companiesCount; i++)
            {
                var companyFounders = new List<CompanyFounder>
                { 
                    new CompanyFounder
                    {
                        Founder = founders[individualsCount + i],
                        Company = companies[i]
                    },
                };

                if (i < companiesWithManyFounders)
                    companyFounders.Add(new CompanyFounder
                    {
                        Founder = founders[companiesCount + individualsCount + i],
                        Company = companies[i]
                    });

                companies[i].CompanyFounders = companyFounders;
            }

            context.Founders.AddRange(founders);
            context.Companies.AddRange(individuals);
            context.Companies.AddRange(companies);
            context.SaveChanges();
        }

        private static List<Founder> CreateFounders()
        {
            return new List<Founder>
            {
                new Founder
                {
                    TaxpayerId = 930857958271,
                    LastName = "Александров",
                    FirstName = "Ираклий",
                    Patronymic = "Дмитриевич",
                    CreationDate = new DateTime(2019, 4, 4)
                },
                new Founder
                {
                    TaxpayerId = 927349824644,
                    LastName = "Тимофеев",
                    FirstName = "Эдуард",
                    Patronymic = "Станиславович",
                    CreationDate = new DateTime(2019, 4, 3)
                },
                new Founder
                {
                    TaxpayerId = 931563297390,
                    LastName = "Ерофеева",
                    FirstName = "Мстислава",
                    Patronymic = "Аркадьевна",
                    CreationDate = new DateTime(2019, 4, 2)
                },
                new Founder
                {
                    TaxpayerId = 946296369599,
                    LastName = "Никитина",
                    FirstName = "Марта",
                    Patronymic = "Вячеславовна",
                    CreationDate = new DateTime(2019, 4, 1)
                },
                new Founder
                {
                    TaxpayerId = 964331537437,
                    LastName = "Киселёва",
                    FirstName = "Флора",
                    Patronymic = "Степановна",
                    CreationDate = new DateTime(2019, 3, 31)
                },
                new Founder
                {
                    TaxpayerId = 931858976960,
                    LastName = "Васильева",
                    FirstName = "Полина",
                    Patronymic = "Львовна",
                    CreationDate = new DateTime(2019, 3, 30)
                },
                new Founder
                {
                    TaxpayerId = 922244724724,
                    LastName = "Семёнов",
                    FirstName = "Василий",
                    Patronymic = "Станиславович",
                    CreationDate = new DateTime(2019, 3, 29)
                },
                new Founder
                {
                    TaxpayerId = 921926953058,
                    LastName = "Богданов",
                    FirstName = "Феоктист",
                    Patronymic = "Леонидович",
                    CreationDate = new DateTime(2019, 3, 28)
                },
                new Founder
                {
                    TaxpayerId = 929243817587,
                    LastName = "Изофатова",
                    FirstName = "Лидия",
                    Patronymic = "Тарасовна",
                    CreationDate = new DateTime(2019, 3, 27)
                },
                new Founder
                {
                    TaxpayerId = 935325422871,
                    LastName = "Секунов",
                    FirstName = "Феликс",
                    Patronymic = "Геннадиевич",
                    CreationDate = new DateTime(2019, 3, 26)
                },
                new Founder
                {
                    TaxpayerId = 963771381782,
                    LastName = "Григорьева",
                    FirstName = "Яна",
                    Patronymic = "Егоровна",
                    CreationDate = new DateTime(2019, 3, 25)
                },
                new Founder
                {
                    TaxpayerId = 900940141641,
                    LastName = "Алексеев",
                    FirstName = "Андрей",
                    Patronymic = "Русланович",
                    CreationDate = new DateTime(2019, 3, 24)
                },
            };
        }
    }
}
