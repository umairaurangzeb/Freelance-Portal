using Freelance_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static string LoginId;
        private string connection_str = "Data Source=DESKTOP-603H7IV;Initial Catalog=FLP;Integrated Security=True";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }
        // admin //
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        // admin view //

        public IActionResult loginUser()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //////////// -----    DB Functions    ------ ////////////////
        public string login(string email, string password)
        {
            try
            {
                String role = "";
                SqlConnection con = new SqlConnection(connection_str);
                con.Open();
                string query = "select * from tbl_Users where UserEmail = '" + email + "' and UserPassword = '" + password + "'";
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader sdr = com.ExecuteReader();
                if (sdr.Read())
                {
                    role = sdr["Role"].ToString();
                    role = role.ToLower();
                    LoginId = sdr["UserEmail"].ToString();
                    con.Close();
                    return role;
                }
                con.Close();
                return "";
            }

            catch (Exception e)
            {
                return "";
            }
        }
        public bool register(string username, string email, string password, string userType)
        {
            try
            {
                SqlConnection con = new SqlConnection(connection_str);
                con.Open();
                string query = "insert into AccountRequests values('" + username + "','" + email + "','" + password + "','" + userType + "','Pending')";
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public string getUserEmail()
        {
            return LoginId;
        }
        public bool postOrder(string orderType, string projectTitle, string pDescription, string skill, string compTime, string propRate)
        {
            SqlConnection con = new SqlConnection(connection_str);
            con.Open();
            string query = "insert into Buyer_Order values('" + projectTitle + "','" + pDescription + "','" + skill + "','" + compTime + "','" + propRate + "','" + LoginId + "','" + DateTime.Now.ToString() + "','','Pending','" + orderType + "')";
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            return true;

        }
        public List<order> getOrders()
        {
            List<order> list = new List<order>();
            order obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from Buyer_Order where status = 'Pending' order by Order_Id desc";

            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new order();
                obj.id = sdr["Order_Id"].ToString();
                obj.JobName = sdr["JobName"].ToString();
                obj.Description = sdr["Description"].ToString();
                obj.Skill = sdr["SkillNeed"].ToString();
                obj.completionTime = sdr["CompletionTime"].ToString();
                obj.proposedRate = sdr["ProposedRate"].ToString();
                obj.postedBy = sdr["PostedBy"].ToString();
                obj.postedDate = sdr["postDateTime"].ToString();
                obj.status = sdr["status"].ToString();
                obj.deliveryType = sdr["deliveryType"].ToString();
                list.Add(obj);

            }
            con.Close();
            return list;
        }
        public List<order> getActiveOrders()
        {
            List<order> list = new List<order>();
            order obj;
            SqlConnection con = new SqlConnection(connection_str);
            SqlConnection con1 = new SqlConnection(connection_str);
            string query1 = "select * from tbl_Bidding where status = 'Accepted' and FreeLancerId = '" + LoginId + "'  order by Order_Id desc";
            con1.Open();
            SqlCommand com1 = new SqlCommand(query1, con1);
            SqlDataReader sdr1 = com1.ExecuteReader();
            while (sdr1.Read())
            {
                string query = "select * from Buyer_Order where status = 'In Progress' and Order_Id  = " + sdr1["Order_Id"].ToString();
                con.Open();
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader sdr = com.ExecuteReader();
                while (sdr.Read())
                {
                    obj = new order();
                    obj.id = sdr["Order_Id"].ToString();
                    obj.JobName = sdr["JobName"].ToString();
                    obj.Description = sdr["Description"].ToString();
                    obj.Skill = sdr["SkillNeed"].ToString();
                    obj.completionTime = sdr["CompletionTime"].ToString();
                    obj.proposedRate = sdr["ProposedRate"].ToString();
                    obj.postedBy = sdr["PostedBy"].ToString();
                    obj.postedDate = sdr["postDateTime"].ToString();
                    obj.status = sdr["status"].ToString();
                    obj.deliveryType = sdr["deliveryType"].ToString();
                    list.Add(obj);

                }
                con.Close();
            }
            con1.Close();
            return list;
        }
        public List<order> getClientActiveOrders()
        {
            List<order> list = new List<order>();
            order obj;
            SqlConnection con = new SqlConnection(connection_str);
            SqlConnection con1 = new SqlConnection(connection_str);
            string query = "select * from Buyer_Order where status = 'In Progress' and PostedBy  = '" + LoginId + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new order();
                obj.id = sdr["Order_Id"].ToString();
                obj.JobName = sdr["JobName"].ToString();
                obj.Description = sdr["Description"].ToString();
                obj.Skill = sdr["SkillNeed"].ToString();
                obj.completionTime = sdr["CompletionTime"].ToString();
                obj.proposedRate = sdr["ProposedRate"].ToString();
                //obj.postedBy = sdr["PostedBy"].ToString();
                obj.postedDate = sdr["postDateTime"].ToString();
                obj.status = sdr["status"].ToString();
                obj.deliveryType = sdr["deliveryType"].ToString();
                query = "select * from tbl_Bidding where status = 'Accepted' and Order_Id  = '" + obj.id + "'";
                con1.Open();
                SqlCommand com1 = new SqlCommand(query, con1);
                SqlDataReader sdr1 = com1.ExecuteReader();
                while (sdr1.Read())
                {
                    obj.postedBy = sdr1["FreeLancerId"].ToString();
                }
                con1.Close();
                list.Add(obj);
            }
            con.Close();
            return list;
        }
        public bool deliverProject(string projId)
        {
            try
            {
                order obj = new order();
                SqlConnection con = new SqlConnection(connection_str);
                con.Open();
                string query = "update Buyer_Order set status = 'Completed' where Order_Id = "+projId;
                SqlCommand com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                con.Close();
                con.Open();
                query = "select * from Buyer_Order where Order_Id = " + projId;
                com = new SqlCommand(query, con);
                SqlDataReader sdr = com.ExecuteReader();
                while (sdr.Read())
                {
                    obj.postedBy = sdr["PostedBy"].ToString();
                }
                con.Close();
                con.Open();
                query = "select * from tbl_Bidding where status = 'Accepted' and Order_Id = " + projId;
                com = new SqlCommand(query, con);
                sdr = com.ExecuteReader();
                while (sdr.Read())
                {
                    obj.Description = sdr["FreeLancerId"].ToString();
                }
                con.Close();
                con.Open();
                query = "insert into tbl_Notification values ("+projId+",'"+obj.Description+"','"+obj.postedBy+"','Unread')";
                com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public List<Bid> getBids()
        {
            List<Bid> list = new List<Bid>();
            Bid obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Bidding where FreeLancerId = '" + LoginId + "' order by Bid_Id desc";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new Bid();
                obj.bid = sdr["Bid_Id"].ToString();
                obj.orderId = sdr["Order_Id"].ToString();
                obj.suggPrice = sdr["SuggestedPrice"].ToString();
                obj.deliveryDate = sdr["DeliveryDate"].ToString();
                obj.status = sdr["status"].ToString();
                obj.freeLancerId = sdr["FreeLancerId"].ToString();
                obj.desc = sdr["Decs"].ToString();
                list.Add(obj);
            }
            con.Close();
            return list;
        }
        public bool saveBid(string jobName, string orderId, string description, string proposedrate, string deliveryDate)
        {
            bool permission = true;
            SqlConnection con = new SqlConnection(connection_str);
            con.Open();
            string query = "select * from tbl_Bidding where Order_Id = " + orderId + " and FreeLancerId = '" + LoginId + "'";

            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            if (sdr.Read())
            {
                permission = false;
            }
            con.Close();
            if (!permission)
            {
                return false;
            }
            con.Open();
            query = "insert into tbl_Bidding values('" + orderId + "','" + proposedrate + "','" + deliveryDate + "','Pending','" + LoginId + "','" + description + "')";
            com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            return true;
        }
        public List<order> getMyOrders()
        {
            List<order> list = new List<order>();
            order obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from Buyer_Order where PostedBy = '" + LoginId + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new order();
                obj.id = sdr["Order_Id"].ToString();
                obj.JobName = sdr["JobName"].ToString();
                obj.Description = sdr["Description"].ToString();
                obj.Skill = sdr["SkillNeed"].ToString();
                obj.completionTime = sdr["CompletionTime"].ToString();
                obj.proposedRate = sdr["ProposedRate"].ToString();
                obj.postedBy = sdr["PostedBy"].ToString();
                obj.postedDate = sdr["postDateTime"].ToString();
                obj.status = sdr["status"].ToString();
                list.Add(obj);

            }
            con.Close();
            return list;
        }
        public List<order> getBidDetails(string orderId)
        {
            List<order> list = new List<order>();
            order obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Bidding where Order_Id = '" + orderId + "' and status = 'Pending'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new order();
                obj.id = sdr["Order_Id"].ToString();
                obj.JobName = sdr["SuggestedPrice"].ToString();
                obj.Description = sdr["Decs"].ToString();
                obj.Skill = sdr["FreeLancerId"].ToString();
                obj.postedDate = sdr["DeliveryDate"].ToString();
                string[] temp = obj.postedDate.Split(' ');
                obj.postedDate = temp[0];
                obj.status = sdr["status"].ToString();
                list.Add(obj);

            }
            con.Close();
            return list;
        }
        public void rejectBRequest(string orderId, string fid)
        {
            SqlConnection con = new SqlConnection(connection_str);
            string query = "update tbl_Bidding set status = 'Declined' where Order_Id='" + orderId + "' and FreeLancerId = '" + fid + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
        }
        public void acceptBRequest(string orderId, string fid)
        {
            SqlConnection con = new SqlConnection(connection_str);
            string query = "update tbl_Bidding set status = 'Declined' where Order_Id='" + orderId + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            query = "update tbl_Bidding set status = 'Accepted' where Order_Id='" + orderId + "' and FreeLancerId = '" + fid + "'";
            con.Open();
            com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            query = "update Buyer_Order set status = 'In Progress' where Order_Id='" + orderId + "'";
            con.Open();
            com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
        }
        public List<Bid> getNotifications()
        {
            List<Bid> list = new List<Bid>();
            Bid obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Bidding where FreeLancerId = '" + LoginId + "' and status='Accepted' order by Bid_Id desc";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new Bid();
                obj.bid = sdr["Bid_Id"].ToString();
                obj.orderId = sdr["Order_Id"].ToString();
                obj.suggPrice = sdr["SuggestedPrice"].ToString();
                obj.deliveryDate = sdr["DeliveryDate"].ToString();
                obj.status = sdr["status"].ToString();
                obj.freeLancerId = sdr["FreeLancerId"].ToString();
                obj.desc = sdr["Decs"].ToString();
                list.Add(obj);
            }
            con.Close();
            return list;
        }
        public User getProfileDetails()
        {
            User obj = new User();
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Users where UserEmail = '" + LoginId + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            if (sdr.Read())
            {
                obj.UserName = sdr["UserName"].ToString();
                obj.UserEmail = sdr["UserEmail"].ToString();
                obj.UserPassword = sdr["UserPassword"].ToString();
                obj.Role = sdr["Role"].ToString();
                obj.Status = sdr["Location"].ToString();
            }
            con.Close();
            return obj;
        }
        public bool saveFreelancer(string userName, string location, string role, string skill, string email)
        {
            SqlConnection con = new SqlConnection(connection_str);
            string query = "update FreelancerDetails set Skills='" + skill + "' where FreelancerId = '" + email + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            con.Open();
            query = "update tbl_Users set UserName = '" + userName + "',Location = '" + location + "',Role = '" + role + "' where UserEmail = '" + email + "'";
            com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            return true;
        }
        public bool saveGigDetails()
        {
            return true;
        }
        ///////////   ---------   Admin Functions    -------- ////////////
        public string getUsers()
        {
            int count = 0;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Users";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                count++;
            }
            con.Close();
            return count.ToString();
        }
        public List<User> getAccountRequests()
        {
            List<User> list = new List<User>();
            User obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from AccountRequests";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new User();
                obj.UserName = sdr["UserName"].ToString();
                obj.UserEmail = sdr["UserEmail"].ToString();
                obj.UserPassword = sdr["UserPassword"].ToString();
                obj.Role = sdr["Role"].ToString();
                obj.Status = sdr["Status"].ToString();
                list.Add(obj);

            }
            con.Close();
            return list;
        }
        public void approveRequest(string name, string email, string password, string role)
        {
            SqlConnection con = new SqlConnection(connection_str);
            string query = "update AccountRequests set status = 'Approved' where UserEmail='" + email + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            query = "insert into tbl_Users values('" + name + "','" + email + "','" + password + "','" + role + "','Pakistan')";
            con.Open();
            com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
            if (role == "freelancer")
            {
                con.Open();
                query = "insert into FreelancerDetails values('" + email + "','','')";
                com = new SqlCommand(query, con);
                com.ExecuteNonQuery();
                con.Close();
            }
        }
        public void declineRequest(string email)
        {
            SqlConnection con = new SqlConnection(connection_str);
            string query = "update AccountRequests set status = 'Declined' where UserEmail='" + email + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
        }
        public string getFreelancers()
        {
            int count = 0;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Users where Role='freelancer'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                count++;
            }
            con.Close();
            return count.ToString();
        }
        public string getClients()
        {
            int count = 0;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Users where Role='Client'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                count++;
            }
            con.Close();
            return count.ToString();
        }
        public List<User> getFreelancersDetail()
        {
            List<User> list = new List<User>();
            User obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Users where Role = 'freelancer'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new User();
                obj.UserName = sdr["UserName"].ToString();
                obj.UserEmail = sdr["UserEmail"].ToString();
                obj.UserPassword = sdr["UserPassword"].ToString();
                obj.Role = sdr["Role"].ToString();
                obj.Status = sdr["Location"].ToString();
                list.Add(obj);

            }
            con.Close();
            return list;
        }
        public List<User> getClientsDetail()
        {
            List<User> list = new List<User>();
            User obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Users where Role = 'Client'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new User();
                obj.UserName = sdr["UserName"].ToString();
                obj.UserEmail = sdr["UserEmail"].ToString();
                obj.UserPassword = sdr["UserPassword"].ToString();
                obj.Role = sdr["Role"].ToString();
                obj.Status = sdr["Location"].ToString();
                list.Add(obj);

            }
            con.Close();
            return list;
        }
        public List<User> getUsersDetail()
        {
            List<User> list = new List<User>();
            User obj;
            SqlConnection con = new SqlConnection(connection_str);
            string query = "select * from tbl_Users";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            SqlDataReader sdr = com.ExecuteReader();
            while (sdr.Read())
            {
                obj = new User();
                obj.UserName = sdr["UserName"].ToString();
                obj.UserEmail = sdr["UserEmail"].ToString();
                obj.UserPassword = sdr["UserPassword"].ToString();
                obj.Role = sdr["Role"].ToString();
                obj.Status = sdr["Location"].ToString();
                list.Add(obj);

            }
            con.Close();
            return list;
        }
        // edit function//
        public void updateRecord(string name, string email, string password, string role, string location)
        {
            SqlConnection con = new SqlConnection(connection_str);
            string query = "update tbl_Users set UserName = '" + name + "',UserEmail = '" + email + "',UserPassword = '" + password + "',Role = '" + role + "',Location = '" + location + "' where UserEmail='" + email + "'";
            con.Open();
            SqlCommand com = new SqlCommand(query, con);
            com.ExecuteNonQuery();
            con.Close();
        }
    }
}
