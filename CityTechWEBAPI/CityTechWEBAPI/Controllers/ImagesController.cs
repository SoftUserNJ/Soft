using CityTechWEBAPI.Models;
using CityTechWEBAPI.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityTechWEBAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly CityTechDevContext _dbContext;
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ImagesController(CityTechDevContext dbContext, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
		{
			_dbContext = dbContext;
			_configuration = configuration;
			_webHostEnvironment = webHostEnvironment;
		}



		[HttpGet("{articleNo}")]
		public IActionResult GetArticle(int articleNo)
		{
			
			var article = GetArticleFromDatabase(articleNo);

			if (article == null)
			{
				return NotFound();
			}

			article.ImgPath = Path.Combine("E:\\Project\\wwwroot", article.ImgPath);


			if (!System.IO.File.Exists(article.ImgPath))
			{
				article.ImgPath = null; 
			}

			return Ok(article);
		}


		private TblArticle GetArticleFromDatabase(int articleNo)
		{
			
			var article = _dbContext.TblArticles
				.FirstOrDefault(a => a.ArticleNo == articleNo);

			return article != null
				? new TblArticle
				{
					ArticleNo = article.ArticleNo,
					Name = article.Name,
					GroupId = article.GroupId,
					Uom = article.Uom,
					ImgPath = article.ImgPath
				}
				: null;
		}


	}
}

