﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RunGroupWebApp.Data.Enum;
using RunGroupWebApp.Models;
using System.Collections.Generic;

namespace RunGroupWebApp.Data
{
    public class Seed

    {
        
//            public static void SeedData(IApplicationBuilder applicationBuilder)
//            {
//                using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
//                {
//                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
//                    context.Database.EnsureCreated();
//                    if (!context.Clubs.Any())
//                    {
//                        context.Clubs.AddRange(new List<Club>()
//                    {
//                        new Club()
//                        {
//                            Title = "Running Club 1",
//                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                            Description = "This is the description of the first cinema",
//                            ClubCategory = ClubCategory.City,
//                            Address = new Address()
//                            {
//                                street = "Chatrapati Shivaji maharaj",
//                                city = "Sangli",
//                                state = "Maharashtra"
//                            }
//                         },
//                        new Club()
//                        {
//                            Title = "Running Club 2",
//                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                            Description = "This is the description of the first cinema",
//                            ClubCategory = ClubCategory.Endurance,
//                            Address = new Address()
//                            {
//                                 street = "Chatrapati Shahu maharaj",
//                                city = "Kolhapur",
//                                state = "Maharashtra"
//                            }
//                        },
//                        new Club()
//                        {
//                            Title = "Running Club 3",
//                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                            Description = "This is the description of the first club",
//                            ClubCategory = ClubCategory.Trail,
//                            Address = new Address()
//                            {
//                                 street = "Chatrapati Sambhaji maharaj",
//                                city = "Satara",
//                                state = "Maharashtra"
//                            }
//                        },
//                        new Club()
//                        {
//                            Title = "Running Club 3",
//                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                            Description = "This is the description of the first club",
//                            ClubCategory = ClubCategory.City,
//                            Address = new Address()
//                            {
//                                  street = "vir Baji Paslakar ",
//                                city = "Pune",
//                                state = "Maharashtra"
//                            }
//                        }
//                    });
//                        context.SaveChanges();
//                    }
//                    //Races
//                    if (!context.Races.Any())
//                    {
//                        context.Races.AddRange(new List<Race>()
//                    {
//                        new Race()
//                        {
//                            Title = "Running Race 1",
//                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                            Description = "This is the description of the first race",
//RaceCategory = RaceCategory.Marathon,
//                            Address = new Address()
//                            {
//                                  street = "Chatrapati shivaji maharaj",
//                                city = "Sangli",
//                                state = "Maharashtra"
//                            }
//                        },
//                        new Race()
//                        {
//                            Title = "Running Race 2",
//                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                            Description = "This is the description of the first race",
//                            RaceCategory = RaceCategory.Ultra,
//                            AddressId = 5,
//                            Address = new Address()
//                            {
//                                 street = "Chatrapati shivaji maharaj",
//                                city = "Sangli",
//                                state = "Maharashtra"
//                            }
//                        }
//                    });
//                        context.SaveChanges();
//                    }
//                }
//            }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "Admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "Vishal",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            street = "vishalchavan",
                            city="sangli",
                            state="maharashtra"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
                //string appUserEmail = "user@etickets.com";
                //var appUser = await userManager.FindByEmailAsync(appUserEmail);
                //if (appUser == null)
                //{
                //    var newAppUser = new AppUser()
                //    {
                //        UserName = "user",
                //        Email = appUserEmail,
                //        EmailConfirmed = true,
                //        Address = new Address()
                //        {
                //            street = "hinjewadi",
                //            city = "pune",
                //            state = "maharashtra"
                //        }
                //    };
                //    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                //    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                //}
            }
        }
    }
}
