using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelloCMS.Identity.UnitTests.Services
{
    public class UsersServicesTest
    {
        //Constructor
        public UsersServicesTest()
        {

        }


        //Unit Testing Methods



        //Register User Method Testing
        [Fact]
        [Trait("Category", "Users - Register")]
        public async Task Post_Users_Register_Successs()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Post_Users_Register_DuplicationEmailID()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Post_Users_Register_DuplicationUserName()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Post_Users_Register_PasswordNotMatching()
        {

            //Arrange


            //Act


            //Asset
        }



        //Get Users
        [Fact]
        public async Task Get_User_ByID_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Get_User_ByID_IDNotExists()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Get_User_ByName_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Get_User_ByEmail_Success()
        {

            //Arrange


            //Act


            //Asset
        }
        [Fact]
        public async Task Get_User_ByEmailOrName_EmailOrNameNotExists()
        {

            //Arrange


            //Act


            //Asset
        }



        //Get Users
        [Fact]
        public async Task Get_Users_All_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Get_Users_ByRoles_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Get_Users_ByRoles_RolesNotExists()
        {

            //Arrange


            //Act


            //Asset
        }


        //Update Users
        [Fact]
        public async Task Put_Users_Update_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Put_Users_Update_UserNotExists()
        {

            //Arrange


            //Act


            //Asset
        }



        //Delete Users
        [Fact]
        public async Task Delete_Users_ByID_Success()
        {

            //Arrange


            //Act


            //Asset
        }

        [Fact]
        public async Task Delete_Users_ByID_UserNotExists()
        {

            //Arrange


            //Act


            //Asset
        }
    }
}
