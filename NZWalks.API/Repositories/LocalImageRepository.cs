using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        /**
         * IWebHostEnvironment provides information about the web hosting
         * environment in which an application is running.
         */

        /***
         * IHttpContextAccessor: we can use it to create a path to the image
         * that we have just uploaded.
         */

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.Name}{image.Extension}"); // Path.Combine() combines an array of strings into a path.
            // ex: C:\code\csharp\NZWalks\NZWalks.API\Images\image.png

            // Upload the image to Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream); // Asynchronously copies the contents of the uploaded file to the target stream

            /**
             * The local path is needed to open a stream and copy the file's content into it,
             * but in order to provide a path that can be stored in database to serve 
             * the image later, we need an accessible path such as https://localhost:1234/Images/image.jpg
             */

            // 2. Create a URL File Path

            // {scheme}://{host}{pathBase}/Images/{image.Name}
            // https://localhost:1234/Images/image.jpg
            var scheme = httpContextAccessor?.HttpContext?.Request.Scheme ?? string.Empty; // http or https
            var host = httpContextAccessor?.HttpContext?.Request.Host; // e.g. localhost:8888
            var pathBase = httpContextAccessor?.HttpContext?.Request.PathBase ?? string.Empty; // shouldn't end with a trailing slash
            // in our case the pathBase is empty string

            var urlFilePath = $"{scheme}://{host}{pathBase}/Images/{image.Name}{image.Extension}";

            // Store the Path in the Image domain model object.
            image.Path = urlFilePath;

            // Add the image to the Images table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
