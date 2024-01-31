using Entity.DatabaseConn;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageHandlingController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public ImageHandlingController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile formFile, string code)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                string Filepath = GetFilepath(code);
                if (!Directory.Exists(Filepath))
                {
                    Directory.CreateDirectory(Filepath);
                }
                string imagepath = Filepath + "\\" + code + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Results = "pass";
                }
            }
            catch (Exception ex)
            {
                response.Errormessage = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost("MultipleUploadFile")]
        public async Task<IActionResult> MultipleUploadFile(IFormFileCollection formCollection, string code)
        {
            ApiResponse response = new ApiResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                string Filepath = GetFilepath(code);
                if (!Directory.Exists(Filepath))
                {
                    Directory.CreateDirectory(Filepath);
                }
                foreach (var file in formCollection)
                {
                    string imagepath = Filepath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passcount++;
                    }
                }
            }
            catch (Exception ex)
            {
                errorcount++;
                response.Errormessage = ex.Message;
            }
            response.ResponseCode = 200;
            response.Results = passcount + " Files uploaded & " + errorcount + " files failed";
            return Ok(response);
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string code)
        {
            string imageUrl = string.Empty;

            try
            {
                string filePath = GetFilepath(code);
                string imageFileName = code + ".png";
                string imageFullPath = Path.Combine(filePath, imageFileName);

                if (System.IO.File.Exists(imageFullPath))
                {
                    string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                    imageUrl = $"{hostUrl}/Upload/product/001/{imageFileName}";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

            return Ok(imageUrl);
        }

        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string code)
        {
            ApiResponse apiResponse = new ApiResponse();
            List<string> imageUrls = new List<string>();
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            try
            {
                string filePath = GetFilepath(code);

                if (System.IO.Directory.Exists(filePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imageFullPath = Path.Combine(filePath, filename);

                        if (System.IO.File.Exists(imageFullPath))
                        {
                            string imageUrl = $"{hostUrl}/Upload/product/{code}/{filename}";
                            imageUrls.Add(imageUrl);
                        }
                    }
                }
                else
                {
                    apiResponse.Errormessage = "The Folder is not fonud";
                    apiResponse.ResponseCode = 401;
                    return NotFound(apiResponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }

            return Ok(imageUrls);
        }

        [HttpGet("Dowmload")]
        public async Task<IActionResult> Download(string code)
        {
            try
            {
                string Filepath = GetFilepath(code);
                string imagepath = Filepath + "\\" + code + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    using (FileStream stream = new FileStream(imagepath, FileMode.Open))
                    {
                        stream.CopyToAsync(memoryStream);
                    }
                    memoryStream.Position = 0;
                    return File(memoryStream, "image/png", code + ".png");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost("DBMultiUploadImage")]
        public async Task<IActionResult> DBMultiUploadImage(IFormFileCollection filecollection, string code)
        {
            ApiResponse response = new ApiResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                foreach (var file in filecollection)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        _context.ImageUploads.Add(new ImageIploadModel()
                        {
                            Imagecode = code,
                            Images = stream.ToArray()
                        });
                        await _context.SaveChangesAsync();
                        passcount++;
                    }
                }
            }
            catch (Exception ex)
            {
                errorcount++;
                response.Errormessage = ex.Message;
            }
            response.ResponseCode = 200;
            response.Results = passcount + " Files uploaded &" + errorcount + " files failed";
            return Ok(response);
        }

        [HttpGet("GetDBMultiImage")]
        public async Task<IActionResult> GetDBMultiImage(string code)
        {
            List<string> Imageurl = new List<string>();
            try
            {
                var _productimage = _context.ImageUploads.Where(item => item.Imagecode == code).ToList();
                if (_productimage != null && _productimage.Count > 0)
                {
                    _productimage.ForEach(item =>
                    {
                        Imageurl.Add(Convert.ToBase64String(item.Images));
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);
        }

        [HttpGet("remove")]
        public async Task<IActionResult> remove(string code)
        {
            try
            {
                string filepath = GetFilepath(code);
                string imagepath = filepath + "\\" + code + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                    return Ok("Deleted sucessfully");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Multipleremove")]
        public async Task<IActionResult> Multipleremove(string code)
        {
            try
            {
                if (true)
                {
                    string filepath = GetFilepath(code);
                    DirectoryInfo directoryinfo = new DirectoryInfo(filepath);
                    FileInfo[] fileInfos = directoryinfo.GetFiles();
                    foreach (FileInfo file in fileInfos)
                    {
                        file.Delete();
                    }
                    return Ok("Delete Sucessfully");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("dbdownload")]
        public async Task<IActionResult> dbdownload(string code)
        {
            try
            {
                var _productimage = await _context.ImageUploads.FirstOrDefaultAsync(item => item.Imagecode == code);
                if (_productimage != null)
                {
                    return File(_productimage.Images, "image/png", code + ".png");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        private string GetFilepath(string code)
        {
            return this._webHostEnvironment.WebRootPath + "\\Upload\\product\\" + code;
        }
    }
}