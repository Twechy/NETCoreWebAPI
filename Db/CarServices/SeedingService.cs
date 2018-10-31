using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Db.CarServices.User;
using Db.DbContext;
using Db.DbContext.Db_Models;
using Db.DbContext.Db_Models.CarModels;
using Db.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Db.CarServices
{
    public class SeedingService
    {
        private readonly CarDb _dp;

        public static async Task SeedDbWithDummyCars(CarDb db)
        {
            var featList1 = new List<Feats>();
            var count1 = 0;
            for (var i = 0; i < 3; i++)
            {
                count1++;
                var feat = new Feats
                {
                    FeatName = new FeatName
                    {
                        Name = $"Feat Num {count1}"
                    }
                };
                featList1.Add(feat);
            }

            var user1 = await db.Users.FirstOrDefaultAsync(x => x.UserName.Equals("ansJamal")).ConfigureAwait(false);
            var model1 = new Model
            {
                Fule = new Fule
                {
                    Type = "Gas"
                },
                Year = "2012",
                Brand = "Toyota",
                Engine = new Engine
                {
                    Year = "2012",
                    Made = "Japan",
                    Name = "Toyota Motors"
                },
                Transmission = new Transmission
                {
                    Type = "Auto"
                },
                MadeM = new Made
                {
                    CarType = new CarType
                    {
                        Type = "Sedan"
                    },
                    MadeName = "Toyota"
                }
            };
            var car1 = new CarModel
            {
                City = new City
                {
                    CityName = "Tripoli"
                },
                Color = new Color
                {
                    ExtColor = "white",
                    IntColor = "black"
                },
                Feats = featList1,
                Price = "12500",
                Title = "",
                Country = new Country
                {
                    CountryName = "Libya"
                },
                Description = "",
                IsNew = true,
                PayType = new PayType
                {
                    Type = "cash"
                },
                DrivePath = new DrivePath
                {
                    Path = "12500"
                },
                AppUser = user1,
                Model = model1
            };

            await db.AddAsync(car1).ConfigureAwait(false);

            var featList2 = new List<Feats>();
            var count2 = 0;
            for (var i = 0; i < 3; i++)
            {
                count2++;
                var feat = new Feats
                {
                    FeatName = new FeatName
                    {
                        Name = $"Feat Num {count2}"
                    }
                };
                featList2.Add(feat);
            }

            var user2 = await db.Users.FirstOrDefaultAsync(x => x.UserName.Equals("ansJamal")).ConfigureAwait(false);
            var model2 = new Model
            {
                Fule = new Fule
                {
                    Type = "Gas"
                },
                Year = "2012",
                Brand = "Toyota",
                Engine = new Engine
                {
                    Year = "2012",
                    Made = "Japan",
                    Name = "Toyota Motors"
                },
                Transmission = new Transmission
                {
                    Type = "Auto"
                },
                MadeM = new Made
                {
                    CarType = new CarType
                    {
                        Type = "Sedan"
                    },
                    MadeName = "Toyota"
                }
            };
            var car2 = new CarModel
            {
                City = new City
                {
                    CityName = "Tripoli"
                },
                Color = new Color
                {
                    ExtColor = "white",
                    IntColor = "black"
                },
                Feats = featList2,
                Price = "12500",
                Title = "",
                Country = new Country
                {
                    CountryName = "Libya"
                },
                Description = "",
                IsNew = true,
                PayType = new PayType
                {
                    Type = "cash"
                },
                DrivePath = new DrivePath
                {
                    Path = "12500"
                },
                AppUser = user2,
                Model = model2
            };

            await db.AddAsync(car2).ConfigureAwait(false);

            var featList3 = new List<Feats>();
            var count3 = 0;
            for (var i = 0; i < 3; i++)
            {
                count3++;
                var feat = new Feats
                {
                    FeatName = new FeatName
                    {
                        Name = $"Feat Num {count3}"
                    }
                };
                featList3.Add(feat);
            }

            var user3 = await db.Users.FirstOrDefaultAsync(x => x.UserName.Equals("ansJamal")).ConfigureAwait(false);
            var model3 = new Model
            {
                Fule = new Fule
                {
                    Type = "Gas"
                },
                Year = "2012",
                Brand = "Toyota",
                Engine = new Engine
                {
                    Year = "2012",
                    Made = "Japan",
                    Name = "Toyota Motors"
                },
                Transmission = new Transmission
                {
                    Type = "Auto"
                },
                MadeM = new Made
                {
                    CarType = new CarType
                    {
                        Type = "Sedan"
                    },
                    MadeName = "Toyota"
                }
            };
            var car3 = new CarModel
            {
                City = new City
                {
                    CityName = "Tripoli"
                },
                Color = new Color
                {
                    ExtColor = "white",
                    IntColor = "black"
                },
                Feats = featList3,
                Price = "12500",
                Title = "",
                Country = new Country
                {
                    CountryName = "Libya"
                },
                Description = "",
                IsNew = true,
                PayType = new PayType
                {
                    Type = "cash"
                },
                DrivePath = new DrivePath
                {
                    Path = "12500"
                },
                AppUser = user3,
                Model = model3
            };

            await db.AddAsync(car3).ConfigureAwait(false);

            var featList4 = new List<Feats>();
            var count4 = 0;
            for (var i = 0; i < 3; i++)
            {
                count4++;
                var feat = new Feats
                {
                    FeatName = new FeatName
                    {
                        Name = $"Feat Num {count4}"
                    }
                };
                featList4.Add(feat);
            }

            var user4 = await db.Users.FirstOrDefaultAsync(x => x.UserName.Equals("haret")).ConfigureAwait(false);
            var model4 = new Model
            {
                Fule = new Fule
                {
                    Type = "Gas"
                },
                Year = "2012",
                Brand = "Toyota",
                Engine = new Engine
                {
                    Year = "2012",
                    Made = "Japan",
                    Name = "Toyota Motors"
                },
                Transmission = new Transmission
                {
                    Type = "Auto"
                },
                MadeM = new Made
                {
                    CarType = new CarType
                    {
                        Type = "Sedan"
                    },
                    MadeName = "Toyota"
                }
            };
            var car4 = new CarModel
            {
                City = new City
                {
                    CityName = "Tripoli"
                },
                Color = new Color
                {
                    ExtColor = "white",
                    IntColor = "black"
                },
                Feats = featList4,
                Price = "12500",
                Title = "",
                Country = new Country
                {
                    CountryName = "Libya"
                },
                Description = "",
                IsNew = true,
                PayType = new PayType
                {
                    Type = "cash"
                },
                DrivePath = new DrivePath
                {
                    Path = "12500"
                },
                AppUser = user4,
                Model = model4
            };

            await db.AddAsync(car4).ConfigureAwait(false);

            var featList5 = new List<Feats>();
            var count5 = 0;
            for (var i = 0; i < 3; i++)
            {
                count5++;
                var feat = new Feats
                {
                    FeatName = new FeatName
                    {
                        Name = $"Feat Num {count5}"
                    }
                };
                featList5.Add(feat);
            }

            var user5 = await db.Users.FirstOrDefaultAsync(x => x.UserName.Equals("haret")).ConfigureAwait(false);
            var model5 = new Model
            {
                Fule = new Fule
                {
                    Type = "Gas"
                },
                Year = "2012",
                Brand = "Toyota",
                Engine = new Engine
                {
                    Year = "2012",
                    Made = "Japan",
                    Name = "Toyota Motors"
                },
                Transmission = new Transmission
                {
                    Type = "Auto"
                },
                MadeM = new Made
                {
                    CarType = new CarType
                    {
                        Type = "Sedan"
                    },
                    MadeName = "Toyota"
                }
            };
            var car5 = new CarModel
            {
                City = new City
                {
                    CityName = "Tripoli"
                },
                Color = new Color
                {
                    ExtColor = "white",
                    IntColor = "black"
                },
                Feats = featList5,
                Price = "12500",
                Title = "",
                Country = new Country
                {
                    CountryName = "Libya"
                },
                Description = "",
                IsNew = true,
                PayType = new PayType
                {
                    Type = "cash"
                },
                DrivePath = new DrivePath
                {
                    Path = "12500"
                },
                AppUser = user5,
                Model = model5
            };

            await db.AddAsync(car5).ConfigureAwait(false);

            await db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}