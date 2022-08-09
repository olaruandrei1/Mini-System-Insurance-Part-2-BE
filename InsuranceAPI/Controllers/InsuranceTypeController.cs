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

namespace InsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceTypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public InsuranceTypeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //GET API, to get all Insurance Types;
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT InsuranceId, InsuranceNameType FROM InsuranceType";
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
        public JsonResult POST(InsuranceType ins)
        {
            string query = @"INSERT INTO dbo.InsuranceType values(@InsuranceNameType)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("InsuranceConnection");
            SqlDataReader myReader;
            using (SqlConnection Con = new SqlConnection(sqlDataSource))
            {
                Con.Open();
                using (SqlCommand newCommand = new SqlCommand(query, Con))
                {
                    newCommand.Parameters.AddWithValue("@InsuranceNameType", ins.InsuranceNameType);
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
        public JsonResult PUT(InsuranceType ins)
        {
            string query = @"UPDATE dbo.InsuranceType SET InsuranceNameType = @InsuranceNameType WHERE InsuranceId=@InsuranceId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("InsuranceConnection");
            SqlDataReader myReader;
            using (SqlConnection Con = new SqlConnection(sqlDataSource))
            {
                Con.Open();
                using (SqlCommand newCommand = new SqlCommand(query, Con))
                {
                    newCommand.Parameters.AddWithValue("@InsuranceId", ins.InsuranceId);
                    newCommand.Parameters.AddWithValue("@InsuranceNameType", ins.InsuranceNameType);
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
            string query = @"DELETE FROM dbo.InsuranceType WHERE InsuranceId=@InsuranceId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("InsuranceConnection");
            SqlDataReader myReader;
            using (SqlConnection Con = new SqlConnection(sqlDataSource))
            {
                Con.Open();
                using (SqlCommand newCommand = new SqlCommand(query, Con))
                {
                    newCommand.Parameters.AddWithValue("@InsuranceId", id);
                    myReader = newCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    Con.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
