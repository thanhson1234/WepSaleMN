using AutoMapper;
using System.ComponentModel;
using System.Reflection;
using WebApp.Entities;
using WebApp.Models;

namespace WebApp.Helper
{
    public static class Utils
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<ProductViewModel, Product>();
                
                //Any Other Mapping Configuration ....
            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
        public static Mapper InitializeAutomapperCompany()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<CompanyViewModel, Company>();

                //Any Other Mapping Configuration ....
            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
		public static Mapper InitializeAutomapperCate()
		{
			//Provide all the Mapping Configuration
			var config = new MapperConfiguration(cfg =>
			{
				//Configuring Employee and EmployeeDTO
				cfg.CreateMap<CateViewModel, CategoryProduct>();

				//Any Other Mapping Configuration ....
			});

			//Create an Instance of Mapper and return that Instance
			var mapper = new Mapper(config);
			return mapper;
		}
        public static Mapper InitializeAutomapperBrand()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<BrandViewModel, Brand>();

                //Any Other Mapping Configuration ....
            });

            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
		public static Mapper InitializeAutomapperUser()
		{
			//Provide all the Mapping Configuration
			var config = new MapperConfiguration(cfg =>
			{
				//Configuring Employee and EmployeeDTO
				cfg.CreateMap<UserViewModel, AspNetUsers>();

				//Any Other Mapping Configuration ....
			});

			//Create an Instance of Mapper and return that Instance
			var mapper = new Mapper(config);
			return mapper;
		}

		public static Mapper InitializeAutomapperModule()
		{
			//Provide all the Mapping Configuration
			var config = new MapperConfiguration(cfg =>
			{
				//Configuring Employee and EmployeeDTO
				cfg.CreateMap<ModelViewModel, AspModule>();

				//Any Other Mapping Configuration ....
			});

			//Create an Instance of Mapper and return that Instance
			var mapper = new Mapper(config);
			return mapper;
		}
		public static Mapper InitializeAutomapperRoles()
		{
			//Provide all the Mapping Configuration
			var config = new MapperConfiguration(cfg =>
			{
				//Configuring Employee and EmployeeDTO
				cfg.CreateMap<RolesViewModel, AspNetRoles>();

				//Any Other Mapping Configuration ....
			});

			//Create an Instance of Mapper and return that Instance
			var mapper = new Mapper(config);
			return mapper;
		}
		public static Mapper InitializeAutomapperUnits()
		{
			//Provide all the Mapping Configuration
			var config = new MapperConfiguration(cfg =>
			{
				//Configuring Employee and EmployeeDTO
				cfg.CreateMap<UnitsViewModel, Units>();

				//Any Other Mapping Configuration ....
			});

			//Create an Instance of Mapper and return that Instance
			var mapper = new Mapper(config);
			return mapper;
		}
	}
	


public static class Enum_Status
    {
        public  enum Provide
		{
			[Description("Hà Nội")]
            Hanoi = 4,
            [Description("Hà Nội")]
            ProjectFunding = 3,
            TotalEmployee = 4,
            NumberOfServers = 5,
            TopBusinessConcern = 6
        }
    }
}
