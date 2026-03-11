using Microsoft.AspNetCore.Mvc;
using CRUDadoapp.Models;
using Microsoft.Data.SqlClient;

namespace CRUDadoapp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _cs;

        public EmployeeController(IConfiguration config)
        {
            _cs = config.GetConnectionString("DefaultConnection");
        }

        // READ
        public IActionResult Index()
        {
            List<Employee> list = new();

            try
            {
                using SqlConnection con = new SqlConnection(_cs);
                SqlCommand cmd = new SqlCommand("select * from Employees", con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Employee
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        Age = Convert.ToInt32(dr["Age"]),
                        Department = dr["Department"].ToString(),
                        City = dr["City"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(list);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        public IActionResult Create(Employee e)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_cs);
                SqlCommand cmd = new SqlCommand(
                    "insert into Employees values(@n,@g,@a,@d,@c)", con);

                cmd.Parameters.AddWithValue("@n", e.Name);
                cmd.Parameters.AddWithValue("@g", e.Gender);
                cmd.Parameters.AddWithValue("@a", e.Age);
                cmd.Parameters.AddWithValue("@d", e.Department);
                cmd.Parameters.AddWithValue("@c", e.City);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // EDIT - GET
        public IActionResult Edit(int id)
        {
            Employee e = new();

            try
            {
                using SqlConnection con = new SqlConnection(_cs);
                SqlCommand cmd = new SqlCommand(
                    "select * from Employees where Id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    e.Id = Convert.ToInt32(dr["Id"]);
                    e.Name = dr["Name"].ToString();
                    e.Gender = dr["Gender"].ToString();
                    e.Age = Convert.ToInt32(dr["Age"]);
                    e.Department = dr["Department"].ToString();
                    e.City = dr["City"].ToString();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(e);
        }

        // EDIT - POST
        [HttpPost]
        public IActionResult Edit(Employee e)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_cs);
                SqlCommand cmd = new SqlCommand(
                    "update Employees set Name=@n,Gender=@g,Age=@a,Department=@d,City=@c where Id=@id", con);

                cmd.Parameters.AddWithValue("@id", e.Id);
                cmd.Parameters.AddWithValue("@n", e.Name);
                cmd.Parameters.AddWithValue("@g", e.Gender);
                cmd.Parameters.AddWithValue("@a", e.Age);
                cmd.Parameters.AddWithValue("@d", e.Department);
                cmd.Parameters.AddWithValue("@c", e.City);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_cs);
                SqlCommand cmd = new SqlCommand(
                    "delete from Employees where Id=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
