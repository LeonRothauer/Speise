# Speise
First you have to install following 3 Packages (all in 7.0.15)

EntityFrameworkCore.Design
EntityFrameworkCore.SqLite
EntityFrameworkCore.Tools
Afterwards you create a SqLite Database Then an ApplicationDbContext File and add your Databases + Migration To make the Database complete, add following Lines in the Program.cs:

var connectionString = builder.Configuration.GetConnectionString("Name of Db"); builder.Services.AddDbContext(options => options.UseSqlite(connectionString));
