namespace WebApplication1.Http
{
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;

	public class DataController : Controller
    {
        [HttpGet]
        public IActionResult GetData()
        {
            // Your logic to fetch updated data
            var data = FetchUpdatedData(); // Implement this method to fetch your data

            return PartialView("_DataPartial", data); // Return a partial view with the updated data
        }

        private string FetchUpdatedData()
        {
            return System.DateTime.Now.ToString();
        }
	}
}
