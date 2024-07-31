using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using test2.Models.Entities;
using test2.Models.Mapping;
using test2.Services.HangHoaServices;

namespace test2.Controllers
{
    [Route("[controller]")]
    public class HH_PhieuNhapController : Controller
    {
        private readonly DemoNhaKhoaContext _context;
        private readonly IMapper _mapper;
        private readonly IHhPhieuNhapServices _services;

        public HH_PhieuNhapController(DemoNhaKhoaContext context, IMapper mapper, IHhPhieuNhapServices services)
        {
            _context = context;
            _mapper = mapper;
            _services = services;
        }

        [HttpPost("Modify")]
        public async Task<dynamic> Modify([FromBody] PhieuNhap data)
        {
            return await _services.Modify(data.PhieuNhapMap);
        }

        [HttpPost("Read")]
        public async Task<dynamic> Read()
        {
            return await _services.Read();
        }

    }
}
