using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using InsuranceAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace InsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
            _env = env;
        }
        //GET API, to get all Insurance Types;
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT ClientId, ClientName, InsuranceType, convert(varchar(10), DateOfStart, 120) as DateOfStart, PhotoCI FROM dbo.Client";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("InsuranceConnection");
            SqlDataReader myReader;
            using (SqlConnection Con = new SqlConnection(sqlDataSource))
            {
                Con.Open();
                using (SqlCommand newCommand = new SqlCommand(query, Con))
                {
                    myReader = newCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    Con.Close();
                }
            }
            return new JsonResult(table);
        }
        //POST API, to post new types of insurance
        [HttpPost]
        public JsonResult POST(Client clt)
        {
            string query = @"INSERT INTO dbo.Client(ClientName, InsuranceType, DateOfStart, PhotoCI) values(@ClientName, @InsuranceType, @DateOfStart, @PhotoCI)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("InsuranceConnection");
            SqlDataReader myReader;
            using (SqlConnection Con = new SqlConnection(sqlDataSource))
            {
                Con.Open();
                using (SqlCommand newCommand = new SqlCommand(query, Con))
                {
                    newCommand.Parameters.AddWithValue("@ClientName", clt.ClientName);
                    newCommand.Parameters.AddWithValue("@InsuranceType", clt.InsuranceType);
                    newCommand.Parameters.AddWithValue("@DateOfStart", clt.DateOfStart);
                    newCommand.Parameters.AddWithValue("@PhotoCI", clt.PhotoCI);

                    myReader = newCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    Con.Close();
                }
            }
            return new JsonResult(table);
        }
        //PUT API, to update existing types of insurance
        [HttpPut]
        public JsonResult PUT(Client clt)
        {
            string query = @"UPDATE dbo.Client SET ClientName = @ClientName, InsuranceType=@InsuranceType, DateOfStart=@DateOfStart, PhotoCI=@PhotoCI WHERE ClientId=@ClientId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("InsuranceConnection");
            SqlDataReader myReader;
            using (SqlConnection Con = new SqlConnection(sqlDataSource))
            {
                Con.Open();
                using (SqlCommand newCommand = new SqlCommand(query, Con))
                {
                    newCommand.Parameters.AddWithValue("@ClientId", clt.ClientId);
                    newCommand.Parameters.AddWithValue("@ClientName", clt.ClientName);
                    newCommand.Parameters.AddWithValue("@InsuranceType", clt.InsuranceType);
                    newCommand.Parameters.AddWithValue("@DateOfStart", clt.DateOfStart);
                    newCommand.Parameters.AddWithValue("@PhotoCI", clt.PhotoCI);
                    myReader = newCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    Con.Close();
                }
            }
            return new JsonResult(table);
        }
        //DELETE API, to delete existing types of insurance
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM dbo.Client WHERE ClientId=@ClientId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("InsuranceConnection");
            SqlDataReader myReader;
            using (SqlConnection Con = new SqlConnection(sqlDataSource))
            {
                Con.Open();
                using (SqlCommand newCommand = new SqlCommand(query, Con))
                {
                    newCommand.Parameters.AddWithValue("@ClientId", id);
                    myReader = newCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    Con.Close();
                }
            }
            return new JsonResult(table);
        }
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/CI-photo/" + filename;

                using(var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
                catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

    }
}
